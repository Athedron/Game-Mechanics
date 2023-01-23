using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Speed Stats")]
    public float minMeleeSpeed;
    public float maxMeleeSpeed;


    [HideInInspector] public Transform hammer;
    private Vector3 hammerStartPos;

    public bool dropsHealth;

    public override void Start()
    {
        base.Start();
        calcSpeed = Random.Range(minMeleeSpeed, maxMeleeSpeed);
        agent.speed = calcSpeed;
        hammer = transform.GetChild(2);
        hammerStartPos = hammer.localPosition;

        HealthPackDropCalc();
    }
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (canAttack)
            HammerHitAnim();
        else
            ResetHammerPos();
    }

    public override IEnumerator Attack(int enemyDamage, GameObject target)
    {
        isCoroutingRunning = true;

        base.Attack(enemyDamage, target);

        if (target.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
            damagable.TakeDamage(enemyDamage);

        agent.speed = 0;

        yield return new WaitForSeconds(attackCooldown);

        agent.speed = calcSpeed;

        isCoroutingRunning = false;
    }

    public void HammerHitAnim()
    {
        hammer.transform.localPosition += transform.forward * Mathf.Sin(Time.time * 10f) * 0.2f;
    }

    public void ResetHammerPos()
    {
        hammer.localPosition = hammerStartPos;
    }

    public override void SpawnItem()
    {
        if (dropsHealth)
            Instantiate(healthPack, transform.position, Quaternion.identity);
    }

    public void HealthPackDropCalc()
    {
        if ((int)Random.Range(0f, 3f) == 1)
            dropsHealth = true;
    }
}
