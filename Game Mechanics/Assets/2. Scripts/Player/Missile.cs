using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, ISelfDestructable
{
    private Rigidbody missileRb;
    [HideInInspector] public float missileSpeed;
    public float missileLifeTime;
    [HideInInspector]public Vector3 target;

    private GameObject explosionPrefab;

    private void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Combat/Explosion");
        missileRb = GetComponent<Rigidbody>();
        Invoke(nameof(SpawnItem), missileLifeTime);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player" || other.gameObject.tag == "EnemyParent" || other.gameObject.tag == "Environment")
        {
            SpawnItem();
        }
    }

    void FixedUpdate()
    {
        MissileMovement();
    }

    private void MissileMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, missileSpeed * Time.deltaTime);
        //missileRb.AddForce(transform.forward * missileSpeed, ForceMode.Force);
    }    

    public void SpawnItem()
    {
        Explode();
        Destroy(gameObject);
    }

    public void Explode()
    {
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
