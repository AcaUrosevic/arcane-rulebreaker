using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 50f;
    private float currentHealth;
    public Rigidbody rb;
    public float hitImpulse = 2f;

    void Awake()
    {
        currentHealth = maxHealth;
        if (!rb) rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void AddImpulse(Vector3 dir, float force)
    {
        if (rb) rb.AddForce(dir * force, ForceMode.Impulse);
    }

    void Die()
    {
        if (SpawnManager.Instance) SpawnManager.Instance.OnEnemyDied();
        Destroy(gameObject);
    }
}
