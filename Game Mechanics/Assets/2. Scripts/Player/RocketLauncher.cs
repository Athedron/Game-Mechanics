using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public int damage;

    private Transform combatLookAt;
    private RaycastHit rocketLauncherLookRayHit;

    private bool canFire = true;

    private GameObject missilePrefab;
    private Transform firePoint;
    private Transform lookAtPoint;

    [Header("Rocket Launcher")]
    public float rocketLauncherCoolDown;
    public float missileSpeed;
    private bool playerFired = true;

    private void Start()
    {
        combatLookAt = transform.parent.parent.GetChild(0).GetChild(0);
        missilePrefab = Resources.Load<GameObject>("Combat/Missile");
        firePoint = transform.GetChild(1);
        lookAtPoint = Camera.main.transform.GetChild(0);
    }

    void FixedUpdate()
    {
        RocketLauncherLookAt();
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetButton("Fire1") && canFire)
        {
            canFire = false;
            Fire();
            Invoke(nameof(ResetFire), rocketLauncherCoolDown);
        }
    }

    private void ResetFire()
    {
        canFire = true;
    }

    private void RocketLauncherLookAt()
    {
        transform.LookAt(lookAtPoint.position);
        /*if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
            //transform.LookAt(hit.point);
            
        else
            Debug.Log("oops");
            //transform.LookAt(lookAtPoint.position);*/
    }

    private void Fire()
    {
        RaycastHit hit;
        GameObject missile = Instantiate(missilePrefab, firePoint.position, transform.rotation);
        var missleComp = missile.GetComponent<Missile>();

        missleComp.missileSpeed = missileSpeed;
        missleComp.damage = damage;
        missleComp.playerFired = playerFired;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity) && 
            hit.collider.gameObject.activeSelf)
        {
            missile.GetComponent<Missile>().target = hit.point;
        }
        else /*if (hit.collider.gameObject.activeSelf)*/
        {
            missile.GetComponent<Missile>().target = Camera.main.transform.position + Camera.main.transform.forward * 5000f;
        }
    }
}
