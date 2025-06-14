using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUnit : MonoBehaviour
{
    [SerializeField, Tooltip("ヒットポイント")]
    private int hitPoint = 12;

    [SerializeField, Tooltip("ずらす値")]
    private Vector2 shiftPosition = Vector2.zero;

    [SerializeField, Tooltip("ストック位置")]
    private RectTransform bossStockPosRect = null;

    private Transform playerTransform = null;

    private Transform myTransform;

    void Start()
    {
        myTransform = transform;
        playerTransform = MainGameRoot.Instance.playerOneRigidbody2D.transform;

        myTransform.position = new Vector3(playerTransform.position.x + shiftPosition.x, playerTransform.position.y + shiftPosition.y, 0);
        bossStockPosRect.position = MainGameRoot.Instance.cameraOne.WorldToScreenPoint(myTransform.position);
    }

    void Update()
    {
        var pos = myTransform.position;
        pos.x = playerTransform.position.x + shiftPosition.x;
        myTransform.position = pos;
    }

    public void ApplyDamage(int damage)
    {
        hitPoint -= damage;
        Debug.Log(hitPoint);
        if(hitPoint <= 0)
        {
            MainGameRoot.Instance.KilledBoss();
            Destroy(gameObject);
        }
    }
}
