using UnityEngine;

public class FrostOrb : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;

    public float slowRadius = 3f;
    public float slowMultiplier = 0.5f;
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
        Collider[] hits = Physics.OverlapSphere(transform.position, slowRadius, ~0, QueryTriggerInteraction.Ignore);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var mover = hit.GetComponent<EnemyMovement>();
                if (mover != null)
                {
                    mover.ApplySlow(slowMultiplier, slowDuration);
                }
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, slowRadius);
    }
}
