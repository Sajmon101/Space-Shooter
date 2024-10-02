using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcUiOptionScreen : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private NpcUiController _controller;

	[Space]
	[SerializeField] private Button _chatButton;
	[SerializeField] private Button _dialogButton;
	[SerializeField] private Button _exitButton;

	[Space]
	[SerializeField] private TMP_Text _invitationLabel;


    private void Start()
    {
		_chatButton.onClick.AddListener(() => _controller.ActivateScreen(InteractionState.Chat));
		_dialogButton.onClick.AddListener(() => _controller.ActivateScreen(InteractionState.Dialog));
		_exitButton.onClick.AddListener(_controller.DeactivateMenu);
    }

    public void InitializeScreen(string introductionText)
	{
		_invitationLabel.text = introductionText;
	}

	public void ActivateScreen()
	{
		_controller.ExecuteOnTalkingAction();
	}

	public void CleanScreen()
	{
        _invitationLabel.text = string.Empty;
	}
}
