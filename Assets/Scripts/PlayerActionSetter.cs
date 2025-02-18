using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionSetter : MonoBehaviour
{
    // InputActionAssetのラッパークラス
    // 自動生成されたクラス名にする必要がある
    private PlayerAction playerAction;

    private Player playerOne;
    private Player playerTwo;

    void Start()
    {
        // InputActionAssetのラッパークラスをインスタンス化
        playerAction = new PlayerAction();

        playerOne = MainGameRoot.Instance.playerOneRigidbody2D.gameObject.GetComponent<Player>();
        playerTwo = MainGameRoot.Instance.playerTwoRigidbody2D.gameObject.GetComponent<Player>();

        // InputActionのコールバックの設定
        playerAction.Love.OneJump.started += playerOne.OnJump;
        playerAction.Love.OneShot.started += playerOne.OnShot;
        playerAction.Love.TwoJump.started += playerTwo.OnJump;
        playerAction.Love.TwoShot.started += playerTwo.OnShot;

        playerAction.Enable();
    }

    private void OnDestroy()
    {
        // InputActionのコールバックの解除
        playerAction.Love.OneJump.started -= playerOne.OnJump;
        playerAction.Love.TwoJump.started -= playerTwo.OnJump;

        // InputActionAssetのラッパークラスの破棄
        // IDisposableを実装しているので、Disposeする必要がある
        playerAction.Dispose();
    }

    private void OnEnable()
    {
        // 全体のActionを有効化
        if(playerAction != null) playerAction.Enable();
    }

    private void OnDisable()
    {
        // 全体のActionを無効化
        playerAction.Disable();
    }
}
