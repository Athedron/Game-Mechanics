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

    public int coinAmount;

    public override void Start()
    {
        base.Start();

        calcSpeed = Random.Range(minRangedSpeed, maxRangedSpeed);
        agent.speed = calcSpeed;

        rangedEnemyProjectile = Resources.Load("Enemies/EnemyProjectile") as GameObject;
        offset = new Vector3(0f, 2.2f, 0f);

        if (coinAmount == 0)
            CoinAmountCalc();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override IEnumerator Attack(int enemyDamage, GameObject target)
    {
        isCoroutingRunning = true;

        base.Attack(enemyDamage, target);

        GameObject projectile = Instantiate(rangedEnemyProjectile, transform.position + offset, transform.rotation);

        projectile.GetComponent<EnemyProjectile>().enemyProjectileDamage = enemyDamage;
        projectile.GetComponent<EnemyProjectile>().enemyProjectileSpeed = enemyProjectileSpeed;

        agent.speed = 0;

        yield return new WaitForSeconds(attackCooldown);

        agent.speed = calcSpeed;

        isCoroutingRunning = false;
    }

    public override void SpawnItem()
    {
        var coinTmp = Instantiate(coin, transform.position, Quaternion.identity);
        coinTmp.GetComponent<CoinBehaviour>().coinAmount = coinAmount;
    }

    public void CoinAmountCalc()
    {
        if ((int)Random.Range(0f, 20f) == 1)
            coinAmount = 5;
        else
            coinAmount = 1;
    }
}
