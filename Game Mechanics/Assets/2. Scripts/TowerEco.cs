using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerEco : MonoBehaviour
{
    public int towerCost;
    [HideInInspector] public int towerCostStart;
    public int towerCostMultiplier;
    public int towerUpgradeDamageMultiplier;
    public int towerLevel;

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
    }

    private void FixedUpdate()
    {
        //BuyTower();
    }

    public void EnableUi()
    {
        if (!towerBought)
        {
            buyUi.SetActive(true);
            buyUi.transform.GetChild(1).GetComponent<TMP_Text>().text = "Tower Cost: " + towerCost;
        }
        else
        {
            upgradeUi.SetActive(true);
            upgradeUi.transform.GetChild(0).GetComponent<TMP_Text>().text = "Upgrade Cost: " + towerCost;
        }

        lookAt = StartCoroutine(LookAt());
        buyTower = StartCoroutine(BuyTower());
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
                buyUi.transform.LookAt(playerObj.transform.position);
            else
                upgradeUi.transform.LookAt(playerObj.transform.position);
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
                    EnableUi();
                }
                else
                {
                    UpgradeTower();
                }

                yield return new WaitForSeconds(2);
            }
        }
    }

    public void UpgradeTower()
    {
        player.UpdateCoinAmount(-towerCost);
        towerCost *= towerCostMultiplier;
        tower.towerDamage *= towerUpgradeDamageMultiplier;
        towerLevel++;
        upgradeUi.transform.GetChild(0).GetComponent<TMP_Text>().text = "Upgrade Cost: " + towerCost;
    }
}
