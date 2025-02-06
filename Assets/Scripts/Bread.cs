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

    private int playerIndex = 0;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var layer = 1 << collider.gameObject.layer;
        if((layer & playerLayerMask) != 0)
        {
            if(collider.GetComponent<Player>().Character == Player.PlayCharacter.Water)
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
            Destroy(gameObject);
        }
    }
}
