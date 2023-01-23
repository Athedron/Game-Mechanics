using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    public static SpawnPortal Instance;
 
    public enum PortalStates 
    { 
        PASSIVE,
        ACTIVE,
        AGRESSIVE
    }

    public PortalStates portalStates;
    public int activeTimeDuration;
    private Material passiveMat;
    private Material activeMat;
    private Material agressiveMat;

    private MeshRenderer portalRenderer;

    public GameObject skipIntermissionText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }


    private void Start()
    {
        passiveMat = Resources.Load("SpawnPortal/SpawnPortalPassive") as Material;
        activeMat = Resources.Load("SpawnPortal/SpawnPortalActive") as Material;
        agressiveMat = Resources.Load("SpawnPortal/SpawnPortalAgressive") as Material;

        portalRenderer = GetComponent<MeshRenderer>();
        
        portalStates = PortalStates.PASSIVE;
        ChangePortalState(portalStates);
    }

    public void ChangePortalState(PortalStates portalStates)
    {
        switch (portalStates)
        {
            case PortalStates.PASSIVE:
                // green
                portalRenderer.material = passiveMat;

                if (EnemySpawnController.Instance.waveNumber == 4)
                    skipIntermissionText.SetActive(true);                    

                break;
            case PortalStates.ACTIVE:
                // yellow
                GameStateManager.Instance.timer = activeTimeDuration;
                portalRenderer.material = activeMat;
                skipIntermissionText.SetActive(false);

                break;
            case PortalStates.AGRESSIVE:
                // red
                portalRenderer.material = agressiveMat;

                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Missile>())
        {
            Destroy(collision.gameObject);

            if (portalStates == PortalStates.PASSIVE && GameStateManager.Instance.levelStarted)
                ChangePortalState(PortalStates.ACTIVE);
        }
    }
}
