using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [Header("Scenes")]
    public string[] levelScenes = { "GameScene", "Level3", "Level4" };
    public string victorySceneName = "VictoryScene";
    public string loseSceneName    = "DefeatScene";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject); // da preživi prelazak između scena
    }

    public void Victory()
    {
        Debug.Log("[GameFlow] VICTORY");

        string currentScene = SceneManager.GetActiveScene().name;
        int index = System.Array.IndexOf(levelScenes, currentScene);

        if (index >= 0 && index < levelScenes.Length - 1)
        {
            // Učitaj sledeći nivo
            string nextScene = levelScenes[index + 1];
            Debug.Log("[GameFlow] Loading next level: " + nextScene);
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            // Ako nema sledećeg nivoa → victory
            if (!string.IsNullOrEmpty(victorySceneName))
                SceneManager.LoadScene(victorySceneName);
            else
                Debug.LogWarning("[GameFlow] Victory scene not set.");
        }
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
