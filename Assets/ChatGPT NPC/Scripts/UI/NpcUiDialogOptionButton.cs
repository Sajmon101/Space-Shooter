using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcUiDialogOptionButton : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Button _button;
	[SerializeField] private TMP_Text _label;

	private NpcUiDialogScreen _dialogScreen;
	private int _id;


    private void Start()
    {
		_button.onClick.AddListener(OnClickHandler);   
    }

    public void Initialize(NpcUiDialogScreen dialogScreen, int id, string message)
	{
		_dialogScreen = dialogScreen;
		_id = id;
		_label.text = message;
	}

	public void OnClickHandler()
	{
		_dialogScreen.ChooseOption(_id);
	}
}
