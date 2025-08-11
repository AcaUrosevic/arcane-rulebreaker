using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float maxHP = 100f;
    public float currentHP;

    [Header("Death")]
    public float deathDelay = 1.0f;
    public Animator anim;

    [Header("Damage Gates")]
    public bool invulnerable = false; 
    public float minHitInterval = 0.1f;
    float lastHitTime = -999f;

    void Awake()
    {
        currentHP = maxHP;
        if (!anim) anim = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(float amount)
    {
        if (invulnerable) return;
        if (Time.time < lastHitTime + minHitInterval) return;

        lastHitTime = Time.time;
        currentHP = Mathf.Max(0f, currentHP - amount);
        Debug.Log($"[Player] HP {currentHP}/{maxHP} (-{amount})");


        if (currentHP <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        Debug.Log($"[Player] Heal -> {currentHP}/{maxHP}");
    }

    void Die()
    {
        Debug.Log("[Player] DEAD");

        if (anim) anim.SetBool("Dead", true);

        var sp = GetComponent<SpellCaster>(); if (sp) sp.enabled = false;
        var mv = GetComponent<PlayerMovement>(); if (mv) mv.enabled = false;
        var dc = GetComponent<DashController>(); if (dc) dc.enabled = false;

    }

    public void SetInvulnerable(bool value)
    {
        invulnerable = value;
    }
}
