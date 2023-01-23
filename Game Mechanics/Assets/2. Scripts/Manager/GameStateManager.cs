using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    [HideInInspector] public CharacterController player;
    [HideInInspector] public Ship baseObject;

    public GameObject playerUi;

    public TowerEco[] towerEcoObjs = new TowerEco[4];

    public bool levelStarted = false;
    public bool tutorialCompleted;

    public int waveTimer;
    public int waveTimerIntermission;
    public int timer;

    public Transform playerStartPos;
    public Transform playerTutorialStartPos;

    public List<GameObject> tutorialEnemies = new List<GameObject>();


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

        if (PlayerPrefs.GetInt("TutorialCompleted") == 1)
        {
            tutorialCompleted = true;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        player = FindObjectOfType<CharacterController>();
        baseObject = FindObjectOfType<Ship>();

        towerEcoObjs = FindObjectsOfType<TowerEco>(true);


        if (tutorialCompleted)
        {
            foreach (GameObject enemy in tutorialEnemies)
            {
                Destroy(enemy);
            }

            player.UpdateCoinAmount(5);
            player.health = player.maxHealth;
            player.UpdateHealthBar();

            player.transform.position = playerStartPos.position;
            player.transform.rotation = playerStartPos.rotation;
            player.transform.localScale = playerStartPos.localScale;

            EnableTowerEcos();
            tutorialEnemies.Clear();
            StartLevel();            
        }
    }

    public void WinCondition()
    {
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerUi.transform.GetChild(1).gameObject.SetActive(false);
        playerUi.transform.GetChild(3).gameObject.SetActive(true);
    }

    public void LoseCondition(int typeOfLoss)
    {
        Time.timeScale = 0;

        playerUi.transform.GetChild(1).gameObject.SetActive(false);
        playerUi.transform.GetChild(2).gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 0 = player died, 1 = ship died
        playerUi.transform.GetChild(2).transform.GetChild(typeOfLoss).gameObject.SetActive(true);

        if (SceneManager.GetActiveScene().buildIndex != 3)
        {

            playerUi.transform.GetChild(2).transform.GetChild(2).GetComponent<TMP_Text>().text = "Wave: "
                                                                              + EnemySpawnController.Instance.waveNumber
                                                                              + " / "
                                                                              + EnemySpawnController.Instance.maxWaves;
        }
        else
        {

            playerUi.transform.GetChild(2).transform.GetChild(2).GetComponent<TMP_Text>().text = "Wave: "
                                                                              + EnemySpawnController.Instance.waveNumber;

        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void Level1()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("TutorialCompleted", 0);
        SceneManager.LoadScene(1);
    }
    
    public void Level2()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        SceneManager.LoadScene(2);
    }
    
    public void Endless()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        SceneManager.LoadScene(2);
    }

    public void MainMenu()
    {
        PlayerPrefs.SetInt("TutorialCompleted", 0);
        Time.timeScale = 1;
        
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void EnableTowerEcos()
    {
        foreach (TowerEco towerEco in towerEcoObjs)
        {
            towerEco.gameObject.SetActive(true);
            towerEco.enabled = true;
            towerEco.StartCoroutine(towerEco.LookAt());
            towerEco.buyUi.transform.GetChild(2).GetComponent<TMP_Text>().color = Color.red;
        }
    }

    public void StartLevel()
    {
        PlayerPrefs.SetInt("TutorialCompleted", 1);

        StartCoroutine(CountDown(waveTimer));
    }
    
    public void StartWave()
    {
        if (EnemySpawnController.Instance.waveNumber >= 12)
        {
            foreach (Item item in Resources.FindObjectsOfTypeAll(typeof(Item)) as Item[])
            {
                item.moveToPlayer = true;
            }
        }

        if (EnemySpawnController.Instance.waveNumber == 4 || 
            EnemySpawnController.Instance.waveNumber == 7)
        {
            foreach (Item item in Resources.FindObjectsOfTypeAll(typeof(Item)) as Item[])
            {
                item.moveToPlayer = true;
            }

            StartCoroutine(CountDown(waveTimerIntermission));
            SpawnPortal.Instance.ChangePortalState(SpawnPortal.PortalStates.PASSIVE);
        }
        else
        {
            StartCoroutine(CountDown(waveTimer));
        }
    }
            

    public IEnumerator CountDown(int timerDuration)
    {
        SpawnPortal.Instance.ChangePortalState(SpawnPortal.PortalStates.ACTIVE);

        timer = timerDuration;
        playerUi.transform.GetChild(5).gameObject.SetActive(true);

        while (timer > 0)
        {
            playerUi.transform.GetChild(5).GetComponent<TMP_Text>().text = "" + timer;
            yield return new WaitForSeconds(1);
            timer--;
        }

        playerUi.transform.GetChild(5).gameObject.SetActive(false);

        if (!levelStarted)
        {
            EnemySpawnController.Instance.StartLevel();
            levelStarted = true;
        }
        else
            EnemySpawnController.Instance.StartCoroutine(EnemySpawnController.Instance.StartNewWave());

        SpawnPortal.Instance.ChangePortalState(SpawnPortal.PortalStates.AGRESSIVE);

        foreach (Item item in Resources.FindObjectsOfTypeAll(typeof(Item)) as Item[])
        {
            item.moveToPlayer = false;
        }
    }
}
