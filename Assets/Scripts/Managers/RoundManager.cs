using UnityEngine;
using System;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }
    public event Action OnRoundStarted;

    [Header("Debug")]
    public int currentRound = 1;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void StartNewRound()
    {
        currentRound++;
        OnRoundStarted?.Invoke();
        Debug.Log($"[RoundManager] New round: {currentRound}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            StartNewRound();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

}
