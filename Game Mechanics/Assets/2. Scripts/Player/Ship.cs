using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour, IDamagable
{
    public int health;
    public int maxHealth;

    public int healAmount;

    public Image healthBar;

    private void Start()
    {
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        UpdateHealthBar();

        if (health < 0)
            Die();
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)health / maxHealth;

        if (healthBar.fillAmount < 0.25f)
        {
            healthBar.color = Color.red;
        }
        else if (healthBar.fillAmount < 0.5f)
        {
            healthBar.color = Color.yellow;
        }
        else
        {
            healthBar.color = Color.green;
        }
    }

    private void FixedUpdate()
    {
        PassiveHeal();
    }

    public void PassiveHeal()
    {
        health += healAmount;

        if (health > maxHealth)
            health = maxHealth;

        UpdateHealthBar();
    }

    public void Die()
    {
        GameStateManager.Instance.LoseCondition(1);
    }
}
