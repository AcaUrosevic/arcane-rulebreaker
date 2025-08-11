using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;

    [Header("Attack")]
    public float attackRange = 1.8f;
    public float attackCooldown = 1.0f;
    public float attackDamage = 10f;
    public float attackHitDelay = 0.2f;
    private float lastAttackTime;

    [Header("Status Effects")]
    private float slowMultiplier = 1f;
    private float slowUntilTime = 0f;

    private Transform player;
    public Animator anim;

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;

        if (!anim) anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Time.time > slowUntilTime) slowMultiplier = 1f;

        if (!player) return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        float dist = toPlayer.magnitude;

        if (toPlayer.sqrMagnitude > 0.0001f)
        {
            Quaternion look = Quaternion.LookRotation(toPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, rotationSpeed * Time.deltaTime);
        }

        float speedNow = 0f;

        if (dist > attackRange * 0.95f)
        {
            Vector3 step = transform.forward * (moveSpeed * slowMultiplier) * Time.deltaTime;
            transform.position += step;

            Vector3 planarStep = new Vector3(step.x, 0f, step.z);
            speedNow = planarStep.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        }
        else
        {
            speedNow = 0f;
            TryAttack();
        }

        if (anim) anim.SetFloat("Speed", speedNow);
    }

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        lastAttackTime = Time.time;
        if (anim) anim.SetTrigger("Attack");

        StartCoroutine(MeleeHitAfterDelay());
    }

    IEnumerator MeleeHitAfterDelay()
    {
        yield return new WaitForSeconds(attackHitDelay);

        if (!player) yield break;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;

        if (toPlayer.magnitude <= attackRange + 0.2f)
        {
            var hp = player.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(attackDamage);
            }
            else
            {
                Debug.Log("[Enemy] Player hit, but no PlayerHealth found.");
            }
        }
    }

    public void ApplySlow(float multiplier, float duration)
    {
        slowMultiplier = Mathf.Clamp(multiplier, 0.1f, 1f);
        slowUntilTime = Time.time + duration;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}
