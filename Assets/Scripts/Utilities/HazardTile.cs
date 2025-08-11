using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HazardTile : MonoBehaviour
{
    [Header("Damage")]
    public float damagePerTick = 5f;
    public float tickInterval = 0.5f;
    public bool dealOnceOnEnter = false;

    private readonly Dictionary<PlayerHealth, Coroutine> running = new();

    void Reset()
    {
        var c = GetComponent<Collider>();
        if (c) c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var hp = other.GetComponent<PlayerHealth>() ?? other.GetComponentInParent<PlayerHealth>();
        if (hp == null) return;

        RuleManager.Instance?.ReportViolation("Stepped on forbidden tile");

        if (dealOnceOnEnter)
        {
            hp.TakeDamage(damagePerTick);
            return;
        }

        if (!running.ContainsKey(hp))
        {
            running[hp] = StartCoroutine(DamageOverTime(hp));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var hp = other.GetComponent<PlayerHealth>() ?? other.GetComponentInParent<PlayerHealth>();
        if (hp == null) return;

        if (running.TryGetValue(hp, out var co))
        {
            StopCoroutine(co);
            running.Remove(hp);
        }
    }

    IEnumerator DamageOverTime(PlayerHealth hp)
    {
        while (true)
        {
            hp.TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
    }

    void OnDisable()
    {
        foreach (var kv in running)
            if (kv.Value != null) StopCoroutine(kv.Value);
        running.Clear();
    }
}
