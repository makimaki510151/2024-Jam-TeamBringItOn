using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherMenu : MonoBehaviour
{
    [SerializeField, Tooltip("決定ボタン")]
    private List<Button> confirmButtons = new List<Button>();

    [SerializeField, Tooltip("選択ボタン")]
    private List<GameObject> changeButtons = new List<GameObject>();

    [SerializeField, Tooltip("待機UI")]
    private GameObject waitUI = null;

    [SerializeField, Tooltip("エラーUI")]
    private GameObject errorUI = null;

    //[SerializeField, Tooltip("エラーUIのボタン")]
    //private Button errorUIButton = null;

    [SerializeField, Tooltip("キャラクターの移動速度")]
    private float characterSpeed = 1.0f;

    [SerializeField, Tooltip("設定を終えてからフェードアウトを開始するまでの時間")]
    private float fadeoutStartTime = 1.0f;
    private float fadeoutStartTimer = 0;

    [SerializeField, Tooltip("フェードアニメーター")]
    private Animator fadeAnimator = null;

    [SerializeField, Tooltip("フェードアニメーション")]
    private AnimationClip fadeAnimationClip = null;

    static readonly int isFadeout = Animator.StringToHash("isFadeout");

    [Header("設定項目")]

    [SerializeField, Tooltip("人数選択の画像")]
    private List<GameObject> playerAmountObjects = new List<GameObject>();

    //[SerializeField, Tooltip("操作選択の画像")]
    //private List<GameObject> operationObjects = new List<GameObject>();

    [SerializeField, Tooltip("キャラクター（1P）選択の画像")]
    private List<GameObject> characterOneObjects = new List<GameObject>();

    [SerializeField, Tooltip("キャラクター（2P）選択の画像")]
    private List<GameObject> characterTwoObjects = new List<GameObject>();

    [SerializeField, Tooltip("ステージ（1P）選択の画像")]
    private List<GameObject> stageOneObjects = new List<GameObject>();

    [SerializeField, Tooltip("ステージ（2P）選択の画像")]
    private List<GameObject> stageTwoObjects = new List<GameObject>();

    [Header("音設定")]

    [SerializeField]
    private float seCursorVol = 1.0f;
    [SerializeField]
    private AudioClip seCursorClip = null;

    [SerializeField]
    private float seChoiceVol = 1.0f;
    [SerializeField]
    private AudioClip seChoiceClip = null;

    private int selectCounter = 0;
    private int selectIndexOne = 0;
    private int selectIndexTwo = 0;
    private int waitCounter = 0;
    private float deltaTime = 0;
    private List<RectTransform> characterRectTransforms = new List<RectTransform>();
    private Vector2 pos = Vector2.zero;
    private bool isFinish = false;
    private bool isFade = false;

    void Update()
    {
        if (isFinish)
        {
            deltaTime = Time.deltaTime;
            for (int i = 0; i < characterRectTransforms.Count; i++)
            {
                pos = characterRectTransforms[i].position;
                pos.x += characterSpeed * deltaTime;
                characterRectTransforms[i].position = pos;
            }
            fadeoutStartTimer += deltaTime;
            if(!isFade && fadeoutStartTimer >= fadeoutStartTime)
            {
                StartCoroutine(OnFinish());
            }
        }
    }

    IEnumerator OnFinish()
    {
        fadeAnimator.SetTrigger(isFadeout);

        yield return new WaitForSeconds(fadeAnimationClip.length);

        ModeSelectRoot.Instance.ButtonPlayTwo();
    }

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
            //// 操作
            //case 1:
            //    selectIndexOne = ModeSelectRoot.Instance.dataScriptableObject.operationNumber;
            //    operationObjects[selectIndexOne].SetActive(true);
            //    break;
            // キャラクター
            case 1:
                selectIndexOne = ModeSelectRoot.Instance.dataScriptableObject.characterOneNumber;
                characterOneObjects[selectIndexOne].SetActive(true);
                selectIndexTwo = ModeSelectRoot.Instance.dataScriptableObject.characterTwoNumber;
                characterTwoObjects[selectIndexTwo].SetActive(true);
                waitCounter = 0;
                break;
            // ステージ
            case 2:
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
        //for (int i = 0; i < operationObjects.Count; i++)
        //{
        //    operationObjects[i].SetActive(false);
        //}
        for (int i = 0; i < stageOneObjects.Count; i++)
        {
            stageOneObjects[i].SetActive(false);
        }
        for (int i = 0; i < stageTwoObjects.Count; i++)
        {
            stageTwoObjects[i].SetActive(false);
        }

        // ステージ選択時でなければ、キャラクター選択も非表示にする
        if (selectCounter != 2)
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
            //// 操作
            //case 1:
            //    operationObjects[selectIndexOne].SetActive(false);
            //    selectIndexOne++;
            //    // 1Pのみ
            //    if (ModeSelectRoot.Instance.dataScriptableObject.playerAmountNumber == 0)
            //    {
            //        if (selectIndexOne >= 2) selectIndexOne = 0;
            //    }
            //    // 2Pあり
            //    else
            //    {
            //        if (selectIndexOne >= operationObjects.Count) selectIndexOne = 2;
            //    }
            //    operationObjects[selectIndexOne].SetActive(true);
            //    break;
            // キャラクター
            case 1:
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
            case 2:
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
        AudioControl.Instance.SetSEVol(seCursorVol * ModeSelectRoot.Instance.dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seCursorClip);
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
            //// 操作
            //case 1:
            //    operationObjects[selectIndexOne].SetActive(false);
            //    selectIndexOne--;
            //    // 1Pのみ
            //    if (ModeSelectRoot.Instance.dataScriptableObject.playerAmountNumber == 0)
            //    {
            //        if (selectIndexOne < 0) selectIndexOne = 1;
            //    }
            //    // 2Pあり
            //    else
            //    {
            //        if (selectIndexOne < 2) selectIndexOne = operationObjects.Count - 1;
            //    }
            //    operationObjects[selectIndexOne].SetActive(true);
            //    break;
            // キャラクター
            case 1:
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
            case 2:
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
        AudioControl.Instance.SetSEVol(seCursorVol * ModeSelectRoot.Instance.dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seCursorClip);
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
                    ModeSelectRoot.Instance.dataScriptableObject.operationNumber = 2;
                }
                selectCounter++;
                SetSelectObject();
                break;
            //// 操作
            //case 1:
            //    // コントローラー・ライバルのとき
            //    if (selectIndexOne == 4)
            //    {
            //        // 接続されているコントローラーが足りなかったら、エラーUIを表示する
            //        if(Input.GetJoystickNames().Length <= 1)
            //        {
            //            errorUI.SetActive(true);
            //            errorUIButton.Select();
            //            break;
            //        }
            //    }
            //    ModeSelectRoot.Instance.dataScriptableObject.operationNumber = selectIndexOne;
            //    selectCounter++;
            //    SetSelectObject();
            //    break;
            // キャラクター
            case 1:
                if(player == 0)
                {
                    ModeSelectRoot.Instance.dataScriptableObject.characterOneNumber = selectIndexOne;
                    waitUI.SetActive(true);
                }
                else
                {
                    ModeSelectRoot.Instance.dataScriptableObject.characterTwoNumber = selectIndexTwo;
                    waitUI.SetActive(false);
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
            case 2:
                if (player == 0)
                {
                    ModeSelectRoot.Instance.dataScriptableObject.stageOneNumber = selectIndexOne;
                    waitUI.SetActive(true);
                }
                else
                {
                    ModeSelectRoot.Instance.dataScriptableObject.stageTwoNumber = selectIndexTwo;
                    waitUI.SetActive(false);
                }

                waitCounter++;
                // 2人分の設定が終わったら設定を終える
                if (waitCounter >= 2)
                {
                    FinishSetting();
                }
                // まだ設定が終わっていなかったら、2人目の選択に移行する
                else
                {
                    confirmButtons[1].Select();
                }
                break;
        }
        AudioControl.Instance.SetSEVol(seChoiceVol * ModeSelectRoot.Instance.dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seChoiceClip);
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
            waitUI.SetActive(false);
            confirmButtons[0].Select();
        }
        else
        {
            selectCounter--;
            SetSelectObject();
        }
        AudioControl.Instance.SetSEVol(seChoiceVol * ModeSelectRoot.Instance.dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seChoiceClip);
    }

    /// <summary>
    /// エラーUIを非表示にします
    /// </summary>
    public void HideErrorUI()
    {
        errorUI.SetActive(false);
        SetSelectObject();
        confirmButtons[0].Select();
    }

    /// <summary>
    /// 設定を終えます
    /// </summary>
    private void FinishSetting()
    {
        characterOneObjects[ModeSelectRoot.Instance.dataScriptableObject.characterOneNumber].GetComponent<Animator>().enabled = true;
        characterTwoObjects[ModeSelectRoot.Instance.dataScriptableObject.characterTwoNumber].GetComponent<Animator>().enabled = true;
        characterRectTransforms.Add(characterOneObjects[ModeSelectRoot.Instance.dataScriptableObject.characterOneNumber].GetComponent<RectTransform>());
        characterRectTransforms.Add(characterTwoObjects[ModeSelectRoot.Instance.dataScriptableObject.characterTwoNumber].GetComponent<RectTransform>());
        for(int i = 0; i < confirmButtons.Count; i++)
        {
            confirmButtons[i].gameObject.SetActive(false);
        }
        for(int i = 0; i < changeButtons.Count; i++)
        {
            changeButtons[i].SetActive(false);
        }
        isFinish = true;
    }
}
