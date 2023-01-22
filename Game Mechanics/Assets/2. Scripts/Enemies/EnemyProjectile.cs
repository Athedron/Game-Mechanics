using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody missileRb;
    [HideInInspector] public float enemyProjectileSpeed;
    public float enemyProjectileLifeTime;
    [HideInInspector]public int enemyProjectileDamage;

    //private GameObject explosionPrefab;

    private void Start()
    {
        //explosionPrefab = Resources.Load<GameObject>("Combat/Explosion");
        missileRb = GetComponent<Rigidbody>();
        Invoke(nameof(SelfDestruct), enemyProjectileLifeTime);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "EndPoint" ||
            other.gameObject.tag == "Tower")
        {
            if (other.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
                damagable.TakeDamage(enemyProjectileDamage);

            SelfDestruct();
        }
    }

    void Update()
    {
        EnemyProjectileMovement();
    }

    private void EnemyProjectileMovement()
    {
        missileRb.AddForce(transform.forward * enemyProjectileSpeed, ForceMode.Force);
    }

    public void SelfDestruct()
    {
        //Explode();
        //enemyProjectileDamage = 0;        
        Destroy(gameObject);
    }

   /* public void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }*/
}
