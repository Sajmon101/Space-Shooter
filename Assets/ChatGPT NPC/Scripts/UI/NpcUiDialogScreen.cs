using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcUiDialogScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NpcUiController _controller;

    [Space]
    [SerializeField] private GameObject _optionsContainer;
    [SerializeField] private TMP_Text _answerLabel;

    [Header("Prefabs")]
    [SerializeField] private GameObject _optionPrefab;

    private List<Dialog> _dialogs;
    private Dialog _currentDialog;


    public void InitializeScreen(List<Dialog> dialogs)
    {
        _dialogs = dialogs;
    }

    public void ActivateScreen()
    {
        try
        {
            PrintDialog(_dialogs[0]);
        }
        catch (NullReferenceException)
        {
            _controller.ActivateScreen(InteractionState.Menu);
            Debug.LogError("Invalid dialog input!");
        }
        catch (ArgumentOutOfRangeException)
        {
            _controller.ActivateScreen(InteractionState.Menu);
            Debug.Log("Character has no dialog options!");
        }
    }

    public void CleanScreen()
    {
        _dialogs = null;
        CleanCurrentOptions();
    }

    private void PrintDialog(Dialog dialog)
    {
        CleanCurrentOptions();
        _controller.ExecuteOnTalkingAction();

        _answerLabel.text = dialog.Text;
        List<DialogOption> options = dialog.Options;
        for (int i = 0; i < options.Count; i++)
        {
            int index = i;
            AddButtonToList(index, options[i].Text);
        }

        // Exit button
        AddButtonToList(options.Count, "Exit");
        _currentDialog = dialog;
    }

    public void ChooseOption(int index)
    {
        // This is exit button
        if (index == _currentDialog.Options.Count)
        {
            _controller.ActivateScreen(InteractionState.Menu);
        }
        else
        {
            PrintDialog(_currentDialog.Options[index].Response);
        }
    }

    private void AddButtonToList(int index, string message)
    {
        GameObject optionObject = Instantiate(_optionPrefab);
        optionObject.transform.SetParent(_optionsContainer.transform, false);

        NpcUiDialogOptionButton button = optionObject.GetComponent<NpcUiDialogOptionButton>();
        if (button != null)
        {
            button.Initialize(this, index, message);
        }
        else
        {
            Debug.LogError("Invalid button prefab!");
        }
    }

    private void CleanCurrentOptions()
    {
        int optionsContainerChildernCount = _optionsContainer.transform.childCount;
        for (int i = 0; i < optionsContainerChildernCount; i++)
        {
            Destroy(_optionsContainer.transform.GetChild(i).gameObject);
        }

        _answerLabel.text = string.Empty;
    }
}
