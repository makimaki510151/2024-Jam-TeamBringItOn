using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : MonoBehaviour
{
    private enum BreadType
    {
        Small,
        Big,
    }

    [SerializeField, Tooltip("パンの種類")]
    private BreadType breadType = BreadType.Small;

    [SerializeField, Tooltip("プレイヤーのレイヤー")]
    private LayerMask playerLayerMask = default;

    [SerializeField]
    private float seEatPanVol = 1.0f;
    [SerializeField]
    private AudioClip seEatPanClip = null;

    private int playerIndex = 0;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var layer = 1 << collider.gameObject.layer;
        if((layer & playerLayerMask) != 0)
        {
            if(collider.GetComponent<Player>().character == Player.PlayCharacter.One)
            {
                playerIndex = 0;
            }
            else
            {
                playerIndex = 1;
            }
            switch(breadType)
            {
                case BreadType.Small:
                    BreadEatingCompetitionRoot.Instance.CountScore(playerIndex);
                    break;
                case BreadType.Big:
                    BreadEatingCompetitionRoot.Instance.DoubleScore(playerIndex);
                    break;
            }
            AudioControl.Instance.SetSEVol(seEatPanVol * MainGameRoot.Instance.dataScriptableObject.bgmVolSetting);
            AudioControl.Instance.PlaySE(seEatPanClip);
            Destroy(gameObject);
        }
    }
}
