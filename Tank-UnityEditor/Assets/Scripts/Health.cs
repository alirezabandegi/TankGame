using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 3f;
    public float damageFlashTime = 1f;
    public Color damageColor = Color.red;
    public GameObject explosion;

    private float currentHealth;
    private Color originalColor;
    private Color originalEmissionColor;
    private float t;

    private MeshRenderer[] meshRenderers;
    
    void Start()
    {
        currentHealth = maxHealth;
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        originalColor = meshRenderers[0].material.color;
        originalEmissionColor = meshRenderers[0].material.GetColor("_EmissionColor");
    }

    public void ReduceHealth(float damage)
    {
        StartCoroutine(DamageFlash());
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Instantiate(explosion, transform.position, new Quaternion());
            Destroy(gameObject);
        }
    }

    private IEnumerator DamageFlash()
    {
        t = damageFlashTime;
        while(t > 0)
        {
            t -= Time.deltaTime;

            Color newColor = Color.Lerp(originalColor, damageColor, t / damageFlashTime);
            Color newEmissionColor = Color.Lerp(originalEmissionColor, damageColor, t / damageFlashTime);

            foreach (MeshRenderer r in meshRenderers)
            {
                r.material.color = newColor;
                r.material.SetColor("_EmissionColor", newEmissionColor);
            }
            yield return null;
        }
        foreach (MeshRenderer r in meshRenderers)
        {
            r.material.color = originalColor;
            r.material.SetColor("_EmissionColor", originalEmissionColor);
        }
    }
    
}
