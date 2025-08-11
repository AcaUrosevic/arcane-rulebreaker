using UnityEngine;
using System;

public class RuleManager : MonoBehaviour
{
    public static RuleManager Instance { get; private set; }

    [Header("Tokens")]
    public int allowedViolations = 3;
    public int currentViolations = 0;

    public event Action<int, int> OnTokensChanged;   // current,max
    public event Action<string> OnRuleViolated;      // poruka
    public event Action OnGameOver;                  // subscribe spolja, ali Invoke samo ovde

    public bool IsGameOver { get; private set; } = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        OnTokensChanged?.Invoke(currentViolations, allowedViolations);
    }

    public void ReportViolation(string ruleName)
    {
        currentViolations++;
        OnTokensChanged?.Invoke(currentViolations, allowedViolations);
        OnRuleViolated?.Invoke(ruleName);

        if (currentViolations > allowedViolations)
        {
            TriggerGameOver("Too many rule violations");
        }
    }

    public void TriggerGameOver(string reason = null)
    {
        if (IsGameOver) return;
        IsGameOver = true;

        OnGameOver?.Invoke();
        if (!string.IsNullOrEmpty(reason))
            Debug.Log($"[RuleManager] GAME OVER – {reason}");
        else
            Debug.Log("[RuleManager] GAME OVER");
    }

}
