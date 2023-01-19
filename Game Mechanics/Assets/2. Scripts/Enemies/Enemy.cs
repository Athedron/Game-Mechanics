using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable
{
    public int health;
    public int enemyDamage;

    public Transform endPoint;
    public NavMeshAgent agent;

    public virtual void Start()
    {
        endPoint = GameObject.FindGameObjectWithTag("EndPoint").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    public virtual void Update()
    {
        transform.LookAt(endPoint);
        agent.SetDestination(endPoint.position);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        EnemySpawnController.Instance.amountOfEnemiesAlive--;

        if (EnemySpawnController.Instance.amountOfEnemiesAlive == 0)
            EnemySpawnController.Instance.StartCoroutine(EnemySpawnController.Instance.StartNewWave());

        Destroy(gameObject);
    }

    
}
