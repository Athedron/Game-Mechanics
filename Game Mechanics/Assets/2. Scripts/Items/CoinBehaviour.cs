using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinBehaviour : Item
{
    public int coinAmount;
    UnityEvent m_CoinScaler;
    private bool isScaled;

    private void Start()
    {
        if (m_CoinScaler == null)
            m_CoinScaler = new UnityEvent();

        m_CoinScaler.AddListener(CoinScale);
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterController>().UpdateCoinAmount(coinAmount);

            Destroy(gameObject);
        }
    }

    public override void Update()
    {
        base.Update();

        if (coinAmount > 1 && !isScaled)
            m_CoinScaler.Invoke();
    }

    public void CoinScale()
    {
        transform.localScale *= coinAmount * 0.3f;
        isScaled = true;
    }
}
