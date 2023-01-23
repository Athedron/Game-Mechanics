using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRange : MonoBehaviour
{
    Enemy enemyScript;
    bool inRangeOffShip;

    public GameObject currentTarget;
    private GameObject oldTarget;

    private void Start()
    {
        enemyScript = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "EndPoint" ||
            other.gameObject.tag == "Tower")
        {
            if (other.gameObject == enemyScript.ship)
            {
                enemyScript.ChangeTarget(enemyScript.ship);
            }
            else if (gameObject.GetComponentInParent<RangedEnemy>() && 
                     other.gameObject.tag == "Tower" && ChangeTargetTower())
            {
                enemyScript.towers.Add(other.gameObject);
                enemyScript.ChangeTarget(PickClosestTower());
            }
            else if (other.gameObject == enemyScript.player && currentTarget == other.gameObject && ChangeTargetPlayer())
            {
                enemyScript.ChangeTarget(enemyScript.player);
            }
        }
    }

    private bool ChangeTargetTower()
    {
        if ((int)Random.Range(0f, 9f) == 1)
            return true;
        else
            return false;
    }
    
    private bool ChangeTargetPlayer()
    {
        if ((int)Random.Range(0f, 2f) == 1)
            return true;
        else
            return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.tag == "Player"/* && other.gameObject != enemyScript.attackTarget*/) ||
            other.gameObject.tag == "EndPoint" ||
            other.gameObject.tag == "Tower")
        {
            currentTarget = other.gameObject;

            if (currentTarget == enemyScript.ship)
            {
                enemyScript.ChangeTarget(enemyScript.ship);
            }
            else  if (gameObject.GetComponentInParent<RangedEnemy>() &&
                      currentTarget != enemyScript.tower)
            {
                enemyScript.ChangeTarget(PickClosestTower());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemyScript.towers.Count != 0)
        {
            enemyScript.ChangeTarget(enemyScript.ship);
            currentTarget = null;
        }


        if (other.gameObject == enemyScript.player)
        {
            enemyScript.ChangeTarget(enemyScript.ship);
            currentTarget = null;
        }
        
    }

    private void Update()
    {
        if (currentTarget == null)
            enemyScript.agent.speed = enemyScript.calcSpeed;

        if (currentTarget == enemyScript.ship)
            return;

        if (enemyScript.towers.Count != 0 && currentTarget != null && currentTarget.TryGetComponent<Tower>(out Tower t))
        {
            if (!t.broken)
            {
                enemyScript.ChangeTarget(ATower(currentTarget));
            }

            return;
        }

        if ((Vector3.Distance(transform.position, enemyScript.endPoint.position) >
            Vector3.Distance(enemyScript.player.transform.position, enemyScript.endPoint.position)) &&
            currentTarget == enemyScript.player)
        {
            enemyScript.ChangeTarget(enemyScript.player);
        }

        if ((Vector3.Distance(enemyScript.transform.position, enemyScript.endPoint.position) <
            Vector3.Distance(enemyScript.player.transform.position, enemyScript.endPoint.position)) &&
            currentTarget != enemyScript.ship)
        {
            enemyScript.ChangeTarget(enemyScript.ship);
        }        
    }

    private GameObject ATower(GameObject curTarget)
    {
        if (enemyScript.towers.Contains(curTarget))
        {
            return curTarget;
        }
        else
        {
            return null;
        }
    }

    public GameObject PickClosestTower()
    {
        GameObject closestTower = null;
        float closestDistance = 0f;
        bool first = true;

        foreach (GameObject tower in enemyScript.towers)
        {
            float distance = Vector3.Distance(tower.transform.position, transform.position);

            if (first)
            {
                closestDistance = distance;
                first = false;
            }
            else if (distance < closestDistance)
            {
                closestTower = tower;
                closestDistance = distance;
            }
        }
        return closestTower;
    }
}
