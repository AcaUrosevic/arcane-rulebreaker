using System;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [Header("Refs")]
    public Transform castPoint;
    public Camera mainCamera;

    [Header("Fireball")]
    public GameObject fireballPrefab;
    public float fireballCooldown = 1.5f;
    float lastFireballTime = -999f;

    [Header("Frost Orb")]
    public GameObject frostOrbPrefab;
    public float frostCooldown = 5f;
    float lastFrostTime = -999f;

    [Header("Hollow Purple")]
    public GameObject hollowPurplePrefab;
    public float hollowPurpleSpeed = 12f;
    public int hollowPurpleUsesPerRound = 3;
    int hollowPurpleUsesLeft;

    Animator anim;
    public event Action<int,int> OnHollowUsesChanged;
    public int HollowLeft => hollowPurpleUsesLeft;
    public int HollowPerRound => hollowPurpleUsesPerRound;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        hollowPurpleUsesLeft = hollowPurpleUsesPerRound;
        OnHollowUsesChanged?.Invoke(hollowPurpleUsesLeft, hollowPurpleUsesPerRound);
        if (RoundManager.Instance != null)
            RoundManager.Instance.OnRoundStarted += ResetHollowPurpleUses;
    }

    void OnDisable()
    {
        if (RoundManager.Instance != null)
            RoundManager.Instance.OnRoundStarted -= ResetHollowPurpleUses;
    }

    void ResetHollowPurpleUses()
    {
        hollowPurpleUsesLeft = hollowPurpleUsesPerRound;
        OnHollowUsesChanged?.Invoke(hollowPurpleUsesLeft, hollowPurpleUsesPerRound);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanCastFireball())
            CastFireball();

        if (Input.GetMouseButtonDown(1) && CanCastFrost())
            CastFrost();

        if (Input.GetKeyDown(KeyCode.Q) && CanCastHollowPurple())
            CastHollowPurple();
    }

    bool CanCastFireball() => Time.time >= lastFireballTime + fireballCooldown;
    bool CanCastFrost()    => Time.time >= lastFrostTime    + frostCooldown;

    bool CanCastHollowPurple()
    {
        bool fireOnCD  = !CanCastFireball();
        bool frostOnCD = !CanCastFrost();
        return fireOnCD && frostOnCD && hollowPurpleUsesLeft > 0;
    }

    void CastFireball()
    {
        anim?.SetTrigger("Cast");
        Instantiate(fireballPrefab, castPoint.position, transform.rotation);
        lastFireballTime = Time.time;
    }

    void CastFrost()
    {
        anim?.SetTrigger("Cast");
        Instantiate(frostOrbPrefab, castPoint.position, transform.rotation);
        lastFrostTime = Time.time;
        if (RuleManager.Instance) RuleManager.Instance.ReportViolation("Use of Spell #2 is forbidden");
    }

    void CastHollowPurple()
    {
        anim?.SetTrigger("Cast");
        var obj = Instantiate(hollowPurplePrefab, castPoint.position, transform.rotation);
        var hp = obj.GetComponent<HollowPurple>();
        if (hp != null) hp.speed = hollowPurpleSpeed;

        hollowPurpleUsesLeft--;
        OnHollowUsesChanged?.Invoke(hollowPurpleUsesLeft, hollowPurpleUsesPerRound);
    }
}
