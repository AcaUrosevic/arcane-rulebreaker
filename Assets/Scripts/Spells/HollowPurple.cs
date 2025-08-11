using UnityEngine;

public class HollowPurple : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 12f;
    public float lifetime = 5f;

    [Header("Impact")]
    public float impactRadius = 3.5f;
    public float damage = 35f;
    public float slowMultiplier = 0.6f;
    public float slowDuration = 3f;
    

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, impactRadius, ~0, QueryTriggerInteraction.Ignore);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var dmg = hit.GetComponent<IDamageable>();
                if (dmg != null) dmg.TakeDamage(damage);

                var mover = hit.GetComponent<EnemyMovement>();
                if (mover != null) mover.ApplySlow(slowMultiplier, slowDuration);
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.7f, 0.2f, 1f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}
