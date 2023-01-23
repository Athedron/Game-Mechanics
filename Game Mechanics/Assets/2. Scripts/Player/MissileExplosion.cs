using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : MonoBehaviour, ISelfDestructable
{
    public float explosionLifeTime;
    public int missileDamage;
    public float explosionForce;
    public float playerExplosionForce;

    private float radius;

    void Start()
    {
        Invoke(nameof(SpawnExplosion), explosionLifeTime);
        radius = GetComponent<SphereCollider>().radius * 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Take damage
        if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            if (other.TryGetComponent<CharacterController>(out CharacterController player))
            {
                missileDamage = (int)(missileDamage * 0.2f);
            }

            damagable.TakeDamage(missileDamage);
        }

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

    public void SpawnExplosion()
    {
        Destroy(gameObject);
    }
}
