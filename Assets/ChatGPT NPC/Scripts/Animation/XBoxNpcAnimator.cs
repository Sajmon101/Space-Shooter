using System.Collections;
using UnityEngine;

public class XBoxNpcAnimator : NpcAnimator
{
    [Header("Settings")]
    [Range(2, 20)] [SerializeField] private float minDwarfTimeout = 3.0f;
    [Range(2, 20)] [SerializeField] private float maxDwarfTimeout = 15.0f;

    private IEnumerator _currentTimer = null;

    private void Start()
    {
        _currentTimer = DwarfTimeout();
        StartCoroutine(_currentTimer);
    }

    private void OnDisable()
    {
        if (_currentTimer != null)
        {
            StopCoroutine(_currentTimer);
            _currentTimer = null;
        }
    }

    [ContextMenu("Play dwarf")]
    public void PlayDwarfIdle()
    {
        _animator.SetTrigger("Dwarf");
        Debug.Log($"{gameObject.name} trigger");
    }

    private IEnumerator DwarfTimeout()
    {
        float timeout = Random.Range(minDwarfTimeout, maxDwarfTimeout);
        yield return new WaitForSeconds(timeout);

        PlayDwarfIdle();
        _currentTimer = DwarfTimeout();
        StopCoroutine(_currentTimer);
    }
}
