using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [Header("Scenes")]
    public string victorySceneName = "VictoryScene";
    public string loseSceneName    = "DefeatScene";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Victory()
    {
        Debug.Log("[GameFlow] VICTORY");
        if (!string.IsNullOrEmpty(victorySceneName))
            SceneManager.LoadScene(victorySceneName);
        else
            Debug.LogWarning("[GameFlow] Victory scene not set.");
    }

    public void Lose(string reason = null)
    {
        Debug.Log($"[GameFlow] LOSE{(string.IsNullOrEmpty(reason) ? "" : " – " + reason)}");
        if (!string.IsNullOrEmpty(loseSceneName))
            SceneManager.LoadScene(loseSceneName);
        else
            Debug.LogWarning("[GameFlow] Lose scene not set.");
    }
}
