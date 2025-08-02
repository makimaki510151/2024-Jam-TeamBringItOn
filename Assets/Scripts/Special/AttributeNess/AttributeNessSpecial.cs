using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeNessSpecial : PlayerSpecial
{
    [SerializeField, Tooltip("アニメーター")]
    private Animator playerAnimator = null;

    [SerializeField, Tooltip("スペシャルアニメーション")]
    private AnimationClip specialFireClip = null;

    [SerializeField, Tooltip("キャラクター位置")]
    private Transform characterTransform = null;

    [SerializeField, Tooltip("エフェクト")]
    private GameObject specialEffect01 = null;

    [SerializeField, Tooltip("コライダー")]
    private Collider2D playerCollider2D = null;

    static readonly int isSpacialId = Animator.StringToHash("isSpecial");

    private GameObject specialEffectObject;
    private Rigidbody2D playerRigidbody2D;
    private float elapsed = 0;
    private float groundPosY = 0;

    public override void Init()
    {
        base.Init();
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        groundPosY = transform.position.y;
    }

    public override void SetSpecialAttackReadyUI(bool isActive)
    {
        MainGameRoot.Instance.SetSpecialAttackUI(player.character, 2, isActive);
    }

    public override void StartSpecial()
    {
        base.StartSpecial();
        player.AllHeal();
        player.invincibleTimer = 999;
        elapsed = 0;
        playerCollider2D.enabled = false;
        playerRigidbody2D.gravityScale = 0;
        player.isSpecialBan = true;
        var pos = transform.position;
        pos.y = groundPosY;
        transform.position = pos;
        playerAnimator.SetTrigger(isSpacialId);
        MainGameRoot.Instance.ShowSpecialAnimator(player.character, 2);
    }

    public override void EndSpecial()
    {
        base.EndSpecial();
        player.invincibleTimer = 0.01f;
        Destroy(specialEffectObject);
        playerCollider2D.enabled = true;
        playerRigidbody2D.gravityScale = 2.5f;
        player.isSpecialBan = false;
        MainGameRoot.Instance.EndSpecialAnimator(player.character, 2);
    }

    public void ShowEffect()
    {
        specialEffectObject = Instantiate(specialEffect01, transform);
        specialEffectObject.transform.position = characterTransform.position;
        specialEffectObject.GetComponent<AttributeNessSpecialEffect>().Init(player);
    }

    void Update()
    {
        if (isActive)
        {
            var vel = playerRigidbody2D.velocity;
            vel.y = 0;
            playerRigidbody2D.velocity = vel;

            elapsed += Time.deltaTime;
            if (elapsed >= specialFireClip.length)
            {
                EndSpecial();
            }
        }
    }
}
