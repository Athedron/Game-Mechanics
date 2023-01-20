using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, ISelfDestructable
{
    private Rigidbody missileRb;
    [HideInInspector] public float missileSpeed;
    public float missileLifeTime;

    private GameObject explosionPrefab;

    private void Start()
    {
        explosionPrefab = Resources.Load<GameObject>("Combat/Explosion");
        missileRb = GetComponent<Rigidbody>();
        Invoke(nameof(SelfDestruct), missileLifeTime);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player")
        {
            SelfDestruct();
        }
    }

    void Update()
    {
        MissileMovement();
    }

    private void MissileMovement()
    {
        missileRb.AddForce(transform.forward * missileSpeed, ForceMode.Force);
    }    

    public void SelfDestruct()
    {
        Explode();
        Destroy(gameObject);
    }

    public void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
