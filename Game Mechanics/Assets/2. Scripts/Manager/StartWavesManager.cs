using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StartWavesManager : MonoBehaviour
{
    private void OnDestroy()
    {
        if (Application.isEditor && !EditorApplication.isPlaying)
        {
            EnemySpawnController.Instance.StartCoroutine(EnemySpawnController.Instance.StartNewWave());
        }
    }
}
