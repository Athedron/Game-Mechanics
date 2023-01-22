using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float rotationSpeed;

    public virtual void FixedUpdate()
    {
        RotateAnim();
    }

    public virtual void RotateAnim()
    {
        transform.Rotate(0, rotationSpeed, 0, Space.World);
    }
}
