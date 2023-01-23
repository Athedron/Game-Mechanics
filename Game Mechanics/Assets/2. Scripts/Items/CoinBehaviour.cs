using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinBehaviour : Item
{
    public int coinAmount;
    UnityEvent m_CoinScaler;
    private bool isScaled;
    private GameObject smallCoin;
    private GameObject bigCoin;

    private void Start()
    {
        if (m_CoinScaler == null)
            m_CoinScaler = new UnityEvent();

        m_CoinScaler.AddListener(CoinScale);

        smallCoin = transform.GetChild(0).gameObject;
        bigCoin = transform.GetChild(1).gameObject;
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterController>().UpdateCoinAmount(coinAmount);

            Destroy(gameObject);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (coinAmount > 1 && !isScaled)
            m_CoinScaler.Invoke();
    }

    public void CoinScale()
    {
        smallCoin.gameObject.SetActive(false);
        bigCoin.gameObject.SetActive(true);
        isScaled = true;
    }
}
