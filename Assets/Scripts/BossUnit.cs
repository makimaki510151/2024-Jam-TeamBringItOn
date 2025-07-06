using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUnit : MonoBehaviour
{
    [SerializeField, Tooltip("ヒットポイント")]
    private int maxHitPoint = 12;

    [SerializeField, Tooltip("ずらす値")]
    private Vector2 maxShiftPosition = Vector2.zero;

    [SerializeField, Tooltip("ずらす値")]
    private Vector2 minShiftPosition = Vector2.zero;

    [SerializeField, Tooltip("ストック位置")]
    private RectTransform bossStockPosRect = null;

    [SerializeField, Tooltip("加速度")]
    private float acceleration = 0;

    [SerializeField, Tooltip("突進準備時間")]
    private float chargeReadyTime = 3;

    [SerializeField]
    private float seKilledVol = 1.0f;
    [SerializeField]
    private AudioClip seKilledClip = null;

    private enum BossState
    {
        Spawning,
        ChargeReady,
        Charge,
        Dead,
    }
    [SerializeField]
    private BossState bossState = BossState.Spawning;

    private Transform playerTransform = null;
    private Collider2D myCollider2D = null;

    private Transform myTransform;
    private bool isDead = false;
    private Vector2 shiftPosition = Vector2.zero;
    private int hitPoint = 0;
    private float velocity = 0;
    private float elapsed = 0;
    private Vector2 readyPosition = Vector2.zero;
    private float result = 0;

    void Start()
    {
        myTransform = transform;
        myCollider2D = GetComponent<Collider2D>();
        playerTransform = MainGameRoot.Instance.playerOneRigidbody2D.transform;

        hitPoint = maxHitPoint;
        shiftPosition = maxShiftPosition;
        myTransform.position = new Vector3(playerTransform.position.x + shiftPosition.x, playerTransform.position.y + shiftPosition.y, 0);
        bossStockPosRect.position = MainGameRoot.Instance.cameraOne.WorldToScreenPoint(myTransform.position);
    }

    void Update()
    {
        var pos = myTransform.position;

        switch (bossState)
        {
            case BossState.Spawning:
                shiftPosition = Vector2.Lerp(minShiftPosition, maxShiftPosition, (float)hitPoint / maxHitPoint);
                break;
            case BossState.ChargeReady:
                elapsed += Time.deltaTime;
                shiftPosition = Vector2.Lerp(readyPosition, maxShiftPosition, elapsed / chargeReadyTime);
                // 指定した時間が経ったら、突進状態に移行する
                if (elapsed >= chargeReadyTime)
                {
                    elapsed = 0;
                    Debug.Log("突進");
                    bossState = BossState.Charge;
                }
                break;
            case BossState.Charge:
                velocity += acceleration * Time.deltaTime;
                result += velocity * Time.deltaTime;
                shiftPosition = Vector2.Lerp(maxShiftPosition, Vector2.zero, result);
                break;
        }

        pos.x = playerTransform.position.x + shiftPosition.x;
        myTransform.position = pos;
    }

    public void ApplyDamage(int damage)
    {
        hitPoint -= damage;
        // スポーン状態のときHPが0になったら、突進準備状態に移行する
        if(bossState == BossState.Spawning && hitPoint <= 0)
        {
            Debug.Log("突進準備");
            bossState = BossState.ChargeReady;
            readyPosition = myTransform.position - playerTransform.position;
        }
    }

    /// <summary>
    /// 自身をキルする
    /// </summary>
    public void Kill()
    {
        bossState -= BossState.Dead;
        myCollider2D.enabled = false;
        AudioControl.Instance.SetSEVol(seKilledVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seKilledClip, myTransform);
        MainGameRoot.Instance.KilledBoss();
    }

    /// <summary>
    /// プレイヤーに衝突した
    /// </summary>
    public void HitPlayer()
    {
        velocity = 0;
        result = 0;
        readyPosition = myTransform.position - playerTransform.position;
        bossState = BossState.ChargeReady;
    }
}
