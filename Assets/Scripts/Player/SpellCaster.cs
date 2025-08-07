using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform castPoint;
    public float fireballCooldown = 1.5f;
    private float lastCastTime;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastCastTime + fireballCooldown)
        {
            CastFireball();
        }
    }

    void CastFireball()
    {
        Instantiate(fireballPrefab, castPoint.position, transform.rotation);
        lastCastTime = Time.time;
    }
}
