using System.Collections.Generic;
using UnityEngine;

public class PlayerBlocker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<MonoBehaviour> scripts;

    public static PlayerBlocker Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void LockPlayer(bool locked)
    {
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = !locked;
        }
        LockCursor(!locked);
    }

    private void LockCursor(bool locked)
    {
        Cursor.visible = !locked;
        if (locked)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}
