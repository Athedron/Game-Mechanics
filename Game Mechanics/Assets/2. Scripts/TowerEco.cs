using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerEco : MonoBehaviour
{
    public int towerCost;
    public int towerFixCost;
    [HideInInspector] public int towerCostStart;
    public int towerCostMultiplier;
    public int towerUpgradeDamageMultiplier;
    public int towerLevel;

    private Material level1Mat; 
    private Material level2Mat;
    private Material level3Mat;

    [HideInInspector] public bool inRange;

    [HideInInspector] public Tower tower;
    [HideInInspector] public GameObject buyUi;
    [HideInInspector] public GameObject upgradeUi;
    [HideInInspector] public GameObject playerObj;
    private CharacterController player;

    private Coroutine lookAt;
    private Coroutine buyTower;

    [HideInInspector] public bool towerBought;

    private void Start()
    {
        tower = transform.parent.GetChild(0).GetComponentInChildren<Tower>();
        buyUi = transform.GetChild(1).gameObject;
        upgradeUi = transform.GetChild(2).gameObject;

        playerObj = FindObjectOfType<CharacterController>().gameObject;
        player = playerObj.GetComponent<CharacterController>();

        towerCostStart = towerCost;

        level1Mat = Resources.Load("Tower/Level1") as Material;
        level2Mat = Resources.Load("Tower/Level2") as Material;
        level3Mat = Resources.Load("Tower/Level3") as Material;
    }

    private void OnEnable()
    {
        tower = transform.parent.GetChild(0).GetComponentInChildren<Tower>();
        buyUi = transform.GetChild(1).gameObject;
        upgradeUi = transform.GetChild(2).gameObject;

        playerObj = FindObjectOfType<CharacterController>().gameObject;
        player = playerObj.GetComponent<CharacterController>();

        towerCostStart = towerCost;

        level1Mat = Resources.Load("Tower/Level1") as Material;
        level2Mat = Resources.Load("Tower/Level2") as Material;
        level3Mat = Resources.Load("Tower/Level3") as Material;
    }

    private void FixedUpdate()
    {
        //BuyTower();
    }

    public void EnableUi()
    {
        UpdateTextUi();
        lookAt = StartCoroutine(LookAt());
        buyTower = StartCoroutine(BuyTower());
        
    }

    public void UpdateTextUi()
    {
        if (!towerBought)
        {
            buyUi.SetActive(true);
            buyUi.transform.GetChild(1).GetComponent<TMP_Text>().text = "Tower Cost: " + towerCost;
            buyUi.transform.GetChild(2).GetComponent<TMP_Text>().color = Color.green;
        }
        else
        {
            upgradeUi.SetActive(true);
            if (!tower.broken)
            {
                upgradeUi.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Upgrade Cost: " + towerCost;
            }
            else
            {
                upgradeUi.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Repair Cost: " + towerFixCost;
            }
        }
    }

    public IEnumerator LookAt()
    {
        yield return new WaitForEndOfFrame();

        if (!inRange && !GameStateManager.Instance.levelStarted)
            buyUi.transform.GetChild(1).GetComponent<TMP_Text>().text = "Tower Cost: " + towerCost;

        while (buyUi.activeSelf || upgradeUi.activeSelf)
        {
            yield return null;
            if (!towerBought)
                buyUi.transform.LookAt(new Vector3(playerObj.transform.position.x, buyUi.transform.position.y, playerObj.transform.position.z));
            else
                upgradeUi.transform.LookAt(new Vector3(playerObj.transform.position.x, upgradeUi.transform.position.y, playerObj.transform.position.z));
        }
    }

    public void DisableUi()
    {
        if (!towerBought)
        {
            buyUi.SetActive(false);
            StopAllCoroutines();
        }
        else
        {
            upgradeUi.SetActive(false);
            StopAllCoroutines();
        }
    }

    public IEnumerator BuyTower()
    {
        while (player.coins >= towerCost)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!GameStateManager.Instance.levelStarted)
                {
                    foreach (TowerEco towerEco in GameStateManager.Instance.towerEcoObjs)
                    {
                        if (!towerEco.inRange)
                        {
                            towerEco.DisableUi();
                        }
                    }
                }

                if (!towerBought)
                {
                    if (!GameStateManager.Instance.levelStarted)
                        GameStateManager.Instance.StartLevel();

                    player.UpdateCoinAmount(-towerCost);
                    tower.gameObject.SetActive(true);
                    DisableUi();
                    towerCost *= towerCostMultiplier;
                    towerBought = true;
                    UpdateTextUi();
                }
                else if (tower.broken)
                {
                    FixTower();
                    tower.UpdateHealthBar();
                    UpdateTextUi();
                }
                else
                {
                    UpgradeTower();
                }
                yield return new WaitForSeconds(2);
            }
        }
    }

    public void FixTower()
    {
        player.UpdateCoinAmount(-towerFixCost);
        tower.broken = false;
        tower.smoke.SetActive(false);
        tower.enemiesInRange.Clear();
    }

    public void UpgradeTower()
    {
        player.UpdateCoinAmount(-towerCost);
        towerCost *= towerCostMultiplier;
        towerFixCost *= towerCostMultiplier;
        tower.towerDamage *= towerUpgradeDamageMultiplier;
        tower.health *= towerUpgradeDamageMultiplier;
        tower.maxHealth *= towerUpgradeDamageMultiplier;
        towerLevel++;
        ChangeTowerLevelMat(towerLevel);
        upgradeUi.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Upgrade Cost: " + towerCost;
    }

    public void ChangeTowerLevelMat(int towerLevel)
    {
        switch (towerLevel)
        {
            case 1:
                tower.headGfx.GetComponent<MeshRenderer>().material = level1Mat;
                return;
            case 2:
                tower.headGfx.GetComponent<MeshRenderer>().material = level2Mat;
                return;
            case 3:
                tower.headGfx.GetComponent<MeshRenderer>().material = level3Mat;
                return;
        }
        
    }
}
