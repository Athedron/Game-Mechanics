using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    Enemy enemyScript;

    private void Start()
    {
        enemyScript = GetComponentInParent<Enemy>();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "EndPoint" ||
            other.gameObject.tag == "Tower")
        {
            if (other.gameObject == enemyScript.attackTarget)
            {
                enemyScript.canAttack = true;
            }
            else if (other.gameObject != enemyScript.attackTarget && other.gameObject != null)
            {
                enemyScript.canAttack = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "EndPoint" ||
            other.gameObject.tag == "Tower")
        {
            enemyScript.canAttack = false;

            if (enemyScript.attackCoroutine != null)
                enemyScript.StopCoroutine(enemyScript.attackCoroutine);

            enemyScript.isCoroutingRunning = false;
        }
    }

    public void Debuging(Collider other)
    {

        //Debug.Log(other.gameObject == enemyScript.attackTarget);


        if (other.gameObject.tag != "Enemy" &&
            other.gameObject.tag != "Environment")
        {
            Debug.Log("other: " + other.gameObject + "  target: " +
                enemyScript.attackTarget + "  can attack: " +
                enemyScript.canAttack + "  is c running: " +
                enemyScript.isCoroutingRunning);
        }   
    }
}
