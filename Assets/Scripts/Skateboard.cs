using UnityEngine;

public class Skateboard : MonoBehaviour
{
    [SerializeField]
    private int pickSkateboardSEVol = 1;
    [SerializeField]
    private AudioClip pickSkateboardSE = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            if(player == null)
            {
                player = collision.gameObject.GetComponentInParent<Player>();
            }
            if (player.isSpecialBan) return;
            player.SkateboardTime();
            AudioControl.Instance.SetSEVol(pickSkateboardSEVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
            AudioControl.Instance.PlaySE(pickSkateboardSE);
            Destroy(gameObject);
        }
    }
}
