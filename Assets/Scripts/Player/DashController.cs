using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerMovement))]
public class DashController : MonoBehaviour
{
    public KeyCode dashKey = KeyCode.Space;
    public float dashSpeed = 18f;
    public float dashDuration = 0.20f;
    public float dashCooldown = 1.0f;

    private PlayerMovement movement;
    private DashCounterPerWave dashCounter;
    private bool isDashing = false;
    private float lastDashTime = -999f;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        dashCounter = GetComponent<DashCounterPerWave>();
    }

    void Update()
    {
        if (Input.GetKeyDown(dashKey))
        {
            TryDash();
        }
    }

    void TryDash()
    {
        if (isDashing) return;
        if (Time.time < lastDashTime + dashCooldown) return;

        StartCoroutine(DashRoutine());
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        lastDashTime = Time.time;

        Vector3 dir = movement.MoveInputRaw.sqrMagnitude > 0.001f
            ? movement.MoveInputRaw.normalized
            : transform.forward;

        movement.SetExternalVelocity(dir * dashSpeed);
        Debug.Log("[Dash] START");

        dashCounter?.OnDashed();


        yield return new WaitForSeconds(dashDuration);

        movement.SetExternalVelocity(Vector3.zero);
        Debug.Log("[Dash] END");

        isDashing = false;
    }
}
