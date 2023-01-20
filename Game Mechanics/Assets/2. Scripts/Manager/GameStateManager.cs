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

    private void Start()
    {
        player = FindObjectOfType<CharacterController>();
        baseObject = FindObjectOfType<Ship>();
    }

    public void WinCondition()
    {
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        playerUi.transform.GetChild(1).gameObject.SetActive(false);
        playerUi.transform.GetChild(3).gameObject.SetActive(true);
    }

    public void LoseCondition(int typeOfLoss)
    {
        Time.timeScale = 0;

        playerUi.transform.GetChild(1).gameObject.SetActive(false);
        playerUi.transform.GetChild(2).gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // 0 = player died, 1 = ship died
        playerUi.transform.GetChild(2).transform.GetChild(typeOfLoss).gameObject.SetActive(true);

        playerUi.transform.GetChild(2).transform.GetChild(2).GetComponent<TMP_Text>().text = "Wave: " 
                                                                          + EnemySpawnController.Instance.waveNumber 
                                                                          + " / "
                                                                          + EnemySpawnController.Instance.maxWaves;
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

    public void MainMenu()
    {
        Time.timeScale = 1;
        //SceneManager.LoadScene(0);
        Debug.Log("go to main menu boop boop");
    }
}
