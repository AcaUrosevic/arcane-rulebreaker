using UnityEngine;

public class DashCounterPerWave : MonoBehaviour
{
    public int maxDashesPerWave = 3;
    private int dashCount = 0;

    void OnEnable()
    {
        if (RoundManager.Instance) RoundManager.Instance.OnRoundStarted += ResetWave;
    }
    void OnDisable()
    {
        if (RoundManager.Instance) RoundManager.Instance.OnRoundStarted -= ResetWave;
    }

    public void OnDashed()
    {
        dashCount++;
        Debug.Log($"[DashCounter] Dash count this wave: {dashCount}/{maxDashesPerWave}");

        if (dashCount > maxDashesPerWave)
        {
            RuleManager.Instance?.ReportViolation("Dash used more than 3 times this wave");
        }
    }

    void ResetWave()
    {
        dashCount = 0;
        Debug.Log("[DashCounter] Wave reset → dashCount = 0");
    }
}
