using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyMyself : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject, 2f);
    }
}
