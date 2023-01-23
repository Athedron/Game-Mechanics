using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float rotationSpeed;
    public bool moveToPlayer = false;

    public virtual void FixedUpdate()
    {
        RotateAnim();
        PickUpCoins();
    }

    public virtual void RotateAnim()
    {
        transform.Rotate(0, rotationSpeed, 0, Space.World);
    }

    public void PickUpCoins()
    {
        if (moveToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameStateManager.Instance.player.transform.position, 25 * Time.deltaTime);
        }
    }
}
