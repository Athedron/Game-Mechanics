using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRange : MonoBehaviour
{
    Enemy enemyScript;
    bool inRangeOffShip;

    private void Start()
    {
        enemyScript = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Environment" && other.tag != "Enemy")
        {
            if (gameObject.TryGetComponent<RangedEnemy>(out RangedEnemy rangedEnemy) && 
                other.gameObject == enemyScript.tower)
            {
                enemyScript.ChangeTarget(other.gameObject);
                return;
            }
            else if (other.gameObject == enemyScript.ship)
            {
                enemyScript.ChangeTarget(other.gameObject);
                return;
            }

            enemyScript.ChangeTarget(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Environment" && other.tag != "Enemy")
        {
            if (gameObject.TryGetComponent<RangedEnemy>(out RangedEnemy rangedEnemy) &&
                other.gameObject == enemyScript.tower)
            {
                enemyScript.ChangeTarget(enemyScript.tower);
            }
            else if (other.gameObject == enemyScript.ship)
            {
                enemyScript.ChangeTarget(enemyScript.ship);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == enemyScript.player)
        {
            enemyScript.ChangeTarget(enemyScript.ship);
        }
    }
}
