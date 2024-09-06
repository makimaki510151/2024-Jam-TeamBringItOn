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

    [SerializeField, Tooltip("接地しているとみなす時間")]
    private float isGroundTimer = 0.5f;
    private float isGroundTime = 0;

    [SerializeField, Tooltip("アニメーター")]
    private Animator myAnimator = null;

    [SerializeField]
    private float skateboardBuffPower = 1.3f;
    private float skateboardBuffContainer = 1;
    [SerializeField]
    private float skateboardTime = 1.0f;
    private float skateboardTimer = 0;

    [Header("音関係")]
    [SerializeField]
    private float seWaterParryVol = 1.0f;
    [SerializeField]
    private AudioClip seWaterParryClip = null;

    [SerializeField]
    private float seFireParryVol = 1.0f;
    [SerializeField]
    private AudioClip seFireParryClip = null;

    [SerializeField]
    private float seJumpVol = 1.0f;
    [SerializeField]
    private AudioClip seJumpClip = null;


    static readonly int isParryId = Animator.StringToHash("isParry");
    static readonly int isDamageId = Animator.StringToHash("isDamage");

    private bool isJump = false;
    private bool isParry = false;
    private bool isParryHit = false;        // パリィが当たったらフラグをオン
    private bool isParryCancel = false;     // パリィできないならフラグをオン
    private bool isGround = false;
    private bool isTransparent = false;     // 点滅用

    private Vector2 tempVector2 = new(0, 0);
    private Vector2 vector2zero = Vector2.zero;
    private Color colorWhite = Color.white;
    private float deltaTime;
    private Enemy.EnemyAiType aiType;

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
                    myAnimator.SetTrigger(isParryId);
                }
            }
        }
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            MainGameRoot.Instance.StockEnemyShot(character);
        }
    }

    void Start()
    {
        // コンポーネント取得
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myTransform = transform;
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        isGroundTime = isGroundTimer;
    }

    void Update()
    {
        deltaTime = Time.deltaTime;

        // 移動処理
        tempVector2 = (myTransform.right * defaultSpeed + (myTransform.right * defaultSpeed * (speedBuffItemCount / 100)))* skateboardBuffContainer * Time.deltaTime;
        myRigidbody2D.velocity += tempVector2;

        // ジャンプフラグがオンなら、ジャンプする
        if (isJump)
        {
            AudioControl.Instance.SetSEVol(seJumpVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
            AudioControl.Instance.PlaySE(seJumpClip, myTransform);
            myRigidbody2D.AddForce(myTransform.up * normalJumpPower, ForceMode2D.Impulse);
            isJump = false;
        }

        tempVector2 = myRigidbody2D.velocity;

        // 移動速度が最大値を超えないようにする
        if (tempVector2.x > maxSpeed* skateboardBuffContainer)
        {
            tempVector2.x = maxSpeed;
            myRigidbody2D.velocity = tempVector2;
        }

        // 接地していても上昇落下をしていなければ、接地しているか判定する
        if(!isGround && tempVector2.y < 0.1f && tempVector2.y > -0.1f)
        {
            // 指定した時間が経ったら、接地しているとする
            isGroundTime -= deltaTime;
            if(isGroundTime <= 0)
            {
                isGround = true;
            }
        }
        else
        {
            isGroundTime = isGroundTimer;
        }

        // パリィ状態なら、時間を計測する
        if (isParry)
        {
            parryTimer -= deltaTime;

            // 指定した時間が経ったら、パリィ状態を解除する
            if (parryTimer < 0)
            {
                isParry = false;

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
            invincibleTimer -= deltaTime;

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

            // 指定した時間が経ったら、カラーを元に戻す
            if (invincibleTimer <= 0)
            {
                mySpriteRenderer.color = colorWhite;
            }
        }

        if(skateboardTimer > 0)
        {
            skateboardTimer -= deltaTime;
            if(skateboardTimer <= 0)
            {
                skateboardTimer = 0;
                skateboardBuffContainer = 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 地面にヒット
        if (collision.CompareTag("Ground"))
        {
            isGround = true;
            isParryCancel = false;
            isParryHit = false;
            myAnimator.SetBool(isDamageId, false);
        }
        // 敵にヒット
        else if (collision.CompareTag("Enemy"))
        {
            // パリィ中なら、パリィ処理を行う
            if (isParry)
            {
                switch (character)
                {
                    case PlayCharacter.Water:
                        AudioControl.Instance.SetSEVol(seWaterParryVol*MainGameRoot.Instance.dataScriptableObject.seVolSetting);
                        AudioControl.Instance.PlaySE(seWaterParryClip,myTransform);
                        break;
                    case PlayCharacter.Fire:
                        AudioControl.Instance.SetSEVol(seFireParryVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
                        AudioControl.Instance.PlaySE(seFireParryClip,myTransform);
                        break;
                }

                isParryHit = true;
                collision.GetComponent<Enemy>().StockMove(character);
                if (skateboardTimer <= 0)
                {
                    tempVector2 = myRigidbody2D.velocity;
                    tempVector2.y = 0;
                    myRigidbody2D.velocity = tempVector2;
                    myRigidbody2D.AddForce(myTransform.up * parryJumpPower, ForceMode2D.Impulse);
                }

                // エフェクト処理
                
            }
            // 無敵時間でないなら、ノックバック処理を行う
            else if (invincibleTimer <= 0)
            {
                myRigidbody2D.velocity = vector2zero;
                myRigidbody2D.AddForce(-myTransform.right * knockbackLeft, ForceMode2D.Impulse);
                myRigidbody2D.AddForce(myTransform.up * knockbackUp, ForceMode2D.Impulse);
                invincibleTimer = invincibleTime;
                myAnimator.SetBool(isDamageId, true);

                aiType = collision.GetComponent<Enemy>().AiType;
                // タコに当たったら、タコスミを発射させる
                if (aiType == Enemy.EnemyAiType.Octopus)
                {
                    collision.GetComponent<Enemy>().HitOctopus(character);
                }
                // 弾に当たったら、その弾を消す
                else if(aiType == Enemy.EnemyAiType.Bullet)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
        // ゴールにヒット
        else if (collision.CompareTag("Goal"))
        {
            MainGameRoot.Instance.GoalPlayer(character);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGround = false;
        }
    }

    public void BuffUp(int buffPower)
    {
        speedBuffItemCount += buffPower;
    }
    public void SkateboardTime()
    {
        skateboardTimer = skateboardTime;
        skateboardBuffContainer = skateboardBuffPower;
        isParry = true;
        parryTimer = skateboardTime;
    }
    public void Reverse()
    {

    }
}
