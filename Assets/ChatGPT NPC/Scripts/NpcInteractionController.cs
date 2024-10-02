using Newtonsoft.Json;
using OpenAI;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteractionController
{
    [Header("References")]
    private NpcController npcController;
    private JSONReader reader;

    private ApiConfig apiConfig;
    private OpenAIApi openai;

    [Header("Parameters")]
    private int id;
    private string introduction;
    private string prompt;
    private List<Dialog> dialog;
    private List<ChatMessage> messages;
    private List<ChatMessage> uiMessages;

    public NpcInteractionController(NpcController controller, int npcId, string introductionText, string promptText)
    {
        npcController = controller;
        id = npcId;
        introduction = introductionText;
        prompt = promptText;

        reader = new JSONReader();
        dialog = new List<Dialog>(reader.LoadDialog(id));
        apiConfig = reader.LoadConfig();

        messages = new List<ChatMessage>();
        uiMessages = new List<ChatMessage>();
        openai = new OpenAIApi(apiConfig.api_key, apiConfig.organization);
    }

    public void ActivateMenu()
    {
        if (NpcUiController.Instance.ActivateMenu(this, introduction, dialog, new List<ChatMessage>(messages)))
        {
            NpcUiController.Instance.OnTalkingAction += npcController.NpcAnimator.PlayTalking;
            NpcUiController.Instance.OnThinkingAction += npcController.NpcAnimator.PlayThinking;
            npcController.NpcAnimator.PlayTalking();
            PlayerBlocker.Instance.LockPlayer(true);
        }
    }

    public void DeactivateMenu()
    {
        npcController.ChangeState(NpcState.Idle);
        NpcUiController.Instance.OnTalkingAction -= npcController.NpcAnimator.PlayTalking;
        NpcUiController.Instance.OnThinkingAction -= npcController.NpcAnimator.PlayThinking;
        PlayerBlocker.Instance.LockPlayer(false);
    }

    public async void SendReply(string prompt)
    {
        var newMessage = new ChatMessage()
        {
            Role = "user",
            Content = prompt
        };

        uiMessages.Add(newMessage);
        if (messages.Count == 0) newMessage.Content = this.prompt + "\n" + prompt;
        messages.Add(newMessage);

        CreateChatCompletionResponse completionResponse;
        try
        {
            completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                messages.Add(message);
                uiMessages.Add(message);

                // Send answer
                NpcUiController.Instance.ReceivePrompt(this, message.Content, GetChatMessagesCopy());
            }
            else
            {
                // Send empty response error
                NpcUiController.Instance.ReceivePrompt(this, "No text was generated from this prompt. Check if API key is provided and valid.", GetChatMessagesCopy());
                messages.RemoveAt(messages.Count - 1);
                uiMessages.RemoveAt(uiMessages.Count - 1);
            }
        }
        catch (JsonSerializationException)
        {
            // Send no connection error
            NpcUiController.Instance.ReceivePrompt(this, "No connection to the server. Check your internet connection.", GetChatMessagesCopy());
            messages.RemoveAt(messages.Count - 1);
            uiMessages.RemoveAt(uiMessages.Count - 1);
        }
    }

    public List<ChatMessage> GetChatMessagesCopy()
    {
        return new List<ChatMessage>(uiMessages);
    }
}
