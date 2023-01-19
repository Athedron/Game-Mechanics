using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("Speed Stats")]
    public float minRangedSpeed;
    public float maxRangedSpeed;

    public override void Start()
    {
        base.Start();

        agent.speed = Random.Range(minRangedSpeed, maxRangedSpeed);
    }

    public override void Update()
    {
        base.Update();
    }
}
