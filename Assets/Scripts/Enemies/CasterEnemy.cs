using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(EnemyHealth))]
public class CasterEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2.4f;
    public float rotationSpeed = 10f;
    public float castRange = 7f;       // kad uđe u ovaj domet -> staje i castuje
    public float stopBuffer = 0.3f;    // malo da „ne uđe u nos“ playeru
    private float speedMultiplier = 1f;
    private float slowUntilTime  = 0f;

    [Header("Casting")]
    public GameObject enemyFireballPrefab;
    public Transform firePoint;        // child transform iz kog izlazi metak
    public float fireRate = 1.8f;      // vreme između castova
    public float castWindup = 0.35f;   // delay pre ispaljivanja (tajming animacije)
    public float castRecovery = 0.10f; // kratki „opоravak“ posle ispaljivanja

    private float lastCastTime = -999f;
    private Transform player;
    private Animator anim;

    void Awake()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;

        if (!anim) anim = GetComponentInChildren<Animator>();
        if (!firePoint) firePoint = transform; // fallback
    }

    void Update()
    {
        if (!player) return;

        if (Time.time > slowUntilTime) speedMultiplier = 1f;

        // vektor do playera (planarno)
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        float dist = toPlayer.magnitude;

        // rotacija ka playeru
        if (toPlayer.sqrMagnitude > 0.0001f)
        {
            var look = Quaternion.LookRotation(toPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, rotationSpeed * Time.deltaTime);
        }

        // ako smo van dometa -> kreći se ka playeru
        if (dist > castRange - stopBuffer)
        {
            Vector3 step = transform.forward * (moveSpeed * speedMultiplier) * Time.deltaTime;
            transform.position += step;

            float planarSpeed = new Vector3(step.x, 0f, step.z).magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
            if (anim) anim.SetFloat("Speed", planarSpeed);
            return;
        }

        // u dometu: stani i pokušaj cast
        if (anim) anim.SetFloat("Speed", 0f);

        if (Time.time >= lastCastTime + fireRate)
        {
            lastCastTime = Time.time;
            StartCoroutine(CastRoutine());
        }
    }

    IEnumerator CastRoutine()
    {
        // animacija cast-a preko Trigger-a
        if (anim) anim.SetTrigger("Cast");

        // sačekaj windup (sink sa animacijom), pa ispali
        yield return new WaitForSeconds(castWindup);

        FireNow();

        // kratak recovery pre sledećeg kretanja/casta
        yield return new WaitForSeconds(castRecovery);
    }

    private void FireNow()
    {
        if (!player || !enemyFireballPrefab || !firePoint) return;

        Vector3 dir = (player.position - firePoint.position);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) dir = transform.forward; else dir.Normalize();

        var go = Instantiate(enemyFireballPrefab, firePoint.position, Quaternion.LookRotation(dir));
        var efb = go.GetComponent<EnemyFireball>();
        if (efb != null) efb.SetDirection(dir);
    }

    // opcionalno: pozovi ovo iz EnemyHealth.Die() preko anim parametra
    public void PlayDeath()
    {
        if (anim) anim.SetTrigger("Dead");
    }
    public void Anim_FireNow()
    {
        // Ostavi prazno da ne radi ništa
    }

    public void ApplySlow(float slowMultiplier, float slowDuration)
    {
        speedMultiplier = Mathf.Clamp(slowMultiplier, 0.1f, 1f);
        slowUntilTime   = Time.time + slowDuration;
    }
}
