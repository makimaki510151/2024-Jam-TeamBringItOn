using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("移動設定")]

    [SerializeField, Tooltip("デフォルトの移動速度")]
    private float defaultSpeed = 10.0f;
    [SerializeField, Tooltip("ノックバック力(左)")]
    private float knockbackLeft = 10.0f;
    [SerializeField, Tooltip("ノックバック力(上)")]
    private float knockbackUp = 5.0f;
    [SerializeField, Tooltip("最大移動速度")]
    private float maxSpeed = 20f;
    [SerializeField, Tooltip("最大ジャンプ速度")]
    private float maxJumpSpeed = 20f;

    // 取得しているバフアイテムの数
    [SerializeField]
    private float speedBuffItemCount = 0;
    [SerializeField, Tooltip("通常のジャンプ力")]
    private float normalJumpPower = 8.0f;

    private float speed = 0;
    private float jumpPower = 0;

    // プレイヤーの位置
    public enum PlayCharacter
    {
        Water,
        Fire
    }
    [SerializeField]
    private PlayCharacter character = PlayCharacter.Water;

    public PlayCharacter Character { get => character; private set => character = value; }

    [Header("パリィ設定")]

    [SerializeField, Tooltip("順位が上の時のパリィ時間")]
    private float parryTimeForHigherRank = 0.25f;

    [SerializeField, Tooltip("順位が下の時のパリィ時間")]
    private float parryTimeForLowerRank = 0.75f;

    private float parryTime = 0;
    private float parryTimer = 0;

    [SerializeField, Tooltip("パリィのジャンプ力")]
    private float parryJumpPower = 5.0f;

    [SerializeField, Tooltip("パリィエフェクト")]
    private GameObject parryEffectPrafab = null;
    private Transform parryEffectTransform = null;

    [Header("色設定")]

    [SerializeField, Tooltip("基本色")]
    private Color basicColor = Color.white;

    [SerializeField, Tooltip("ベタ塗する色")]
    private Color solidColor = Color.black;

    [SerializeField, Tooltip("ベタ塗する時間")]
    private float solidColorTime = 3.0f;
    private float solidColorTimer = 0;

    [Header("凍結状態設定")]

    [SerializeField, Tooltip("凍結状態時の移動速度")]
    private float maxSpeedForFrozen = 15.0f;

    [SerializeField, Tooltip("凍結状態時の移動速度")]
    private float jumpPowerForFrozen = 4.0f;

    [SerializeField, Tooltip("凍結時間短縮量")]
    private float shorteningFrozenTime = 0.1f;

    [SerializeField, Tooltip("氷の画像")]
    private Transform iceTransform = null;

    [SerializeField, Tooltip("氷の最大の大きさ")]
    private Vector2 maxSizeIce = Vector2.zero;

    private float frozenTime = 0;
    private float frozenTimer = 0;
    private bool isFrozen = false;

    [Header("その他設定")]

    [SerializeField, Tooltip("ダメージエフェクト")]
    private GameObject damageEffectPrafab = null;
    private Transform damageEffectTransform = null;

    [SerializeField, Tooltip("無敵時間")]
    private float invincibleTime = 0.5f;
    private float invincibleTimer = 0;

    [SerializeField, Tooltip("無敵中の色")]
    private Color transparentColor = new(1, 1, 1, 0.25f);

    private Color transparentSolidColor = default(Color);

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

    [SerializeField, Tooltip("地面のレイヤー")]
    private LayerMask groundLayerMask = default;
    [SerializeField, Tooltip("設置判定のレイの長さ")]
    private float maxDistance = 0.625f;

    [SerializeField, Tooltip("敵を消滅させるヒット数")]
    private int hitenemyCountDestroy = 3;

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

    [SerializeField]
    private float seDamageVol = 1.0f;
    [SerializeField]
    private AudioClip seDamageClip = null;

    static readonly int isParryId = Animator.StringToHash("isParry");
    static readonly int isDamageId = Animator.StringToHash("isDamage");
    static readonly int isSkateId = Animator.StringToHash("isSkate");

    private bool isJump = false;
    private bool isParry = false;
    private bool isParryHit = false;        // パリィが当たったらフラグをオン
    private bool isParryCancel = false;     // パリィできないならフラグをオン
    private bool isGround = false;
    private bool isTransparent = false;     // 点滅用
    private bool isSolidColor = false;
    private bool isGoal = false;

    private GameObject enemyObject = null;
    private int hitEnemyCount = 0;

    private Vector2 tempVector2 = new(0, 0);
    private Vector2 vector2zero = Vector2.zero;
    private Color colorWhite = Color.white;
    private float deltaTime;
    private Enemy.EnemyAiType aiType;
    private GimmickGround.GimmickGroundType groundType;
    private RaycastHit2D hit;

    private Rigidbody2D myRigidbody2D = null;
    private Transform myTransform = null;
    private SpriteRenderer mySpriteRenderer;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && Time.timeScale == 1)
        {
            if (!isGround)
            {
                hit = Physics2D.Raycast(myTransform.position, -myTransform.up, maxDistance, groundLayerMask);
                Debug.DrawRay(myTransform.position, -myTransform.up, Color.red, maxDistance);
                //なにかと衝突した時だけそのオブジェクトの名前をログに出す
                if (hit.collider)
                {
                    IsGroundTrue();
                }
            }
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
                    isParry = true;
                    parryTimer = parryTime;
                    myAnimator.SetTrigger(isParryId);
                }
            }

            if (isFrozen)
            {
                frozenTime -= shorteningFrozenTime;
            }
        }
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        if (context.started && Time.timeScale == 1)
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

        speed = maxSpeed;
        jumpPower = normalJumpPower;
        isGroundTime = isGroundTimer;
        parryTime = parryTimeForHigherRank;
        transparentSolidColor = transparentColor * solidColor;
        iceTransform.localScale = vector2zero;
    }

    void Update()
    {
        deltaTime = Time.deltaTime;

        // 移動処理
        tempVector2 = (myTransform.right * defaultSpeed + (myTransform.right * defaultSpeed * (speedBuffItemCount / 100))) * skateboardBuffContainer * Time.deltaTime;
        if(!isGoal)
        {
            myRigidbody2D.velocity += tempVector2;

            // ジャンプフラグがオンなら、ジャンプする
            if (isJump)
            {
                AudioControl.Instance.SetSEVol(seJumpVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
                AudioControl.Instance.PlaySE(seJumpClip, myTransform);
                myRigidbody2D.AddForce(myTransform.up * jumpPower, ForceMode2D.Impulse);
                isJump = false;
            }
        }
        else
        {
            if(myRigidbody2D.velocity.x < 0)
            {
                myRigidbody2D.velocity = vector2zero;
                this.enabled = false;
            }
            else
            {
                myRigidbody2D.velocity -= tempVector2 / 2;
            }
        }

        tempVector2 = myRigidbody2D.velocity;

        // 移動速度が最大値を超えないようにする
        if (tempVector2.x > speed * skateboardBuffContainer)
        {
            tempVector2.x = speed;
            myRigidbody2D.velocity = tempVector2;
        }
        if (tempVector2.y > maxJumpSpeed)
        {
            tempVector2.y = maxJumpSpeed;
            myRigidbody2D.velocity = tempVector2;
        }

        //// 接地していても上昇落下をしていなければ、接地しているか判定する
        //if (!isGround && tempVector2.y < 0.1f && tempVector2.y > -0.1f)
        //{
        //    // 指定した時間が経ったら、接地しているとする
        //    isGroundTime -= deltaTime;
        //    if(isGroundTime <= 0)
        //    {
        //        isGround = true;
        //    }
        //}
        //else
        //{
        //    isGroundTime = isGroundTimer;
        //}

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
                if(!isSolidColor) mySpriteRenderer.color = transparentColor;
                else mySpriteRenderer.color = transparentSolidColor;
            }
            else
            {
                if(!isSolidColor) mySpriteRenderer.color = colorWhite;
                else mySpriteRenderer.color = solidColor;
            }

            // 指定した時間が経ったら、カラーを元に戻す
            if (invincibleTimer <= 0)
            {
                if(!isSolidColor) mySpriteRenderer.color = colorWhite;
                else mySpriteRenderer.color = solidColor;
            }
        }

        // スケボー処理
        if (skateboardTimer > 0)
        {
            skateboardTimer -= deltaTime;
            if (skateboardTimer <= 0)
            {
                skateboardTimer = 0;
                skateboardBuffContainer = 1;
                myAnimator.SetBool(isSkateId, false);
            }
        }

        // ベタ塗処理
        if (isSolidColor)
        {
            solidColorTimer += deltaTime;
            if(solidColorTimer >= solidColorTime)
            {
                solidColorTimer = 0;
                isSolidColor = false;
                mySpriteRenderer.color = colorWhite;
            }
        }

        // 氷結処理
        if (isFrozen)
        {
            frozenTimer += deltaTime;
            iceTransform.localScale = Vector2.Lerp(maxSizeIce, vector2zero, frozenTimer / frozenTime);
            if (frozenTimer >= frozenTime)
            {
                isFrozen = false;
                frozenTimer = 0;
                speed = maxSpeed;
                jumpPower = normalJumpPower;
                iceTransform.localScale = vector2zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 地面にヒット
        if (collision.CompareTag("Ground"))
        {
            IsGroundTrue();
        }
        else if (collision.CompareTag("GimmickGround"))
        {
            groundType = collision.GetComponent<GimmickGround>().GroundType;
            if(groundType == GimmickGround.GimmickGroundType.Ice)
            {
                frozenTime = collision.GetComponent<GimmickGround>().SetFrozenTime();
                isFrozen = true;
                frozenTimer = 0;
                speed = maxSpeedForFrozen;
                jumpPower = jumpPowerForFrozen;
                KnockBackPlayer(collision);
            }
            else if(groundType == GimmickGround.GimmickGroundType.Magma)
            {
                KnockBackPlayer(collision);
            }
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
                        AudioControl.Instance.SetSEVol(seWaterParryVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
                        AudioControl.Instance.PlaySE(seWaterParryClip, myTransform);
                        break;
                    case PlayCharacter.Fire:
                        AudioControl.Instance.SetSEVol(seFireParryVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
                        AudioControl.Instance.PlaySE(seFireParryClip, myTransform);
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
                parryEffectTransform = Instantiate(parryEffectPrafab).transform;
                parryEffectTransform.position = Vector2.Lerp(myTransform.position, collision.transform.position, 0.5f);
            }
            // 無敵時間でないなら、ノックバック処理を行う
            else if (invincibleTimer <= 0)
            {
                KnockBackPlayer(collision);
                aiType = collision.GetComponent<Enemy>().AiType;
                // タコに当たったら、タコスミを発射させる
                if (aiType == Enemy.EnemyAiType.Octopus)
                {
                    collision.GetComponent<Enemy>().HitOctopus(character);
                    isSolidColor = true;
                    solidColorTimer = 0;
                }
                // 弾に当たったら、その弾を消す
                else if (aiType == Enemy.EnemyAiType.Bullet)
                {
                    Destroy(collision.gameObject);
                }
                // カンシャクダマに当たったら、花火を打ち上げさせる
                else if(aiType == Enemy.EnemyAiType.MrFireWorks)
                {
                    collision.GetComponent <Enemy>().HitMrFireWorks(character);
                    isSolidColor = true;
                    solidColorTimer = 0;
                }

                // 連続ヒット処理
                // ヒットした敵が前回の敵と違うなら、その敵を記憶しヒット数をリセットする
                if(enemyObject != collision.gameObject)
                {
                    enemyObject = collision.gameObject;
                    hitEnemyCount = 1;
                }
                // ヒットした敵が前回の敵と同じなら、ヒット数をカウントする
                else
                {
                    hitEnemyCount++;
                    // ヒット数が指定した数以上なら、その敵を消滅させる
                    if(hitEnemyCount >= hitenemyCountDestroy)
                    {
                        Destroy(enemyObject);
                        enemyObject = null;
                        hitEnemyCount = 0;
                    }
                }
            }
        }
        // ゴールにヒット
        else if (collision.CompareTag("Goal"))
        {
            if (!isGoal)
            {
                isGoal = true;
                MainGameRoot.Instance.GoalPlayer(character);
            }
        }
    }
    private void IsGroundTrue()
    {
        isGround = true;
        isParryCancel = false;
        isParryHit = false;
        myAnimator.SetBool(isDamageId, false);
    }

    private void KnockBackPlayer(Collider2D collider)
    {
        myRigidbody2D.velocity = vector2zero;
        myRigidbody2D.AddForce(-myTransform.right * knockbackLeft, ForceMode2D.Impulse);
        myRigidbody2D.AddForce(myTransform.up * knockbackUp, ForceMode2D.Impulse);
        invincibleTimer = invincibleTime;
        myAnimator.SetBool(isDamageId, true);

        // エフェクト処理
        damageEffectTransform = Instantiate(damageEffectPrafab).transform;
        damageEffectTransform.position = Vector2.Lerp(myTransform.position, collider.transform.position, 0.5f);

        AudioControl.Instance.SetSEVol(seDamageVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seDamageClip, myTransform);
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
        myAnimator.SetBool(isSkateId, true);
    }

    public void SetParryTime(bool isHigher)
    {
        if(isHigher)
        {
            parryTime = parryTimeForHigherRank;
        }
        else
        {
            parryTime = parryTimeForLowerRank;
        }
    }
}
