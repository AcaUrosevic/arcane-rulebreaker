using System;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public float hitImpulse = 10f;
    public float damage = 25f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var dmg = other.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }

            var eh = other.GetComponent<EnemyHealth>();
            if (eh != null)
            {
                Vector3 dir = (other.transform.position - transform.position).normalized;
                dir.y = 0f;
                eh.AddImpulse(dir, hitImpulse);
            }
        }

        Destroy(gameObject);
    }
}
