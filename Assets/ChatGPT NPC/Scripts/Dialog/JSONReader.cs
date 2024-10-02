using System.Collections.Generic;
using UnityEngine;

public class JSONReader
{
    private List<Dialog> dialog;

    public List<Dialog> LoadDialog(int npcId)
    {
        string fileName = "dialog_" + npcId;

        TextAsset jsonData = Resources.Load<TextAsset>(fileName);

        if (jsonData != null)
        {
            DialogList dialogData = JsonUtility.FromJson<DialogList>(jsonData.text);
            dialog = dialogData.Dialog;
            return dialog;
        }
        else
        {
            Debug.LogError("File not found!");
            return null;
        }
    }

    public ApiConfig LoadConfig()
    {
        string fileName = "auth";
        TextAsset jsonData = Resources.Load<TextAsset>(fileName);

        if (jsonData != null)
        {
            ApiConfig apiData = JsonUtility.FromJson<ApiConfig>(jsonData.text);
            return apiData;
        }
        else
        {
            Debug.LogError("File not found!");
            return null;
        }
    }
}
