using GDL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotHp : MonoBehaviour, IHealth
{
    private Slider slider;
    public float maxHp = 10;
    public float hp;
    [SerializeField] Animator anim;
    EnemyBrain enemyBrain;

    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();
    private Dictionary<Renderer, Material> originalMaterials = new Dictionary<Renderer, Material>();
    public float flashDuration = 1.0f;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        anim = GetComponent<Animator>();
        enemyBrain = GetComponent<EnemyBrain>();
        slider.value = 1;
        hp = maxHp;
    }

    private void Start()
    {
        // Clone materials for each renderer and store the original color
        foreach (Transform child in transform)
        {
            SkinnedMeshRenderer renderer = child.GetComponent<SkinnedMeshRenderer>();
            if (renderer != null)
            {
                Material clonedMaterial = new Material(renderer.material);
                renderer.material = clonedMaterial;

                originalMaterials[renderer] = clonedMaterial;
                originalColors[renderer] = clonedMaterial.color;
            }
        }
    }
    public void ReduceHp(float hp)
    {
        FlashRed();
        this.hp -= hp;
        slider.value -= hp/maxHp;

        if (this.hp<0) Dead();
        enemyBrain.isChasing = true;
        enemyBrain.isPatrolling = false;
    }

    private void Dead()
    {
        enemyBrain.IsAlive = false;
        anim.SetBool("alive", false);
        Destroy(gameObject, 5f);
    }

    public void FlashRed()
    {
        foreach (KeyValuePair<Renderer, Material> pair in originalMaterials)
        {
            StartCoroutine(Flash(pair.Key, pair.Value, flashDuration));
        }
    }

    private IEnumerator Flash(Renderer renderer, Material material, float flashDuration)
    {
        // Flash to red
        material.color = new Color(1.0f, 0.298f, 0.298f, 1.0f);

        // Wait a bit and then start fading back to the original color
        yield return new WaitForSeconds(flashDuration / 2);

        Color originalColor = originalColors[renderer];
        float elapsedTime = 0;
        while (elapsedTime < flashDuration / 2)
        {
            material.color = Color.Lerp(Color.red, originalColor, elapsedTime / (flashDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restore the original color
        material.color = originalColor;
    }

}
