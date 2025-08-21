using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [Header("Refs")]
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;

    [Header("Wave")]
    public int enemiesPerWave = 6;
    public float spawnInterval = 0.6f;

    [Header("Rounds")]
    public int maxRounds = 3;

    int alive;
    bool spawning;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(StartWaveNextFrame());
    }

    public void StartWave()
    {
        if (!spawning) StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        spawning = true;
        alive = 0;

        int currentRound = RoundManager.Instance != null ? RoundManager.Instance.currentRound : 1;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            var p = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var e = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            var enemyObj = Instantiate(e, p.position, p.rotation);
            alive++;

            // ✅ povećaj brzinu SAMO za ovu instancu (ne prefab!)
            var movement = enemyObj.GetComponent<EnemyMovement>();
            if (movement != null)
            {
                movement.moveSpeed *= Mathf.Pow(1.2f, currentRound - 1);
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        spawning = false;
    }

    public void OnEnemyDied()
    {
        alive = Mathf.Max(0, alive - 1);

        if (alive == 0 && !spawning)
        {
            if (RoundManager.Instance != null)
            {
                int current = RoundManager.Instance.currentRound;
                if (current >= maxRounds)
                {
                    GameFlowManager.Instance?.Victory();
                    return;
                }

                RoundManager.Instance.StartNewRound();
            }

            // ⛔ OVO SE BRIŠE jer kvari prefab:
            // foreach (var prefab in enemyPrefabs)
            // {
            //     var movement = prefab.GetComponent<EnemyMovement>();
            //     if (movement != null)
            //     {
            //         movement.moveSpeed *= 1.2f;
            //     }
            // }

            enemiesPerWave += 4;
            spawnInterval = Mathf.Max(0.25f, spawnInterval - 0.05f);

            StartWave();
        }
    }
    IEnumerator StartWaveNextFrame()
    {
        yield return null;  // сачекај један фрејм да Bootstrap одради reset
        StartWave();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
