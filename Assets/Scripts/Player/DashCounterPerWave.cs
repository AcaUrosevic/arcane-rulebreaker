using System;
using UnityEngine;

public class DashCounterPerWave : MonoBehaviour
{
    public int maxDashesPerWave = 3;
    private int dashCount = 0;

    public int Current => dashCount;
    public int Max => maxDashesPerWave;
    public event Action<int, int> OnDashCountChanged;

    bool subscribed = false;

    void OnEnable()
    {
        TrySubscribe();
    }

    void Start()
    {
        TrySubscribe();
        OnDashCountChanged?.Invoke(dashCount, maxDashesPerWave);
    }

    void OnDisable()
    {
        TryUnsubscribe();
    }

    void TrySubscribe()
    {
        if (subscribed) return;
        if (RoundManager.Instance == null) return;
        RoundManager.Instance.OnRoundStarted += ResetWave;
        subscribed = true;
    }

    void TryUnsubscribe()
    {
        if (!subscribed) return;
        if (RoundManager.Instance != null)
            RoundManager.Instance.OnRoundStarted -= ResetWave;
        subscribed = false;
    }

    public void OnDashed()
    {
        dashCount++;
        OnDashCountChanged?.Invoke(dashCount, maxDashesPerWave);
        Debug.Log($"[DashCounter] Dash count this wave: {dashCount}/{maxDashesPerWave}");

        if (dashCount > maxDashesPerWave)
        {
            RuleManager.Instance?.ReportViolation("Dash used more than 3 times this wave");
        }
    }

    void ResetWave()
    {
        dashCount = 0;
        OnDashCountChanged?.Invoke(dashCount, maxDashesPerWave);
        Debug.Log("[DashCounter] Wave reset → dashCount = 0");
    }
}
