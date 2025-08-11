using UnityEngine;
using System;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    [Header("Timer")]
    public float startSeconds = 180f;
    public bool autoStart = true;

    public event Action<int,int> OnTimeChanged;
    public event Action OnTimeOver;

    float timeLeft;
    bool running;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        timeLeft = startSeconds;
    }

    void Start()
    {
        if (autoStart) StartTimer();
    }

    public void StartTimer()
    {
        running = true;
        EmitTime();
    }

    public void StopTimer() => running = false;

    void Update()
    {
        if (!running) return;
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            running = false;
            EmitTime();
            RuleManager.Instance?.TriggerGameOver("Time over");
            Debug.Log("[Timer] Time over.");
            return;
        }
        EmitTime();
    }

    void EmitTime()
    {
        int m = Mathf.FloorToInt(timeLeft / 60f);
        int s = Mathf.FloorToInt(timeLeft % 60f);
        OnTimeChanged?.Invoke(m, s);
    }

    public void ResetAndStart(float seconds)
    {
        timeLeft = seconds;
        StartTimer();
    }
}
