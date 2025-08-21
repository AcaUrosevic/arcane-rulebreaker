using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayBootstrap : MonoBehaviour
{
    void Awake()
    {
        // 1) Врати време у нормалу — иначе корутини / Update неће радити
        Time.timeScale = 1f;
        // (опционо) reset неких глобалних ствари
        Physics.autoSimulation = true;
    }

    void OnEnable()
    {
        // Осигурај да се binding-и ураде након што се сви синглтони пробуде
        StartCoroutine(BootNextFrame());
    }

    IEnumerator BootNextFrame()
    {
        yield return null;


        if (GameTimer.Instance != null)
        {
            GameTimer.Instance.StopTimer();
            GameTimer.Instance.ResetAndStart(GameTimer.Instance.startSeconds);
        }

        // 3) Покрени први талас (ако већ није стартован)
        if (SpawnManager.Instance != null)
            SpawnManager.Instance.StartWave();
    }
}
