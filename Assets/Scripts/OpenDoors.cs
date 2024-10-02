using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            animator.SetBool("character_nearby", true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            animator.SetBool("character_nearby", false);
    }
}
