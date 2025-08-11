using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [Header("Refs")]
    public PlayerHealth playerHealth;
    public RuleManager ruleManager;
    public DashCounterPerWave dashCounter;
    public SpellCaster spellCaster;
    public GameTimer gameTimer;

    [Header("UI")]
    public Slider hpBar;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI hollowText;
    public TextMeshProUGUI dashText;
    public TextMeshProUGUI rulesText;

    void Awake()
    {
        if (!playerHealth) playerHealth = FindObjectOfType<PlayerHealth>();
        if (!ruleManager)  ruleManager  = RuleManager.Instance;
        if (!dashCounter)  dashCounter  = FindObjectOfType<DashCounterPerWave>();
        if (!spellCaster)  spellCaster  = FindObjectOfType<SpellCaster>();
        if (!gameTimer)    gameTimer    = GameTimer.Instance;
    }

    void OnEnable()
    {
        if (playerHealth != null) playerHealth.OnHealthChanged += OnHealthChanged;
        if (ruleManager  != null) ruleManager.OnTokensChanged   += OnTokensChanged;
        if (dashCounter  != null) dashCounter.OnDashCountChanged+= OnDashChanged;
        if (spellCaster  != null) spellCaster.OnHollowUsesChanged += OnHollowChanged;
        if (gameTimer    != null) gameTimer.OnTimeChanged += OnTimeChanged;
    }

    void OnDisable()
    {
        if (playerHealth != null) playerHealth.OnHealthChanged -= OnHealthChanged;
        if (ruleManager  != null) ruleManager.OnTokensChanged   -= OnTokensChanged;
        if (dashCounter  != null) dashCounter.OnDashCountChanged-= OnDashChanged;
        if (spellCaster  != null) spellCaster.OnHollowUsesChanged -= OnHollowChanged;
        if (gameTimer    != null) gameTimer.OnTimeChanged -= OnTimeChanged;
    }

    void Start()
    {
        if (playerHealth) OnHealthChanged(playerHealth.currentHP, playerHealth.maxHP);
        if (ruleManager)  OnTokensChanged(ruleManager.currentViolations, ruleManager.allowedViolations);
        if (dashCounter)  OnDashChanged(dashCounter.Current, dashCounter.Max);
        if (spellCaster)  OnHollowChanged(spellCaster.HollowLeft, spellCaster.HollowPerRound);
        if (gameTimer)    gameTimer.StartTimer();
    }

    // EVENTS → UI

    void OnHealthChanged(float cur, float max)
    {
        if (!hpBar) return;
        hpBar.maxValue = max;
        hpBar.value = cur;
    }

    void OnTokensChanged(int current, int max)
    {
        if (rulesText) rulesText.text = $"Rules: {current}/{max}";
    }

    void OnDashChanged(int current, int max)
    {
        if (dashText) dashText.text = $"Dash: {current}/{max}";
    }

    void OnHollowChanged(int left, int perRound)
    {
        if (hollowText) hollowText.text = $"Hollow: {left}/{perRound}";
    }

    void OnTimeChanged(int minutes, int seconds)
    {
        if (!timerText) return;
        timerText.text = $"{minutes:0}:{seconds:00}";
    }
}
