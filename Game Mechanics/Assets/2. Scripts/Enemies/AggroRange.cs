using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRange : MonoBehaviour
{
    Enemy enemyScript;
    bool inRangeOffShip;

    private GameObject currentTarget;
    private GameObject newTarget;

    private void Start()
    {
        enemyScript = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        newTarget = other.gameObject;

        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "EndPoint" ||
            other.gameObject.tag == "Tower")
        {
            if (other.gameObject == enemyScript.ship)
            {
                //currentTarget = enemyScript.ship;
                enemyScript.ChangeTarget(enemyScript.ship);
            }
            else if (gameObject.TryGetComponent<RangedEnemy>(out RangedEnemy rangedEnemy) &&
                     other.gameObject == enemyScript.tower)
            {
                //currentTarget = enemyScript.tower;
                enemyScript.ChangeTarget(enemyScript.tower);
            }
            else if (other.gameObject == enemyScript.player)
            {
                //currentTarget = enemyScript.player;
                enemyScript.ChangeTarget(enemyScript.player);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "EndPoint" ||
            other.gameObject.tag == "Tower")
        {
            currentTarget = other.gameObject;

            if (other.gameObject == enemyScript.ship)
            {
                enemyScript.ChangeTarget(enemyScript.ship);
            }
            else  if (gameObject.TryGetComponent<RangedEnemy>(out RangedEnemy rangedEnemy) &&
                      other.gameObject == enemyScript.tower)
            {
                enemyScript.ChangeTarget(enemyScript.tower);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
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

        if ((Vector3.Distance(transform.position, enemyScript.endPoint.position) >
            Vector3.Distance(enemyScript.player.transform.position, enemyScript.endPoint.position)) &&
            currentTarget == enemyScript.player)
        {
            enemyScript.ChangeTarget(enemyScript.player);
        }

        if ((Vector3.Distance(transform.position, enemyScript.endPoint.position) <
            Vector3.Distance(enemyScript.player.transform.position, enemyScript.endPoint.position)) &&
            currentTarget != enemyScript.ship)
        {
            enemyScript.ChangeTarget(enemyScript.ship);
        }        
    }
}
