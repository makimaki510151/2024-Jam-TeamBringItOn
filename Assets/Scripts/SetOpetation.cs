using UnityEngine;
using UnityEngine.InputSystem;

public class SetOpetation : MonoBehaviour
{
    [SerializeField, Tooltip("OtherMenuスクリプト")]
    private OtherMenu otherMenu = null;

    OtherMenuOperation otherMenuOperation;

    // プレイヤー入室時に受け取る通知
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        otherMenuOperation = playerInput.gameObject.GetComponent<OtherMenuOperation>();
        otherMenuOperation.SetOtherMenuScript(otherMenu);
        otherMenuOperation.SetUserIndex(playerInput.user.index);
    }

    // プレイヤー退室時に受け取る通知
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        print($"プレイヤー#{playerInput.user.index}が退室！");
    }
}
