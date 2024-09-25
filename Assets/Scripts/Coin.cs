using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private int coinPower = 1;

    [SerializeField]
    private int pickCoinSEVol = 1;
    [SerializeField]
    private AudioClip pickCoinSE = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().BuffUp(coinPower);
            AudioControl.Instance.SetSEVol(pickCoinSEVol);
            AudioControl.Instance.PlaySE(pickCoinSE);
            Destroy(gameObject);
        }
    }
}
