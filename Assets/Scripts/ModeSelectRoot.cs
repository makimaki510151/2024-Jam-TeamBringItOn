using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ModeSelectRoot : RootParent
{
    public EventSystem EventSystem { get => eventSystem; private set => eventSystem = value; }

    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField, Tooltip("メニュー")]
    private List<GameObject> menuObjects = new List<GameObject>();

    private int menuCount = 0;

    [SerializeField, Tooltip("メニューを開いたときに選択するボタン")]
    private List<Button> firstSelectMenuButtons = new List<Button>();

    [SerializeField, Tooltip("その他メニュースクリプト")]
    private OtherMenu otherMenu = null;

    [Header("音関係")]
    [SerializeField]
    private float bgmModeSelectVol = 1.0f;
    [SerializeField]
    private AudioClip bgmModeSelectClip = null;

    [SerializeField]
    private float seDecisionVol = 1.0f;
    [SerializeField]
    private AudioClip seDecisionClip = null;

    private bool isCoroutines = false;
    private AsyncOperation asyncLoad;

    private GameObject selectEndButtonObject;

    public static ModeSelectRoot Instance { get; private set; }

    public override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    void Start()
    {
        menuCount = menuObjects.Count;
        ShowMenu(0);

        AudioControl.Instance.SetBGMVol(bgmModeSelectVol * dataScriptableObject.bgmVolSetting);
        AudioControl.Instance.PlayBGM(bgmModeSelectClip);
    }

    private void Update()
    {
        if (selectEndButtonObject != eventSystem.currentSelectedGameObject && eventSystem.currentSelectedGameObject != null)
        {
            selectEndButtonObject = eventSystem.currentSelectedGameObject;
        }

        if (eventSystem.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(selectEndButtonObject);
        }
    }

    public void ButtonPlayTwo()
    {
        dataScriptableObject.playType = DataScriptableObject.PlayType.Two;
        dataScriptableObject.cameraRotation = 0;

        AudioControl.Instance.SetSEVol(seDecisionVol * dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seDecisionClip);

        LoadScene(2);
    }

    public void ButtonPlayOne()
    {
        dataScriptableObject.playType = DataScriptableObject.PlayType.One;
        dataScriptableObject.cameraRotation = 0;

        AudioControl.Instance.SetSEVol(seDecisionVol * dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seDecisionClip);

        LoadScene(2);
    }

    public void ButtonOmake()
    {
        dataScriptableObject.playType = DataScriptableObject.PlayType.Two;
        dataScriptableObject.cameraRotation = 180;

        AudioControl.Instance.SetSEVol(seDecisionVol * dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seDecisionClip);

        LoadScene(2);
    }

    public void LoadScene(int buildNumber)
    {
        if (!isCoroutines)
        {
            isCoroutines = true;
            StartCoroutine(OnLoadScene(buildNumber));
        }
    }

    IEnumerator OnLoadScene(int buildNumber)
    {
        asyncLoad = SceneManager.LoadSceneAsync(buildNumber);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    /// <summary>
    /// 指定したメニューを表示します
    /// </summary>
    /// <param name="menuNumber">表示したいメニュー番号</param>
    public void ShowMenu(int menuNumber)
    {
        for (int i = 0; i < menuCount; i++)
        {
            menuObjects[i].SetActive(false);
        }
        menuObjects[menuNumber].SetActive(true);
        firstSelectMenuButtons[menuNumber].Select();

        if(menuNumber == 2)
        {
            otherMenu.SetSelectObject();
        }
    }
}
