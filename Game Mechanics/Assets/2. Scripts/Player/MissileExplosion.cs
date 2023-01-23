using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileExplosion : MonoBehaviour, ISelfDestructable
{
    public float explosionLifeTime;
    public int missileDamage;
    public float explosionForce;
    public float playerExplosionForce;
    public bool playerFired;

    private float radius;

    private GameObject crosshairHit;

    void Start()
    {
        Invoke(nameof(SpawnItem), explosionLifeTime);
        radius = GetComponent<SphereCollider>().radius * 2;

        crosshairHit = GameStateManager.Instance.playerUi.transform.GetChild(8).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Take damage
        if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            if (other.TryGetComponent<CharacterController>(out CharacterController player))
            {
                missileDamage = (int)(missileDamage * 0.1f);
            }

            damagable.TakeDamage(missileDamage);

            if (playerFired)
            {
                crosshairHit = GameStateManager.Instance.playerUi.transform.GetChild(8).gameObject;
                StartCoroutine(CrosshairHit());
            }
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

    IEnumerator CrosshairHit()
    {
        crosshairHit.SetActive(true);
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        crosshairHit.SetActive(false);
    }

    public void SpawnItem()
    {
        Destroy(gameObject);
    }
}
