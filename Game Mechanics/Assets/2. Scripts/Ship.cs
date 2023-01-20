using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IDamagable
{
    public int health;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
            GameStateManager.Instance.LoseCondition(1);
    }

    public void Die()
    {

    }
}
