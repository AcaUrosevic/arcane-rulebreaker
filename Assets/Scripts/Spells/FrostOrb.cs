using UnityEngine;

public class FrostOrb : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 5f;
    public float slowRadius = 3f;
    public float slowAmount = 0.5f;
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, slowRadius);
        foreach (var hit in hitColliders)
        /*{
            if (hit.CompareTag("Enemy"))
            {
                EnemyMovement enemy = hit.GetComponent<EnemyMovement>();
                if (enemy != null)
                {
                    enemy.ApplySlow(slowAmount, slowDuration);
                }
            }
        }*/

        Destroy(gameObject);
    }
}
