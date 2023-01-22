using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyRange : MonoBehaviour
{
    private TowerEco towerEco;

    private void Start()
    {
        towerEco = GetComponentInParent<TowerEco>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            towerEco.EnableUi();
            towerEco.inRange = true;
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            towerEco.DisableUi();
            towerEco.inRange = false;
        }
    }
}
