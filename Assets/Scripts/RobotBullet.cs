using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBullet : MonoBehaviour
{
    [SerializeField] int damage = 1;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision == null || collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        IHealth ihealth = collision.transform.GetComponent<IHealth>();
        if (ihealth == null)
        {
            ihealth = collision.transform.GetComponentInParent<IHealth>();
        }

        if (ihealth != null)
        {
            ihealth.ReduceHp(damage);
        }

        //Destroy(this.gameObject);
    }
}
