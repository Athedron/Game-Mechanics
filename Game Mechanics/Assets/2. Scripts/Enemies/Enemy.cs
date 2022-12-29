using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public int health;
    public int enemyDamage;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("oof");

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
