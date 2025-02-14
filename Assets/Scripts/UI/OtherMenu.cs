using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherMenu : MonoBehaviour
{
    [SerializeField, Tooltip("決定ボタン")]
    private List<Button> confirmButtons = new List<Button>();

    [SerializeField, Tooltip("人数選択の画像")]
    private List<GameObject> playerAmountObjects = new List<GameObject>();

    [SerializeField, Tooltip("操作選択の画像")]
    private List<GameObject> operationObjects = new List<GameObject>();

    [SerializeField, Tooltip("キャラクター（1P）選択の画像")]
    private List<GameObject> characterOneObjects = new List<GameObject>();

    [SerializeField, Tooltip("キャラクター（2P）選択の画像")]
    private List<GameObject> characterTwoObjects = new List<GameObject>();

    [SerializeField, Tooltip("ステージ（1P）選択の画像")]
    private List<GameObject> stageOneObjects = new List<GameObject>();

    [SerializeField, Tooltip("ステージ（2P）選択の画像")]
    private List<GameObject> stageTwoObjects = new List<GameObject>();

    private int selectCounter = 0;
    private int selectIndexOne = 0;
    private int selectIndexTwo = 0;
    private int waitCounter = 0;

    /// <summary>
    /// 選択するものを設定します
    /// </summary>
    public void SetSelectObject()
    {
        HideSelectObject();
        switch (selectCounter)
        {
            // 人数
            case 0:
                selectIndexOne = ModeSelectRoot.Instance.dataScriptableObject.playerAmountNumber;
                playerAmountObjects[selectIndexOne].SetActive(true);
                break;
            // 操作
            case 1:
                selectIndexOne = ModeSelectRoot.Instance.dataScriptableObject.operationNumber;
                operationObjects[selectIndexOne].SetActive(true);
                break;
            // キャラクター
            case 2:
                selectIndexOne = ModeSelectRoot.Instance.dataScriptableObject.characterOneNumber;
                characterOneObjects[selectIndexOne].SetActive(true);
                selectIndexTwo = ModeSelectRoot.Instance.dataScriptableObject.characterTwoNumber;
                characterTwoObjects[selectIndexTwo].SetActive(true);
                waitCounter = 0;
                break;
            // ステージ
            case 3:
                selectIndexOne = ModeSelectRoot.Instance.dataScriptableObject.stageOneNumber;
                stageOneObjects[selectIndexOne].SetActive(true);
                selectIndexTwo = ModeSelectRoot.Instance.dataScriptableObject.stageTwoNumber;
                stageTwoObjects[selectIndexTwo].SetActive(true);
                waitCounter = 0;
                break;
        }
    }

    /// <summary>
    /// 選択するものを非表示にする
    /// </summary>
    private void HideSelectObject()
    {
        for (int i = 0; i < playerAmountObjects.Count; i++)
        {
            playerAmountObjects[i].SetActive(false);
        }
        for (int i = 0; i < operationObjects.Count; i++)
        {
            operationObjects[i].SetActive(false);
        }
        for (int i = 0; i < stageOneObjects.Count; i++)
        {
            stageOneObjects[i].SetActive(false);
        }
        for (int i = 0; i < stageTwoObjects.Count; i++)
        {
            stageTwoObjects[i].SetActive(false);
        }

        // ステージ選択時でなければ、キャラクター選択も非表示にする
        if (selectCounter != 3)
        {
            for (int i = 0; i < characterOneObjects.Count; i++)
            {
                characterOneObjects[i].SetActive(false);
            }
            for (int i = 0; i < characterTwoObjects.Count; i++)
            {
                characterTwoObjects[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 次の選択肢に変更します
    /// </summary>
    /// <param name="player">プレイヤー</param>
    public void NextSelectObject(int player)
    {
        switch(selectCounter)
        {
            // 人数
            case 0:
                playerAmountObjects[selectIndexOne].SetActive(false);
                selectIndexOne++;
                if (selectIndexOne >= playerAmountObjects.Count) selectIndexOne = 0;
                playerAmountObjects[selectIndexOne].SetActive(true);
                break;
            // 操作
            case 1:
                operationObjects[selectIndexOne].SetActive(false);
                selectIndexOne++;
                // 1Pのみ
                if (ModeSelectRoot.Instance.dataScriptableObject.playerAmountNumber == 0)
                {
                    if (selectIndexOne >= 2) selectIndexOne = 0;
                }
                // 2Pあり
                else
                {
                    if (selectIndexOne >= operationObjects.Count) selectIndexOne = 2;
                }
                operationObjects[selectIndexOne].SetActive(true);
                break;
            // キャラクター
            case 2:
                if(player == 0)
                {
                    characterOneObjects[selectIndexOne].SetActive(false);
                    selectIndexOne++;
                    if (selectIndexOne >= characterOneObjects.Count) selectIndexOne = 0;
                    characterOneObjects[selectIndexOne].SetActive(true);
                }
                else
                {
                    characterTwoObjects[selectIndexTwo].SetActive(false);
                    selectIndexTwo++;
                    if (selectIndexTwo >= characterTwoObjects.Count) selectIndexTwo = 0;
                    characterTwoObjects[selectIndexTwo].SetActive(true);
                }
                break;
            // ステージ
            case 3:
                if (player == 0)
                {
                    stageOneObjects[selectIndexOne].SetActive(false);
                    selectIndexOne++;
                    if (selectIndexOne >= stageOneObjects.Count) selectIndexOne = 0;
                    stageOneObjects[selectIndexOne].SetActive(true);
                }
                else
                {
                    stageTwoObjects[selectIndexTwo].SetActive(false);
                    selectIndexTwo++;
                    if (selectIndexTwo >= stageTwoObjects.Count) selectIndexTwo = 0;
                    stageTwoObjects[selectIndexTwo].SetActive(true);
                }
                break;
        }
    }

    /// <summary>
    /// 前の選択肢に変更します
    /// </summary>
    /// <param name="player"></param>
    public void BackSelectObject(int player)
    {
        switch(selectCounter)
        {
            // 人数
            case 0:
                playerAmountObjects[selectIndexOne].SetActive(false);
                selectIndexOne--;
                if (selectIndexOne < 0) selectIndexOne = playerAmountObjects.Count - 1;
                playerAmountObjects[selectIndexOne].SetActive(true);
                break;
            // 操作
            case 1:
                operationObjects[selectIndexOne].SetActive(false);
                selectIndexOne--;
                // 1Pのみ
                if (ModeSelectRoot.Instance.dataScriptableObject.playerAmountNumber == 0)
                {
                    if (selectIndexOne < 0) selectIndexOne = 1;
                }
                // 2Pあり
                else
                {
                    if (selectIndexOne < 2) selectIndexOne = operationObjects.Count - 1;
                }
                operationObjects[selectIndexOne].SetActive(true);
                break;
            // キャラクター
            case 2:
                if (player == 0)
                {
                    characterOneObjects[selectIndexOne].SetActive(false);
                    selectIndexOne--;
                    if (selectIndexOne < 0) selectIndexOne = characterOneObjects.Count - 1;
                    characterOneObjects[selectIndexOne].SetActive(true);
                }
                else
                {
                    characterTwoObjects[selectIndexTwo].SetActive(false);
                    selectIndexTwo--;
                    if (selectIndexTwo < 0) selectIndexTwo = characterTwoObjects.Count - 1;
                    characterTwoObjects[selectIndexTwo].SetActive(true);
                }
                break;
            // ステージ
            case 3:
                if (player == 0)
                {
                    stageOneObjects[selectIndexOne].SetActive(false);
                    selectIndexOne--;
                    if (selectIndexOne < 0) selectIndexOne = stageOneObjects.Count - 1;
                    stageOneObjects[selectIndexOne].SetActive(true);
                }
                else
                {
                    stageTwoObjects[selectIndexTwo].SetActive(false);
                    selectIndexTwo--;
                    if (selectIndexTwo < 0) selectIndexTwo = stageTwoObjects.Count - 1;
                    stageTwoObjects[selectIndexTwo].SetActive(true);
                }
                break;
        }
    }

    /// <summary>
    /// 決定ボタン
    /// </summary>
    /// <param name="player">プレイヤー</param>
    public void ConfirmSelectObject(int player)
    {
        switch(selectCounter)
        {
            // 人数
            case 0:
                ModeSelectRoot.Instance.dataScriptableObject.playerAmountNumber = selectIndexOne;
                // 1Pvs2Pなら、最初に表示する画像を変更する
                if(selectIndexOne == 1)
                {
                    ModeSelectRoot.Instance.dataScriptableObject.playerAmountNumber = 2;
                }
                selectCounter++;
                SetSelectObject();
                break;
            // 操作
            case 1:
                ModeSelectRoot.Instance.dataScriptableObject.operationNumber = selectIndexOne;
                selectCounter++;
                SetSelectObject();
                break;
            // キャラクター
            case 2:
                if(player == 0)
                {
                    ModeSelectRoot.Instance.dataScriptableObject.characterOneNumber = selectIndexOne;
                    //----- ここでちょっとまってね画像を表示 -----//
                }
                else
                {
                    ModeSelectRoot.Instance.dataScriptableObject.characterTwoNumber = selectIndexTwo;
                    //----- ここでちょっとまってね画像を表示 -----//
                }

                waitCounter++;
                // 2人分の設定が終わったら次の設定画面に移行する
                if (waitCounter >= 2)
                {
                    waitCounter = 0;
                    confirmButtons[0].Select();
                    selectCounter++;
                    SetSelectObject();
                }
                // まだ設定が終わっていなかったら、2人目の選択に移行する
                else
                {
                    confirmButtons[1].Select();
                }
                break;
            // ステージ
            case 3:
                if (player == 0)
                {
                    ModeSelectRoot.Instance.dataScriptableObject.stageOneNumber = selectIndexOne;
                    //----- ここでちょっとまってね画像を表示 -----//
                }
                else
                {
                    ModeSelectRoot.Instance.dataScriptableObject.stageTwoNumber = selectIndexTwo;
                    //----- ここでちょっとまってね画像を表示 -----//
                }

                waitCounter++;
                if (waitCounter >= 2)
                {
                    //----- ここでメインシーンへ移行処理 -----//
                    Debug.Log("設定完了！！！れ！！！！");
                }
                // まだ設定が終わっていなかったら、2人目の選択に移行する
                else
                {
                    confirmButtons[1].Select();
                }
                break;
        }
    }

    /// <summary>
    /// ひとつ前の設定に戻ります
    /// </summary>
    public void ReturnSelectObject()
    {
        // 一番前の状態から戻ったら、競争モード選択画面に移行します
        if (selectCounter <= 0)
        {
            selectCounter = 0;
            ModeSelectRoot.Instance.ShowMenu(1);
        }
        else if(waitCounter == 1)
        {
            waitCounter = 0;
            confirmButtons[0].Select();
        }
        else
        {
            selectCounter--;
            SetSelectObject();
        }
    }
}
