using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Speed Stats")]
    public float minRangedSpeed;
    public float maxRangedSpeed;
    private GameObject rangedEnemyProjectile;
    private Vector3 offset;

    public float enemyProjectileSpeed;

    public override void Start()
    {
        base.Start();

        attackCooldown = 3;

        agent.speed = Random.Range(minRangedSpeed, maxRangedSpeed);

        rangedEnemyProjectile = Resources.Load("Enemies/EnemyProjectile") as GameObject;
        offset = new Vector3(0f, 2.2f, 0f);
    }

    public override void Update()
    {
        base.Update();
    }

    public override IEnumerator Attack(int enemyDamage, GameObject target)
    {
        isCoroutingRunning = true;

        base.Attack(enemyDamage, target);

        GameObject projectile = Instantiate(rangedEnemyProjectile, transform.position + offset, transform.rotation);

        projectile.GetComponent<EnemyProjectile>().enemyProjectileDamage = enemyDamage;
        projectile.GetComponent<EnemyProjectile>().enemyProjectileSpeed = enemyProjectileSpeed;

        yield return new WaitForSeconds(attackCooldown);

        Debug.Log("coroutine is niet gestopt");


        isCoroutingRunning = false;
    }
}
