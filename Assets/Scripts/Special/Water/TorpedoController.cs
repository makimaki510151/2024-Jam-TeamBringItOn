using UnityEngine;

public class TorpedoController : MonoBehaviour
{
    [Header("魚雷設定")]

    [SerializeField, Tooltip("エフェクト")]
    private GameObject torpedEffect = null;

    [SerializeField, Tooltip("減速移動の時間（秒）")]
    private float slowPhaseDuration = 1.5f;

    [SerializeField, Tooltip("加速にかかる時間（秒）")]
    private float accelerationDuration = 1.5f;

    [SerializeField, Tooltip("減速時の最低速度")]
    private float minSpeed = 1.0f;

    [SerializeField, Tooltip("最大移動速度")]
    private float maxSpeed = 5.0f;

    private float elapsedTime = 0f;

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
        elapsedTime = 0f;
    }

    void Update()
    {
        if (isShot)
        {
            elapsedTime += Time.deltaTime;

            if (!mySpriteRenderer.isVisible)
            {
                isShot = false;
                Destroy(gameObject);
            }

            float currentSpeed;

            // 減速フェーズ
            if (elapsedTime < slowPhaseDuration)
            {
                float t = elapsedTime / slowPhaseDuration;
                currentSpeed = Mathf.Lerp(maxSpeed, minSpeed, t);
            }
            // 加速フェーズ
            else
            {
                float t = (elapsedTime - slowPhaseDuration) / accelerationDuration;
                currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, t);
            }

            // 移動
            myRigidbody2D.velocity = myTransform.right * currentSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().DirectShot(player.character);
            Instantiate(torpedEffect).transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
