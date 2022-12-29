using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    private Transform combatLookAt;
    private RaycastHit rocketLauncherLookRayHit;

    private bool canFire = true;

    private GameObject missilePrefab;
    private Transform firePoint;
    private Transform lookAtPoint;

    [Header("Rocket Launcher")]
    public float rocketLauncherCoolDown;
    public float missileSpeed;

    private void Start()
    {
        combatLookAt = transform.parent.parent.GetChild(0).GetChild(0);
        missilePrefab = Resources.Load<GameObject>("Combat/Missile");
        firePoint = transform.GetChild(1);
        lookAtPoint = Camera.main.transform.GetChild(0);
    }

    void Update()
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
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out rocketLauncherLookRayHit))
        {
            transform.LookAt(rocketLauncherLookRayHit.point);
        }
        else
            transform.LookAt(lookAtPoint);
    }

    private void Fire()
    {
        GameObject missile = Instantiate(missilePrefab, firePoint.position, transform.rotation);
        missile.GetComponent<Missile>().missileSpeed = missileSpeed;
    }
}
