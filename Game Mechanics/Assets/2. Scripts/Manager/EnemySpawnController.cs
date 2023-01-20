using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawnController : MonoBehaviour
{
    public static EnemySpawnController Instance;

    [HideInInspector] public GameObject meleeEnemy;
    [HideInInspector] public GameObject rangedEnemy;

    private List<GameObject> enemies = new List<GameObject>();
    private List<Transform> spawnList = new List<Transform>();
    private List<Transform> enemiesSpawned = new List<Transform>();

    public int waveNumber;
    public int maxWaves;
    public float rampUpSpeed;
    private int amountOfEnemies;
    public int amountOfEnemiesAlive;

    [InspectorButton("OnButtonClicked")]
    public bool spawnWave;

    public int spawnWaveInterval;
    [HideInInspector] public float spawnWaveTime = 0f;

    public TMP_Text waveNumberTmp;


    private void OnButtonClicked()
    {
        SpawnWave();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        meleeEnemy = Resources.Load("Enemies/MeleeEnemy") as GameObject;
        rangedEnemy = Resources.Load("Enemies/RangedEnemy") as GameObject;

        enemies.Add(meleeEnemy);
        enemies.Add(rangedEnemy);

        waveNumber = 1;

        waveNumberTmp.text = "Wave: " + waveNumber + " / " + maxWaves;
    }

    public void SpawnWave()
    {
        if (waveNumber == maxWaves)
            GameStateManager.Instance.WinCondition();

        amountOfEnemies = 1 + (int)(rampUpSpeed * Mathf.Pow(waveNumber, 2f));

        if (amountOfEnemies >= spawnList.Count)
            amountOfEnemies = spawnList.Count;

        amountOfEnemiesAlive = amountOfEnemies;

        SpawnEnemies(amountOfEnemies);

        waveNumberTmp.text = "Wave: " + waveNumber + " / " + maxWaves;
        waveNumber++;
    }

    public IEnumerator StartNewWave()
    {
        ResetForNextWave();
        yield return new WaitForSeconds(spawnWaveInterval);
        SpawnWave();
    }

    public void SpawnEnemies(int enemyAmount)
    {
        int maxMeleeEnemies = (int)(0.33f * enemyAmount);
        int maxRangedEnemies = enemyAmount - maxMeleeEnemies;

        for (int i = 0; i < maxRangedEnemies; i++)
        {
            var spawnedEnemy = Instantiate(rangedEnemy, RandomSpawnPoint(), false);
            spawnedEnemy.transform.SetParent(transform, true);
        }

        for (int i = 0; i < maxMeleeEnemies; i++)
        {
            var spawnedEnemy = Instantiate(meleeEnemy, RandomSpawnPoint(), false);
            spawnedEnemy.transform.SetParent(transform, true);
        }
    }

    public Transform RandomSpawnPoint()
    {
        Transform spawnPoint = spawnList[Random.Range(0, spawnList.Count)];

        spawnList.Remove(spawnPoint);

        return spawnPoint;
    }

    public void ResetForNextWave()
    {
        spawnList.Clear();

        foreach (Transform spawnPoint in transform.GetChild(0))
        {
            spawnList.Add(spawnPoint);
        }
    }
}
