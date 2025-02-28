using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CompetitionMenu : MonoBehaviour
{
    [SerializeField, Tooltip("ボタン")]
    private List<GameObject> buttonObjects = new List<GameObject>();

    [SerializeField, Tooltip("吹き出しリスト")]
    private List<GameObject> SpeechBubbleObjects = new List<GameObject>();

    private EventSystem eventSystem = null;

    void Start()
    {
        eventSystem = ModeSelectRoot.Instance.EventSystem;
    }

    void Update()
    {
        // 選択されているボタンに応じて吹き出しを変更する
        for (int i = 0; i < buttonObjects.Count; i++)
        {
            if (eventSystem.currentSelectedGameObject == buttonObjects[i])
            {
                for(int j = 0; j < SpeechBubbleObjects.Count; j++)
                {
                    SpeechBubbleObjects[j].SetActive(false);
                }
                SpeechBubbleObjects[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// 競争モードを選択します
    /// </summary>
    /// <param name="isBreadMode">パン食い競争モードかどうか</param>
    public void SelectCompetitionMode(bool isBreadMode)
    {
        ModeSelectRoot.Instance.dataScriptableObject.isBreadMode = isBreadMode;
        ModeSelectRoot.Instance.ShowMenu(2);
    }

    /// <summary>
    /// 前のメニューに戻ります
    /// </summary>
    public void ReturnMenu()
    {
        ModeSelectRoot.Instance.ShowMenu(0);
    }
}
