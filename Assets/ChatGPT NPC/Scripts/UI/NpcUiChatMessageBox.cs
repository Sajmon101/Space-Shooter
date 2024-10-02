using TMPro;
using UnityEngine;

public class NpcUiChatMessageBox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text _message;

    public void SetMessage(string message)
    {
        _message.text = message;
    }

    public void ClearMessage()
    {
        _message.text = string.Empty;
    }
}
