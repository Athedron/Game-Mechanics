using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    void DoDamage();
    void DoKnockBack();
}

public interface IDamagable 
{
    void TakeDamage(int damage);
    void Die();
}

public interface ISelfDestructable 
{
    void SpawnItem();
}