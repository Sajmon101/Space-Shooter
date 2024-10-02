using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f; // Adjust this value as needed
    public float damage = 1.0f;

    [HideInInspector] public Vector3 movementDirection;

    private void Start()
    {
        // Initialization if needed
    }

    public void OnSpawn(RaycastHit hit)
    {
        StartCoroutine(MoveBullet(hit));
    }

    private IEnumerator MoveBullet(RaycastHit hit)
    {
        float distance = Vector3.Distance(transform.position, hit.point);
        float timeToReachTarget = distance / speed;

        float time = 0f;
        Vector3 startPosition = transform.position;

        while (time < 1f)
        {
            time += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startPosition, hit.point, time);

            yield return null;
        }

        transform.position = hit.point;
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (collision.transform.TryGetComponent(out IHealth ihealth) || collision.transform.parent.TryGetComponent(out ihealth))
        {
            ihealth.ReduceHp(damage);
        }

        Destroy(gameObject);
    }
}
