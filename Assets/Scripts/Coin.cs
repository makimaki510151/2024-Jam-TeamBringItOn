using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private int coinPower = 1;

    [SerializeField, Tooltip("取得エフェクト")]
    private GameObject AcquisitionEffectPrefab = null;

    [Header("音設定")]

    [SerializeField]
    private int pickCoinSEVol = 1;
    [SerializeField]
    private AudioClip pickCoinSE = null;

    private Transform effectTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            if (player == null)
            {
                player = collision.gameObject.GetComponentInParent<Player>();
            }
            if (player.isSpecialBan) return;
            player.BuffUp(coinPower);
            effectTransform = Instantiate(AcquisitionEffectPrefab).transform;
            effectTransform.position = transform.position;
            AudioControl.Instance.SetSEVol(pickCoinSEVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
            AudioControl.Instance.PlaySE(pickCoinSE);
            Destroy(gameObject);
        }
    }
}
