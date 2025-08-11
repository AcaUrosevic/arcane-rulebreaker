using UnityEngine;

public class StandingStillChecker : MonoBehaviour
{
    public float maxStillSeconds = 3f;
    public float speedThreshold = 0.05f;

    Vector3 lastPos;
    float stillTimer;

    void Start(){ lastPos = transform.position; }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, lastPos);
        if (dist < speedThreshold * Time.deltaTime)
            stillTimer += Time.deltaTime;
        else
            stillTimer = 0f;

        lastPos = transform.position;

        if (stillTimer >= maxStillSeconds)
        {
            RuleManager.Instance?.ReportViolation("Standing still > 3s");
            stillTimer = 0f;
        }
    }
}
