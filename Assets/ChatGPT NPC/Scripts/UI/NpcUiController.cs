using System;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;

public class NpcUiController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NpcUiOptionScreen _optionScreen;
    [SerializeField] private NpcUiDialogScreen _dialogScreen;
    [SerializeField] private NpcUiChatScreen _chatScreen;

    private NpcInteractionController _currentInveractionController = null;
    private InteractionState _interactionState = InteractionState.None;

    public static NpcUiController Instance { get; private set; }

    public event Action<bool> OnThinkingAction;
    public event Action OnTalkingAction;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public bool ActivateMenu(NpcInteractionController interactionController, string introductionText, List<Dialog> dialogs, List<ChatMessage> messages)
    {
        if (_currentInveractionController != null)
        {
            Debug.LogWarning("The conversation is already underway!");
            return false;
        }

        _currentInveractionController = interactionController;

        // Initialize screens
        _optionScreen.InitializeScreen(introductionText);
        _dialogScreen.InitializeScreen(dialogs);
        _chatScreen.InitializeScreen(messages);

        ActivateScreen(InteractionState.Menu);
        return true;
    }

    public void DeactivateMenu()
    {
        _optionScreen.CleanScreen();
        _dialogScreen.CleanScreen();
        _chatScreen.CleanScreen();
        DeactivateAllScreensObjects();

        _currentInveractionController.DeactivateMenu();
        _currentInveractionController = null;
        _interactionState = InteractionState.None;
    }

    public void ActivateScreen(InteractionState interactionState)
    {
        DeactivateAllScreensObjects();

        switch (interactionState)
        {
            case InteractionState.Menu:
                _optionScreen.gameObject.SetActive(true);
                _optionScreen.ActivateScreen();
                break;

            case InteractionState.Dialog:
                _dialogScreen.gameObject.SetActive(true);
                _dialogScreen.ActivateScreen();
                break;

            case InteractionState.Chat:
                _chatScreen.gameObject.SetActive(true);
                _chatScreen.ActivateScreen(_currentInveractionController.GetChatMessagesCopy());
                break;

            case InteractionState.None:
                DeactivateMenu();
                break;
        }
        _interactionState = interactionState;
    }

    private void DeactivateAllScreensObjects()
    {
        _optionScreen.gameObject.SetActive(false);
        _dialogScreen.gameObject.SetActive(false);
        _chatScreen.gameObject.SetActive(false);
    }


    // Events handlers
    public void ExecuteOnTalkingAction()
    {
        OnTalkingAction?.Invoke();
    }

    public void ExecuteOnThinkingAction(bool isThinking)
    {
        OnThinkingAction?.Invoke(isThinking);
    }


    // Chat communication methods
    public void SendPrompt(string prompt)
    {
        try
        {
            _currentInveractionController.SendReply(prompt);
        }
        catch (NullReferenceException)
        {
            Debug.LogError("Interaction controller not found!");
        }
    }

    public void ReceivePrompt(NpcInteractionController npcInteraction, string anwser, List<ChatMessage> messages)
    {
        if (npcInteraction == _currentInveractionController)
        {
            _chatScreen.ReceivePrompt(anwser, messages);
        }
    }
}

public enum InteractionState { None, Menu, Dialog, Chat }