using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : MonoBehaviour, ISelfDestructable
{
    public float explosionLifeTime = 0.25f;
    public int missileDamage = 25;
    public float explosionForce = 50f;
    public float playerExplosionMultiplier = 5f;

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
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            // TODO
            if (hit.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                if (other.tag == "Player")
                {
                    explosionForce *= playerExplosionMultiplier;
                }
                // add explosion knockback
                rb.AddExplosionForce(explosionForce, transform.position, radius, 3f);

                Debug.Log(explosionForce);
            }
        }
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
