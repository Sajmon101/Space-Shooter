using UnityEngine;

public class NpcAnimator : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] protected Animator _animator;

    [ContextMenu("Play thinking")]
    public void PlayThinking(bool isThinking)
    {
        _animator.SetBool("Thinking", isThinking);
        Debug.Log($"{gameObject.name} thinking");
    }

    [ContextMenu("Play talking")]
    public void PlayTalking()
    {
        _animator.SetTrigger("Talking");
        Debug.Log($"{gameObject.name} talking");
    }
}
