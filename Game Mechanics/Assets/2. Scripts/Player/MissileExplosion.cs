using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : MonoBehaviour, ISelfDestructable
{
    public float explosionLifeTime = 0.25f;
    public int missileDamage = 25;
    public float explosionForce = 50f;
    public float playerExplosionForce = 1250f;

    private float radius;

    void Start()
    {
        Invoke(nameof(SelfDestruct), explosionLifeTime);
        radius = GetComponent<SphereCollider>().radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Take damage
        if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
            damagable.TakeDamage(missileDamage);

        // Knockback
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            if (other.tag == "Player")
            {
                rb.AddExplosionForce(playerExplosionForce, transform.position, radius, 3f);
            }
            else
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius, 3f);
            }                
        }
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
