using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable, ISelfDestructable
{
    public int health;
    public int enemyDamage;
    public float attackCooldown;

    public Transform endPoint;
    public NavMeshAgent agent;

    public bool canAttack = false;
    public bool inAggroRange = false;
    public GameObject attackRange;
    public GameObject aggroRange;
    [HideInInspector] public float calcSpeed;

    public enum Targets
    { 
        SHIP,
        PLAYER,
        TOWER
    }

    public Targets target;
    
    public GameObject attackTarget;
    public GameObject player;
    public GameObject ship;
    
    // TBC
    public GameObject tower;

    [HideInInspector]public Coroutine attackCoroutine;
    [HideInInspector] public bool isCoroutingRunning;

    [Header("Item Drops")]
    public GameObject coin;
    public GameObject healthPack;

    public virtual void Start()
    {
        endPoint = GameObject.FindGameObjectWithTag("EndPoint").transform;
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<CharacterController>().gameObject;
        ship = FindObjectOfType<Ship>().gameObject;

        aggroRange = transform.GetChild(0).gameObject;
        attackRange = transform.GetChild(1).gameObject;

        coin = Resources.Load("Items/Coin") as GameObject;
        healthPack = Resources.Load("Items/HealthPack") as GameObject;

        target = Targets.SHIP;
        attackTarget = ship;
    }

    public virtual void FixedUpdate()
    {
        UpdateTarget();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        EnemySpawnController.Instance.m_EnemyDied.Invoke();

        // TODO spawn health pack and maybe coins
        SpawnItem();
        Destroy(gameObject);
    }

    public void MoveToTarget(GameObject lookAtObj, Transform destination)
    {
        transform.LookAt(lookAtObj.transform);
        agent.SetDestination(destination.position);
    }

    public void ChangeTarget(GameObject collisionTarget)
    {
        if (collisionTarget == tower)
        {
            target = Targets.TOWER;
        }
        else if (collisionTarget == ship)
        {
            target = Targets.SHIP;
        }
        else if (collisionTarget == player)
        {
            if (collisionTarget.transform.position.y - transform.position.y >= 9f &&
                gameObject != EnemySpawnController.Instance.rangedEnemy)
            {
                target = Targets.SHIP;
                return;
            }
            else
            {
                target = Targets.PLAYER;
            }
        }
    }


    public void UpdateTarget()
    {
        switch (target)
        {
            case Targets.SHIP:
                MoveToTarget(ship, endPoint);
                attackTarget = ship;

                if (canAttack)
                {
                    AttackTarget(ship);
                }

                break;
            case Targets.PLAYER:
                MoveToTarget(player, player.transform);
                attackTarget = player;
               
                if (canAttack)
                {
                    AttackTarget(player);
                }
                

                break;
            case Targets.TOWER:
                // add tower attack and lookat for range enemies

                break;
            default:
                MoveToTarget(ship, endPoint);
                Debug.Log("ik kom in default");

                break;
        }
    }

    public void AttackTarget(GameObject attackingTarget)
    {
        if (!isCoroutingRunning)
            attackCoroutine = StartCoroutine(Attack(enemyDamage, attackingTarget));
    }

    public virtual IEnumerator Attack(int enemyDamage, GameObject target)
    {
        yield return null;   
    }

    public virtual void SpawnItem()
    {

    }
}
