using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    private GameObject meleeEnemy;
    private GameObject rangedEnemy;

    private List<GameObject> spawnList = new List<GameObject>();    


    // Start is called before the first frame update
    void Start()
    {
        meleeEnemy = Resources.Load("Enemies/MeleeEnemy") as GameObject;
        rangedEnemy = Resources.Load("Enemies/RangedEnemy") as GameObject;

        foreach (Transform spawnPoint in transform.GetChild(0))
        {
            spawnList.Add(spawnPoint.gameObject);
        }
    }

    public void SpawnWave()
    {

    }
}
