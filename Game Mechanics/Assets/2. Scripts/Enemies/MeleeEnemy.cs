using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Speed Stats")]
    public float minMeleeSpeed;
    public float maxMeleeSpeed;

    public override void Start()
    {
        base.Start();

        attackCooldown = 2;

        agent.speed = Random.Range(minMeleeSpeed, maxMeleeSpeed);
    }
    
    public override void Update()
    {
        base.Update();
    }

    /*public void AttackTarget(GameObject attackingTarget)
    {
        if (canAttack)
        {
            canAttack = false;
            StartCoroutine(Attack(enemyDamage, attackingTarget));
        }
    }

    IEnumerator Attack(int enemyDamage, GameObject target)
    {
        // attack
        Debug.Log("zug zug melee attack: " + target.name + " for: " + enemyDamage + " damage");
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }*/
}
