using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour, IHealth
{
    public float maxHealth = 50f;
    [System.NonSerialized] public float currentHealth;

    public Image healthBar;
    public UnityEvent OnHealthChanged;
    public UnityEvent OnDeath;
    private DamageEffect damageEffect;

    public Fade fade;
    public Transform SpawnPoint;
    //public Inventory inventory;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
        damageEffect = GetComponentInChildren<DamageEffect>();
    }

    public void ReduceHp(float damage)
    {
        damageEffect.StartEffect();

        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / maxHealth;

        OnHealthChanged.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        if (currentHealth + healAmount >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = currentHealth + healAmount;
        }

        healthBar.fillAmount = currentHealth / maxHealth;
    }

    void Die()
    {
        OnDeath.Invoke();

        if (transform.tag == "Player")
        {
            currentHealth = maxHealth;
            healthBar.fillAmount = currentHealth / maxHealth;

            if (SpawnPoint)
            {
                transform.position = SpawnPoint.position;

                Inventory inventory = transform.GetComponent<Inventory>();

                if(inventory != null)
                {
                    foreach (Weapon weapon in inventory.weapons)
                    {
                        Gun gun = weapon.GetComponentInChildren<Gun>();

                        if (gun != null)
                        {
                            gun.bulletsInClip = gun.clipCapacity;
                            gun.ammoText.text = gun.bulletsInClip.ToString("D3");
                        }
                    }
                }

            }
            else
            { 
                transform.position = new Vector3(0,0,0);
            }

            fade?.FadeOut(); 
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}