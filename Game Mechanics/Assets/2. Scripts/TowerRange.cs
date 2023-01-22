using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRange : MonoBehaviour
{
    public Tower tower;

    public void OnEnable()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void Start()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyParent")
        {
            tower.enemiesInRange.Add(other.gameObject);
            tower.tTarget = Tower.TowerTargets.ENEMY;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "EnemyParent")
        {
            tower.canAttack = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "EnemyParent")
        {
            tower.LostEnemy(other.gameObject);
        }
    }
}
