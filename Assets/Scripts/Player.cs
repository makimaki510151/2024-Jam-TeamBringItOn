using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField, Tooltip("デフォルトの移動速度")]
    private float defaultSpeed = 10.0f;
    [SerializeField, Tooltip("ノックバック力(左)")]
    private float knockbackLeft = 10.0f;
    [SerializeField, Tooltip("ノックバック力(上)")]
    private float knockbackUp = 5.0f;
    [SerializeField, Tooltip("無敵時間")]
    private float invincibleTime = 0.5f;
    private float invincibleTimer = 0;
    [SerializeField, Tooltip("最大移動速度")]
    private float maxSpeed = 20f;

    // 取得しているバフアイテムの数
    [SerializeField]
    private float speedBuffItemCount = 0;
    [SerializeField, Tooltip("通常のジャンプ力")]
    private float normalJumpPower = 8.0f;
    [SerializeField, Tooltip("パリィのジャンプ力")]
    private float parryJumpPower = 5.0f;

    // プレイヤーの位置
    public enum PlayCharacter
    {
        Water,
        Fire
    }
    [SerializeField]
    private PlayCharacter character = PlayCharacter.Water;

    [SerializeField, Tooltip("パリィ時間")]
    private float parryTime = 0.25f;
    private float parryTimer = 0;

    [SerializeField, Tooltip("無敵中の色")]
    private Color transparentColor = new(1, 1, 1, 0.25f);

    [SerializeField]
    private bool isJump = false;
    private bool isParry = false;
    private bool isParryHit = false;        // パリィが当たったらフラグをオン
    private bool isParryCancel = false;     // パリィできないならフラグをオン
    private bool isShot = false;
    [SerializeField]
    private bool isGround = false;
    private bool isTransparent = false;     // 点滅用

    private Vector2 tempVector2 = new(0, 0);
    private Vector2 vector2zero = Vector2.zero;
    private Color colorWhite = Color.white;
    private Color colorRed = Color.red;

    private Rigidbody2D myRigidbody2D = null;
    private Transform myTransform = null;
    private SpriteRenderer mySpriteRenderer;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // 接地しているならフラグをオン
            if (isGround)
            {
                isJump = true;
            }
            else
            {
                // 接地しておらず、パリィが可能でパリィ中でなければ、パリィを行う
                if (!isParry && !isParryCancel)
                {
                    Debug.Log("準備");
                    isParry = true;
                    parryTimer = parryTime;
                    mySpriteRenderer.color = colorRed;
                }
            }
        }
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isShot = true;
        }
    }

    void Start()
    {
        // コンポーネント取得
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myTransform = transform;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 移動処理
        tempVector2 = (myTransform.right * defaultSpeed + (myTransform.right * defaultSpeed * (speedBuffItemCount / 100))) * Time.deltaTime;
        myRigidbody2D.velocity += tempVector2;

        // ジャンプフラグがオンなら、ジャンプする
        if (isJump)
        {
            myRigidbody2D.AddForce(myTransform.up * normalJumpPower, ForceMode2D.Impulse);
            isJump = false;
        }

        tempVector2 = myRigidbody2D.velocity;

        // 移動速度が最大値を超えないようにする
        if (tempVector2.x > maxSpeed)
        {
            tempVector2.x = maxSpeed;
            myRigidbody2D.velocity = tempVector2;
        }

        // パリィ状態なら、時間を計測する
        if (isParry)
        {
            parryTimer -= Time.deltaTime;

            // 指定した時間が経ったら、パリィ状態を解除する
            if (parryTimer < 0)
            {
                isParry = false;
                mySpriteRenderer.color = colorWhite;

                // 接地していなくパリィが当たっていないなら、パリィできないようにする
                if (!isGround && !isParryHit)
                {
                    isParryCancel = true;
                }
            }
        }

        // 無敵中なら時間を計測する
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;

            // 指定した時間が経ったら、カラーを元に戻す
            if (invincibleTimer <= 0)
            {
                mySpriteRenderer.color = colorWhite;
            }

            // 点滅処理
            isTransparent = !isTransparent;
            if (isTransparent)
            {
                mySpriteRenderer.color = transparentColor;
            }
            else
            {
                mySpriteRenderer.color = colorWhite;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 地面にヒット
        if (collision.CompareTag("Ground"))
        {
            Debug.Log("地面");
            isGround = true;
            isParryCancel = false;
            isParryHit = false;
        }
        // 敵にヒット
        else if (collision.CompareTag("Enemy"))
        {
            Debug.Log("敵");

            // パリィ中なら、パリィ処理を行う
            if (isParry)
            {
                Debug.Log("パリィ！");
                isParryHit = true;
                collision.GetComponent<Enemy>().StockMove(character);
                tempVector2 = myRigidbody2D.velocity;
                tempVector2.y = 0;
                myRigidbody2D.velocity = tempVector2;
                myRigidbody2D.AddForce(myTransform.up * parryJumpPower, ForceMode2D.Impulse);
            }
            // 無敵時間でないなら、ノックバック処理を行う
            else if (invincibleTimer <= 0)
            {
                myRigidbody2D.velocity = vector2zero;
                myRigidbody2D.AddForce(-myTransform.right * knockbackLeft, ForceMode2D.Impulse);
                myRigidbody2D.AddForce(myTransform.up * knockbackUp, ForceMode2D.Impulse);
                invincibleTimer = invincibleTime;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
}
