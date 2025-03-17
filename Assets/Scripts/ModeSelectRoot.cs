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
    private float bgmAreaSelectVol = 1.0f;
    [SerializeField]
    private AudioClip bgmAreaSelectClip = null;

    //[SerializeField]
    //private float seDecisionVol = 1.0f;
    //[SerializeField]
    //private AudioClip seDecisionClip = null;

    [SerializeField]
    private float seCursorVol = 1.0f;
    [SerializeField]
    private AudioClip seCursorClip = null;

    [SerializeField]
    private float seChoiceVol = 1.0f;
    [SerializeField]
    private AudioClip seChoiceClip = null;

    private bool isCoroutines = false;
    private AsyncOperation asyncLoad;

    private GameObject selectEndButtonObject;
    private GameObject oldSelectButtonObject;

    public static ModeSelectRoot Instance { get; private set; }

    public override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    void Start()
    {
        dataScriptableObject.isTutorial = false;
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

        if(eventSystem.currentSelectedGameObject != oldSelectButtonObject)
        {
            AudioControl.Instance.SetSEVol(seCursorVol * dataScriptableObject.seVolSetting);
            AudioControl.Instance.PlaySE(seCursorClip);
        }
        oldSelectButtonObject = eventSystem.currentSelectedGameObject;
    }

    public void ButtonPlayTwo()
    {
        dataScriptableObject.playType = DataScriptableObject.PlayType.Two;
        dataScriptableObject.cameraRotation = 0;

        LoadScene(2);
    }

    public void ButtonPlayOne()
    {
        dataScriptableObject.playType = DataScriptableObject.PlayType.One;
        dataScriptableObject.cameraRotation = 0;
        dataScriptableObject.isBreadMode = false;

        LoadScene(2);
    }

    public void ButtonOmake()
    {
        dataScriptableObject.playType = DataScriptableObject.PlayType.Two;
        dataScriptableObject.cameraRotation = 180;

        LoadScene(2);
    }

    public void ButtonTutorial()
    {
        dataScriptableObject.playType = DataScriptableObject.PlayType.One;
        dataScriptableObject.isTutorial = true;
        dataScriptableObject.isBreadMode = false;

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
        
        if(menuNumber == 4)
        {
            AudioControl.Instance.SetBGMVol(bgmAreaSelectVol * dataScriptableObject.bgmVolSetting);
            AudioControl.Instance.PlayBGM(bgmAreaSelectClip);
        }
        else
        {
            AudioControl.Instance.SetBGMVol(bgmModeSelectVol * dataScriptableObject.bgmVolSetting);
            AudioControl.Instance.PlayBGM(bgmModeSelectClip);
        }

        AudioControl.Instance.SetSEVol(seChoiceVol * dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seChoiceClip);
    }
}
