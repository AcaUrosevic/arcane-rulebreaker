using UnityEngine;

public class EnemyFireball : MonoBehaviour
{
    [Header("Motion")]
    public float speed = 11f;
    public float lifetime = 6f;

    [Header("Damage")]
    public float damage = 12f;

    // smer leta postavljamo spolja pri instanci
    private Vector3 dir = Vector3.forward;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    /// <summary>Postavi pravac i okreni projektil.</summary>
    public void SetDirection(Vector3 d)
    {
        dir = d.sqrMagnitude > 0.0001f ? d.normalized : transform.forward;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void OnTriggerEnter(Collider other)
    {
        // 1) IGNORE: neprijatelje (prođi kroz njih)
        if (other.CompareTag("Enemy") || other.GetComponentInParent<EnemyHealth>() != null)
            return;

        // 2) Sudar sa igračevim projektilima → obostrano uništenje
        if (other.GetComponent<Fireball>()     != null ||
            other.GetComponent<FrostOrb>()     != null ||
            other.GetComponent<HollowPurple>() != null)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }

        // 3) Pogodak igrača (na collideru ili parentu)
        var hp = other.GetComponent<PlayerHealth>() ?? other.GetComponentInParent<PlayerHealth>();
        if (hp != null)
        {
            hp.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // 4) Ostalo (zidovi/teren) – projektil NE SME da udari:
        // ovo rešavamo layer matrix-om (vidi uputstvo ispod), pa ovde ništa ne radimo.
    }
}
