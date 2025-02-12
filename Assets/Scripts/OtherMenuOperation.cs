using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OtherMenuOperation : MonoBehaviour
{
    private OtherMenu otherMenu;
    private int userIndex;

    /// <summary>
    /// 次の選択肢に変更します
    /// </summary>
    /// <param name="context"></param>
    public void OnNextSelectObject(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            otherMenu.NextSelectObject(userIndex);
        }
    }

    /// <summary>
    /// 前の選択肢に変更します
    /// </summary>
    /// <param name="context"></param>
    public void OnBackSelectObject(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            otherMenu.BackSelectObject(userIndex);
        }
    }

    /// <summary>
    /// 決定ボタン
    /// </summary>
    /// <param name="context"></param>
    public void OnConfirmSelectObject(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            otherMenu.ConfirmSelectObject(userIndex);
        }
    }

    /// <summary>
    /// OtherMenuスクリプトを取得します
    /// </summary>
    /// <param name="o">OtherMenuスクリプト</param>
    public void SetOtherMenuScript(OtherMenu o)
    {
        otherMenu = o;
    }

    /// <summary>
    /// ユーザー番号を取得します
    /// </summary>
    public void SetUserIndex(int index)
    {
        userIndex = index;
    }
}
