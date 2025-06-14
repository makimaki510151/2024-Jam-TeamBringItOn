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

    [SerializeField]
    private float seKilledVol = 1.0f;
    [SerializeField]
    private AudioClip seKilledClip = null;

    private Transform playerTransform = null;

    private Transform myTransform;
    private bool isDead = false;
    private Vector2 shiftPosition = Vector2.zero;
    private int hitPoint = 0;

    void Start()
    {
        myTransform = transform;
        playerTransform = MainGameRoot.Instance.playerOneRigidbody2D.transform;

        hitPoint = maxHitPoint;
        shiftPosition = maxShiftPosition;
        myTransform.position = new Vector3(playerTransform.position.x + shiftPosition.x, playerTransform.position.y + shiftPosition.y, 0);
        bossStockPosRect.position = MainGameRoot.Instance.cameraOne.WorldToScreenPoint(myTransform.position);
    }

    void Update()
    {
        shiftPosition = Vector2.Lerp(minShiftPosition, maxShiftPosition, (float)hitPoint / maxHitPoint);

        var pos = myTransform.position;
        pos.x = playerTransform.position.x + shiftPosition.x;
        myTransform.position = pos;
    }

    public void ApplyDamage(int damage)
    {
        hitPoint -= damage;
        if(!isDead && hitPoint <= 0)
        {
            isDead = true;
            AudioControl.Instance.SetSEVol(seKilledVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
            AudioControl.Instance.PlaySE(seKilledClip, myTransform);
            MainGameRoot.Instance.KilledBoss();
        }
    }
}
