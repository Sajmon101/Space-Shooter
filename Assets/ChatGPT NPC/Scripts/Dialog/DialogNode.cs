using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialog
{
    public string Text;
    public List<DialogOption> Options;
}

[Serializable]
public class DialogOption
{
    public string Text;
    public Dialog Response;
}

[Serializable]
public class DialogList
{
    public List<Dialog> Dialog;
}
