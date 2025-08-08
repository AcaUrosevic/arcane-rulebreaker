using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform castPoint;
    public float fireballCooldown = 1.5f;
    private float lastCastTime;

    public GameObject iceSpellPrefab;
    public float iceCooldown = 5f;
    private float lastIceTime;


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastCastTime + fireballCooldown)
        {
            CastFireball();
        }
        if (Input.GetMouseButtonDown(1) && Time.time >= lastIceTime + iceCooldown)
        {
            CastIce();
            lastIceTime = Time.time;
        }
    }

    void CastFireball()
    {
        Instantiate(fireballPrefab, castPoint.position, transform.rotation);
        lastCastTime = Time.time;
    }

    void CastIce()
    {
        Instantiate(iceSpellPrefab, castPoint.position, transform.rotation);
    }

}
