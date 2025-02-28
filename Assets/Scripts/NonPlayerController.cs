using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class NonPlayerController : MonoBehaviour
{
    [Header("監視設定")]

    [SerializeField, Tooltip("レイ（右）の長さ")]
    private float rayRightDistance = 1.0f;

    [SerializeField, Tooltip("レイ（右）空中時の長さ")]
    private float rayRightNotGroundDistance = 1.0f;

    [SerializeField, Tooltip("レイ（右上）の長さ")]
    private float rayUpperRightDistance = 1.0f;

    [SerializeField, Tooltip("レイ（右上）空中時の長さ")]
    private float rayUpperRightNotGroundDistance = 1.0f;

    [SerializeField, Tooltip("レイ（右下）の長さ")]
    private float rayLowerRightDistance = 1.0f;

    [SerializeField, Tooltip("レイ（下）の長さ")]
    private float rayDownDistance = 1.0f;

    [SerializeField, Tooltip("レイ（上）の長さ")]
    private float rayUpDistance = 1.0f;

    [SerializeField, Tooltip("プレイヤーのレイヤー")]
    private LayerMask playerLayerMask = default;

    [SerializeField, Tooltip("敵のタグ")]
    private string enemyTag = "Enemy";

    [SerializeField, Tooltip("ブロックのタグ")]
    private string gimmickGroundTag = "GimmickGround";

    [SerializeField, Tooltip("待機時間")]
    private float delayTime = 0.1f;
    private float delayTimer = 0;

    [SerializeField, Tooltip("待機氷結時間")]
    private float delayFrozenTime = 0.25f;
    private float delayFrozenTimer = 0;

    [Header("パリィ砲設定")]

    [SerializeField, Tooltip("パリィ砲待機時間")]
    private float shotDelayTime = 3.0f;
    private float shotDelayTimer = 0;

    private float deltaTime;
    private Player player;
    private Transform playerTransform;
    RaycastHit2D hit;
    private bool isDelay = false;
    private int rand = 0;
    private Rigidbody2D playerRigidbody2D;

    void Start()
    {
        player = GetComponentInParent<Player>();
        playerRigidbody2D = player.gameObject.GetComponent<Rigidbody2D>();
        playerTransform = player.transform;
    }

    void Update()
    {
        deltaTime = Time.deltaTime;

        // 氷結中
        if (player.isFrozen)
        {
            delayFrozenTimer += deltaTime;
            if(delayFrozenTimer >= delayFrozenTime)
            {
                player.SetJump();
                delayFrozenTimer = 0;
            }
        }
        // 待機
        if (isDelay)
        {
            delayTimer += deltaTime;
            if (delayTimer >= delayTime)
            {
                delayTimer = 0;
                isDelay = false;
            }
        }
        // 監視
        else
        {
            if (player.isGround)
            {
                hit = Physics2D.Raycast(playerTransform.position, playerTransform.right, rayRightDistance, ~playerLayerMask);
                Debug.DrawRay(playerTransform.position, playerTransform.right * rayRightDistance, Color.red);
                if (hit.collider)
                {
                    if (hit.collider.CompareTag(enemyTag) || hit.collider.CompareTag(gimmickGroundTag))
                    {
                        player.SetJump();
                        isDelay = true;
                        Debug.Log("右");
                    }
                }

                hit = Physics2D.Raycast(playerTransform.position, playerTransform.right + playerTransform.up / 3, rayUpperRightDistance, ~playerLayerMask);
                Debug.DrawRay(playerTransform.position, (playerTransform.right + playerTransform.up / 3) * rayUpperRightDistance, Color.red);
                if (hit.collider)
                {
                    if (hit.collider.CompareTag(enemyTag))
                    {
                        player.SetJump();
                        isDelay = true;
                        Debug.Log("右上");
                    }
                }
            }
            else
            {
                hit = Physics2D.Raycast(playerTransform.position, playerTransform.right, rayRightNotGroundDistance, ~playerLayerMask);
                Debug.DrawRay(playerTransform.position, playerTransform.right * rayRightNotGroundDistance, Color.red);
                if (hit.collider)
                {
                    if (hit.collider.CompareTag(enemyTag) || hit.collider.CompareTag(gimmickGroundTag))
                    {
                        player.SetJump();
                        isDelay = true;
                        Debug.Log("空中右");
                    }
                }

                hit = Physics2D.Raycast(playerTransform.position, playerTransform.right + playerTransform.up / 2, rayUpperRightNotGroundDistance, ~playerLayerMask);
                Debug.DrawRay(playerTransform.position, (playerTransform.right + playerTransform.up / 2) * rayUpperRightNotGroundDistance, Color.red);
                if (hit.collider)
                {
                    if (hit.collider.CompareTag(enemyTag))
                    {
                        player.SetJump();
                        isDelay = true;
                        Debug.Log("空中右上");
                    }
                }

                hit = Physics2D.Raycast(playerTransform.position, playerTransform.right - playerTransform.up, rayLowerRightDistance, ~playerLayerMask);
                Debug.DrawRay(playerTransform.position, playerTransform.right - playerTransform.up * rayLowerRightDistance, Color.red);
                if (hit.collider)
                {
                    if (hit.collider.CompareTag(enemyTag) && playerRigidbody2D.velocity.y <= 0)
                    {
                        player.SetJump();
                        isDelay = true;
                        Debug.Log("右下");
                    }
                }

                hit = Physics2D.Raycast(playerTransform.position, -playerTransform.up, rayDownDistance, ~playerLayerMask);
                Debug.DrawRay(playerTransform.position, -playerTransform.up * rayDownDistance, Color.red);
                if (hit.collider)
                {
                    if (hit.collider.CompareTag(enemyTag) && playerRigidbody2D.velocity.y <= 0)
                    {
                        player.SetJump();
                        isDelay = true;
                        Debug.Log("下");
                    }
                }

                hit = Physics2D.Raycast(playerTransform.position, playerTransform.up, rayUpDistance, ~playerLayerMask);
                Debug.DrawRay(playerTransform.position, playerTransform.up * rayUpDistance, Color.red);
                if (hit.collider)
                {
                    if (hit.collider.CompareTag(enemyTag) && playerRigidbody2D.velocity.y > 0)
                    {
                        player.SetJump();
                        isDelay = true;
                        Debug.Log("上");
                    }
                }
            }
        }

        // パリィ砲
        if(MainGameRoot.Instance.twoStockCount > 0)
        {
            if(shotDelayTimer > 0)
            {
                shotDelayTime -= deltaTime;
                if(shotDelayTime <= 0)
                {
                    shotDelayTimer = -1;
                    rand = Random.Range(1, 9);
                }
            }
            else
            {
                if(rand <= 5)
                {
                    for(int i = 1; i <= rand; i++)
                    {
                        player.SetShot();
                    }
                }
                else
                {

                }
            }
        }
        
    }
}
