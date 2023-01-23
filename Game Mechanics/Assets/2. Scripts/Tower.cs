using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour, IDamagable
{
    public int health;
    public int maxHealth;
    public Image healthBar;
    public int towerDamage;
    [HideInInspector]public int startTowerDamage;
    public float towerProjectileSpeed;
    public int attackCooldown;

    private GameObject towerProjectile;

    private Transform barrelGfx;
    [HideInInspector]public GameObject headGfx;
    private Transform shootPoint;

    private Quaternion startRot;

    public List<GameObject> enemiesInRange = new List<GameObject>();
    public bool canAttack;
    public GameObject currentTarget;

    public bool broken;
    public GameObject smoke;


    [HideInInspector] public Coroutine attackCoroutine;
    [HideInInspector] public bool isCoroutingRunning;

    public enum TowerTargets
    {
        ENEMY,
        BOSS,
        NOTARGET
    }

    public TowerTargets tTarget;

    // Start is called before the first frame update
    void Start()
    {
        barrelGfx = transform.GetChild(0).transform;
        shootPoint = barrelGfx.GetChild(0).GetChild(0).transform;
        headGfx = transform.GetChild(1).GetChild(0).gameObject;

        towerProjectile = Resources.Load("Tower/TowerMissile") as GameObject;

        startRot = barrelGfx.rotation;
        startTowerDamage = towerDamage;

        UpdateHealthBar();
    }
    private void OnEnable()
    {
        smoke = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;

        broken = false;
        smoke.SetActive(false);        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!broken)
            UpdateTarget();
    }
    

    public GameObject GetTarget()
    {
        GameObject closestEnemy = null;
        float closestDistance = 0f;
        bool first = true;

        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy != null) 
            {
                float distance = Vector3.Distance(enemy.transform.position, transform.position);

                if (first)
                {
                    closestEnemy = enemy;
                    closestDistance = distance;
                    first = false;
                }
                else if (distance < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distance;
                }
            }
        }

        return closestEnemy;
    }

    public void Aiming(GameObject target)
    {
        if (target == null)
            return;

        barrelGfx.LookAt(target.transform);
    }

    public void UpdateTarget()
    {
        switch (tTarget)
        {
            case TowerTargets.ENEMY:
                currentTarget = GetTarget();
                Aiming(currentTarget);
                
                if (canAttack)
                {
                    AttackTarget(currentTarget);
                }

                break;
            case TowerTargets.BOSS:
                currentTarget = GetTarget();
                Aiming(currentTarget);

                if (canAttack)
                {
                    AttackTarget(currentTarget);
                }
                break;

            case TowerTargets.NOTARGET:
                canAttack = false;

                break;
            default:
                Debug.Log("ik kom in default");
                break;
        }
    }

    public void AttackTarget(GameObject attackingTarget)
    {
        if (!isCoroutingRunning)
            attackCoroutine = StartCoroutine(Attack(towerDamage, attackingTarget));
    }

    public IEnumerator Attack(int enemyDamage, GameObject target)
    {
        isCoroutingRunning = true;

        GameObject projectile = Instantiate(towerProjectile, shootPoint.position, shootPoint.rotation);
        projectile.GetComponent<Missile>().missileSpeed = towerProjectileSpeed;
        projectile.GetComponent<Missile>().damage = towerDamage;
        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, Mathf.Infinity))
            projectile.GetComponent<Missile>().target = hit.point;

        yield return new WaitForSeconds(attackCooldown);

        isCoroutingRunning = false;
    }

    public void ResetPos()
    {
        foreach (var enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                var enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.towers.Remove(gameObject);
                enemyScript.ChangeTarget(enemyScript.ship);

                enemyScript.canAttack = false;
            }
        }

        barrelGfx.rotation = startRot;
        canAttack = false;
    }

    public void LostEnemy(GameObject lostEnemy)
    {
        canAttack = false;
        //isCoroutingRunning = false;

        if (attackCoroutine != null)
            //StopCoroutine(attackCoroutine);

        enemiesInRange.Remove(lostEnemy);

        if (enemiesInRange.Count == 0)
            tTarget = Tower.TowerTargets.NOTARGET;

        ResetPos();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        UpdateHealthBar();

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        foreach (var enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                var enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.towers.Remove(gameObject);
                enemyScript.ChangeTarget(enemyScript.ship);

                enemyScript.canAttack = false;
            }
        }

        health = maxHealth;
        towerDamage = startTowerDamage;

        Broken();
    }

    public void Broken()
    {
        broken = true;
        canAttack = false;
        smoke.SetActive(true);
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
}
