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

        agent.speed = Random.Range(minMeleeSpeed, maxMeleeSpeed);
    }
    
    public override void Update()
    {
        base.Update();
    }
}
