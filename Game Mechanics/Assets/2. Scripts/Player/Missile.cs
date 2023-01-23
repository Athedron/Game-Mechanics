using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, ISelfDestructable
{
    private Rigidbody missileRb;
    [HideInInspector] public float missileSpeed;
    public float missileLifeTime;
    [HideInInspector]public Vector3 target;
    [HideInInspector]public int damage;

    public  bool playerFired;

    private GameObject explosionPrefab;

    private void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Combat/Explosion");
        missileRb = GetComponent<Rigidbody>();
        Invoke(nameof(SpawnItem), missileLifeTime);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnemyParent" || 
            other.gameObject.tag == "Environment" || 
            other.gameObject.tag == "EndPoint")
        {
            //SpawnExplosion();
        }
    }

    void FixedUpdate()
    {
        MissileMovement();
    }

    private void MissileMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, missileSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.2f)
        {
            SpawnItem();
        }
    }    

    public void SpawnItem()
    {
        Explode();
        Destroy(gameObject);
    }

    public void Explode()
    {
        if (explosionPrefab != null)
        {
           GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            var explosionComp = explosion.GetComponent<MissileExplosion>();

            explosionComp.missileDamage = damage;
            explosionComp.playerFired = playerFired;
        }
    }
}
