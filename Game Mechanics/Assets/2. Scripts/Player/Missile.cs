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

    void Update()
    {
        MissileMovement();
    }

    private void MissileMovement()
    {
        missileRb.AddForce(transform.forward * missileSpeed, ForceMode.Force);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            SelfDestruct();
        }
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
