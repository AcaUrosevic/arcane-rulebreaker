using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;

    Transform player;
    float slowMultiplier = 1f;
    float slowUntilTime = 0f;

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    void Update()
    {
        if (Time.time > slowUntilTime) slowMultiplier = 1f;

        if (!player) return;

        Vector3 toPlayer = (player.position - transform.position);
        toPlayer.y = 0f;

        if (toPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion look = Quaternion.LookRotation(toPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, rotationSpeed * Time.deltaTime);

            Vector3 step = transform.forward * (moveSpeed * slowMultiplier) * Time.deltaTime;
            transform.position += step;
        }
    }

    public void ApplySlow(float multiplier, float duration)
    {
        slowMultiplier = Mathf.Clamp(multiplier, 0.1f, 1f);
        slowUntilTime = Time.time + duration;
    }
}
