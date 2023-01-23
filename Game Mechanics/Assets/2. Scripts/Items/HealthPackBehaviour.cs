using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackBehaviour : Item
{
    public int healthPackAmount;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<CharacterController>(); 
            
            player.health = player.maxHealth;

            player.UpdateHealthBar();

            Destroy(gameObject);
        }
    }
}
