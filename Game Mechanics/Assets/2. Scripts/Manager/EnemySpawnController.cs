using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

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
    public float spawnSingleEnemyInterval;

    public int startAmountOffEnemies;

    [InspectorButton("OnButtonClicked")]
    public bool spawnWave;

    public int spawnWaveInterval;
    [HideInInspector] public float spawnWaveTime = 0f;

    public TMP_Text waveNumberTmp;

    public UnityEvent m_EnemyDied;


    private void OnButtonClicked()
    {
        StartLevel();
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

        if (m_EnemyDied == null)
            m_EnemyDied = new UnityEvent();

        m_EnemyDied.AddListener(UpdateEnemyAmount);
    }


    public void StartLevel()
    {
        waveNumberTmp.gameObject.SetActive(true);
        
        waveNumber = 1;

        StartCoroutine(StartNewWave());
    }

    public IEnumerator StartNewWave()
    {
        ResetForNextWave();
        yield return null;//new WaitForSeconds(spawnWaveInterval);
        SpawnWave();
    }

    public void SpawnWave()
    {
        if (waveNumber == maxWaves + 2)
        {
            GameStateManager.Instance.WinCondition();
            return;
        }
            

        amountOfEnemies = startAmountOffEnemies + (int)(rampUpSpeed * Mathf.Pow(waveNumber, 2f));

        /*if (amountOfEnemies >= spawnList.Count)
            amountOfEnemies = spawnList.Count;*/

        amountOfEnemiesAlive = amountOfEnemies;

        SpawnEnemies(amountOfEnemies);

        waveNumberTmp.text = "Wave: " + waveNumber + " / " + maxWaves;
        waveNumber++;
    }

    public void SpawnEnemies(int enemyAmount)
    {
        int maxMeleeEnemies = (int)(0.33f * enemyAmount);
        int maxRangedEnemies = enemyAmount - maxMeleeEnemies;

        StartCoroutine(SpawnRangedEnemy(maxRangedEnemies));
        StartCoroutine(SpawnMeleeEnemy(maxMeleeEnemies));        
    }

    public IEnumerator SpawnRangedEnemy(int maxRangedEnemies)
    {
        for (int i = 0; i < maxRangedEnemies; i++)
        {
            var spawnedEnemy = Instantiate(rangedEnemy, RandomSpawnPoint(), false);
            spawnedEnemy.transform.SetParent(transform, true);
            yield return new WaitForSeconds(spawnSingleEnemyInterval);
        }
    }
    public IEnumerator SpawnMeleeEnemy(int maxMeleeEnemies)
    {
        for (int i = 0; i < maxMeleeEnemies; i++)
        {
            var spawnedEnemy = Instantiate(meleeEnemy, RandomSpawnPoint(), false);
            spawnedEnemy.transform.SetParent(transform, true);
            yield return new WaitForSeconds(spawnSingleEnemyInterval);
        }
    }




    public Transform RandomSpawnPoint()
    {
        Transform spawnPoint = spawnList[Random.Range(0, spawnList.Count)];

        //spawnList.Remove(spawnPoint);

        return spawnPoint;
    }

    public void ResetForNextWave()
    {
        amountOfEnemiesAlive = 0;
        spawnList.Clear();

        foreach (Transform spawnPoint in transform.GetChild(0))
        {
            spawnList.Add(spawnPoint);
        }
    }

    public void UpdateEnemyAmount()
    {
        amountOfEnemiesAlive--;

        if (!GameStateManager.Instance.levelStarted && amountOfEnemiesAlive <= 0)
            GameStateManager.Instance.EnableTowerEcos();

        if (GameStateManager.Instance.levelStarted && amountOfEnemiesAlive <= 0)
            GameStateManager.Instance.StartWave();
    }
}
