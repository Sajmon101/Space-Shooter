using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OpenAI;
using System.Collections.Generic;
using System.Collections;

public class NpcUiChatScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NpcUiController _controller;

    [Space]
    [SerializeField] private GameObject _conversationContainer;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private TMP_InputField _promptInputField;
    [SerializeField] private Button _sendButton;
    [SerializeField] private Button _exitButton;

    [Header("Prefabs")]
    [SerializeField] private GameObject _chatMessagePrefab;
    [SerializeField] private GameObject _playerMessagePrefab;

    [Header("Settings")]
    [Range(0.01f, 120f)]
    [SerializeField] private float _responseTimeout = 90.0f;

    private RectTransform _conversationContainerTransform;
    private List<ChatMessage> _messages;

    private NpcUiChatMessageBox _lastPopUp = null;
    private IEnumerator _ongoingPromptRequest = null;
    private bool _waitingForResponse = false;


    // Unity methods
    private void Start()
    {
        _sendButton.onClick.AddListener(SendPrompt);
        _exitButton.onClick.AddListener(() => _controller.ActivateScreen(InteractionState.Menu));

        _conversationContainerTransform = _conversationContainer.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendPrompt();
        }
    }


    // Npc screen methods
    public void InitializeScreen(List<ChatMessage> messages)
    {
        _messages = messages;
    }

    public void ActivateScreen(List<ChatMessage> messages)
    {
        _messages = messages;
        _promptInputField.Select();
        DisplayMessages(_messages);
    }

    public void CleanScreen()
    {
        _messages = null;
        _lastPopUp = null;
        _ongoingPromptRequest = null;
        _waitingForResponse = false;
        CleanChat();
    }


    // Prompt communication methods
    private void SendPrompt()
    {
        if (_ongoingPromptRequest == null)
        {
            _ongoingPromptRequest = AsyncSendPrompt();
            StartCoroutine(_ongoingPromptRequest);
        }
    }

    private IEnumerator AsyncSendPrompt()
    {
        string promptMessage = _promptInputField.text;
        if (promptMessage != string.Empty)
        {
            _promptInputField.text = string.Empty;
            InitMessage(promptMessage, false);
            InitMessage("Thinking...", true);
            _controller.ExecuteOnThinkingAction(true);

            _controller.SendPrompt(promptMessage);
            yield return WaitingForResponseProcedure();
        }
        _ongoingPromptRequest = null;
    }

    public void ReceivePrompt(string prompt, List<ChatMessage> messages)
    {
        _messages = messages;
        _waitingForResponse = false;
        _controller.ExecuteOnThinkingAction(false);
        _controller.ExecuteOnTalkingAction();

        _lastPopUp.SetMessage(prompt);
        StartCoroutine(ScrollView(new Vector2(0, 0)));     // Scroll to bottom
    }


    // Chat management methods
    private void DisplayMessages(List<ChatMessage> messages)
    {
        CleanChat();
        if (messages.Count > 0)
        {
            _controller.ExecuteOnTalkingAction(); 
        }

        foreach (ChatMessage message in messages)
        {
            bool isChat = (message.Role != "user");
            InitMessage(message.Content, isChat);
        }
    }

    private void InitMessage(string message, bool isChat)
    {
        GameObject newMessage = null;
        if (isChat)
        {
            newMessage = Instantiate(_chatMessagePrefab);
        }
        else
        {
            newMessage = Instantiate(_playerMessagePrefab);
        }
        newMessage.transform.SetParent(_conversationContainer.transform, false);

        NpcUiChatMessageBox messageBox = newMessage.GetComponent<NpcUiChatMessageBox>();
        if (messageBox != null)
        {
            _lastPopUp = messageBox;
            messageBox.SetMessage(message);
            StartCoroutine(ScrollView(new Vector2(0, 0)));     // Scroll to bottom
        }
        else
        {
            Debug.LogError("Invalid message box prefab!");
        }
    }

    private void CleanChat()
    {
        int optionsContainerChildernCount = _conversationContainer.transform.childCount;
        for (int i = 0; i < optionsContainerChildernCount; i++)
        {
            Destroy(_conversationContainer.transform.GetChild(i).gameObject);
        }

        _promptInputField.text = string.Empty;
    }

    private IEnumerator ScrollView(Vector2 scrollPosition)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_conversationContainerTransform);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        _scrollRect.normalizedPosition = scrollPosition.normalized;
    }

    private IEnumerator WaitingForResponseProcedure()
    {
        _waitingForResponse = true;

        // Block UI
        _sendButton.interactable = false;
        _exitButton.interactable = false;
        _promptInputField.interactable = false;

        // Waiting for response timeout
        float elapsedTime = 0.0f;
        while (elapsedTime < _responseTimeout)
        {
            elapsedTime += Time.deltaTime;
            if (!_waitingForResponse)
            {
                break;
            }
            yield return null;
        }

        // Unblock UI
        _sendButton.interactable = true;
        _exitButton.interactable = true;
        _promptInputField.interactable = true;
        _promptInputField.Select();

        // Package expired
        if (_waitingForResponse)
        {
            _waitingForResponse = false;
            _lastPopUp.SetMessage("Expired package. Check your internet connection");
            StartCoroutine(ScrollView(new Vector2(0, 0)));     // Scroll to bottom
        }
    }
}
