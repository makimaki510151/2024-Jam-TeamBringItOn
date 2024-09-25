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
            collision.GetComponent<Player>().SkateboardTime();
            AudioControl.Instance.SetSEVol(pickSkateboardSEVol);
            AudioControl.Instance.PlaySE(pickSkateboardSE);
            Destroy(gameObject);
        }
    }
}
