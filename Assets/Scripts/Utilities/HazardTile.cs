using UnityEngine;

public class HazardTile : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RuleManager.Instance?.ReportViolation("Stepped on forbidden tile");
        }
    }
}
