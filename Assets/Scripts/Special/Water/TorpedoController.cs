using UnityEngine;

public class TorpedoController : MonoBehaviour
{
    [Header("魚雷設定")]

    [SerializeField, Tooltip("移動速度")]
    private float torpedSpeed = 1.0f;

    [SerializeField, Tooltip("エフェクト")]
    private GameObject torpedEffect = null;

    Player player;
    Rigidbody2D myRigidbody2D;
    Transform myTransform;
    SpriteRenderer mySpriteRenderer;
    bool isShot = false;

    public void Init(Player player)
    {
        this.player = player;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myTransform = transform;
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Shot()
    {
        isShot = true;
    }

    void Update()
    {
        if(isShot)
        {
            if(!mySpriteRenderer.isVisible)
            {
                isShot = false;
                Destroy(gameObject);
            }

            // 移動
            myRigidbody2D.velocity = myTransform.right * torpedSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 敵にヒット
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().DirectShot(player.character);
            Instantiate(torpedEffect).transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
