using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class NonPlayerController : MonoBehaviour
{
    [SerializeField, Tooltip("レイ（右）の長さ")]
    private float rayRightDistance = 1.0f;

    [SerializeField, Tooltip("レイ（右）空中時の長さ")]
    private float rayRightNotGroundDistance = 1.0f;

    [SerializeField, Tooltip("レイ（右上）の長さ")]
    private float rayUpperRightDistance = 1.0f;

    [SerializeField, Tooltip("レイ（右下）の長さ")]
    private float rayLowerRightDistance = 1.0f;

    [SerializeField, Tooltip("レイ（下）の長さ")]
    private float rayDownDistance = 1.0f;

    [SerializeField, Tooltip("プレイヤーのレイヤー")]
    private LayerMask playerLayerMask = default;

    [SerializeField, Tooltip("敵のタグ")]
    private string enemyTag = "Enemy";

    [SerializeField, Tooltip("ブロックのタグ")]
    private string gimmickGroundTag = "GimmickGround";

    private Player player;
    private Transform playerTransform;
    RaycastHit2D hit;

    void Start()
    {
        player = GetComponentInParent<Player>();
        playerTransform = player.transform;
    }

    void Update()
    {
        if(player.isGround)
        {
            hit = Physics2D.Raycast(playerTransform.position, playerTransform.right, rayRightDistance, ~playerLayerMask);
            Debug.DrawRay(playerTransform.position, playerTransform.right * rayRightDistance, Color.red);
            if (hit.collider)
            {
                if (hit.collider.CompareTag(enemyTag) || hit.collider.CompareTag(gimmickGroundTag))
                {
                    player.SetJump();
                    Debug.Log("右");
                }
            }

            hit = Physics2D.Raycast(playerTransform.position, playerTransform.right + playerTransform.up / 2, rayUpperRightDistance, ~playerLayerMask);
            Debug.DrawRay(playerTransform.position, playerTransform.right + playerTransform.up / 2 * rayUpperRightDistance, Color.red);
            if (hit.collider)
            {
                if (hit.collider.CompareTag(enemyTag))
                {
                    player.SetJump();
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
                    Debug.Log("空中右");
                }
            }

            hit = Physics2D.Raycast(playerTransform.position, playerTransform.right + playerTransform.up / 2, rayUpperRightDistance, ~playerLayerMask);
            Debug.DrawRay(playerTransform.position, playerTransform.right + playerTransform.up / 2 * rayUpperRightDistance, Color.red);
            if (hit.collider)
            {
                if (hit.collider.CompareTag(enemyTag))
                {
                    player.SetJump();
                    Debug.Log("右上");
                }
            }

            hit = Physics2D.Raycast(playerTransform.position, playerTransform.right - playerTransform.up / 2, rayLowerRightDistance, ~playerLayerMask);
            Debug.DrawRay(playerTransform.position, playerTransform.right - playerTransform.up * rayLowerRightDistance, Color.red);
            if (hit.collider)
            {
                if (hit.collider.CompareTag(enemyTag))
                {
                    player.SetJump();
                    Debug.Log("右下");
                }
            }

            hit = Physics2D.Raycast(playerTransform.position, -playerTransform.up, rayDownDistance, ~playerLayerMask);
            Debug.DrawRay(playerTransform.position, -playerTransform.up * rayDownDistance, Color.red);
            if (hit.collider)
            {
                if (hit.collider.CompareTag(enemyTag))
                {
                    player.SetJump();
                    Debug.Log("下");
                }
            }
        }
    }
}
