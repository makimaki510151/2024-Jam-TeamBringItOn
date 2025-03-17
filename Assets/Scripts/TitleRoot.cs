using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleRoot : RootParent
{
    [SerializeField]
    private GameObject settingObject = null;
    [SerializeField]
    private List<Button> mainButtons = new();
    [SerializeField]
    private EventSystem eventSystem = null;
    [SerializeField]
    private GameObject settingFirstObject = null;
    [SerializeField]
    private GameObject mainFirstObject = null;

    [SerializeField]
    private SettingUI settingUI = null;

    [Header("‰¹ŠÖŒW")]
    [SerializeField]
    private float bgmTitleVol = 1.0f;
    [SerializeField]
    private AudioClip bgmTitleClip = null;

    [SerializeField]
    private float seDecisionVol = 1.0f;
    [SerializeField]
    private AudioClip seDecisionClip = null;


    private GameObject selectEndButtonObject = null;

    public static TitleRoot Instance;
    private AsyncOperation asyncLoad;
    private bool isCoroutines = false;
    private const string InitialLoadingConfirmationId = "InitialLoadingConfirmation";

    public override void Awake() 
    {  
        base.Awake();
        Instance = this;
    }
    private void Start()
    {
        AudioControl.Instance.SetBGMVol(bgmTitleVol * dataScriptableObject.bgmVolSetting);
        AudioControl.Instance.PlayBGM(bgmTitleClip);
    }

    private void Update()
    {
        if(selectEndButtonObject != eventSystem.currentSelectedGameObject && eventSystem.currentSelectedGameObject != null)
        {
            selectEndButtonObject = eventSystem.currentSelectedGameObject;
        }
        
        if(eventSystem.currentSelectedGameObject == null) 
        {
            EventSystem.current.SetSelectedGameObject(selectEndButtonObject);
        }
    }

    public void ButtonNext()
    {
        if (isCoroutines) return;
        isCoroutines = true;
        dataScriptableObject.playType = DataScriptableObject.PlayType.Two;

        AudioControl.Instance.SetSEVol(seDecisionVol * dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seDecisionClip);

        if(PlayerPrefs.GetInt(InitialLoadingConfirmationId, 0) == 0)
        {
            PlayerPrefs.SetInt(InitialLoadingConfirmationId, 1);
            dataScriptableObject.playType = DataScriptableObject.PlayType.One;
            dataScriptableObject.isTutorial = true;
            dataScriptableObject.isBreadMode = false;

            StartCoroutine(LoadYourAsyncScene(2));
        }
        else
        {
            StartCoroutine(LoadYourAsyncScene(1));
        }
    }

    public void ButtonGamedEnd()
    {
        Application.Quit();
    }
    public void ButtonSetting()
    {
        settingUI.Show();
        for (int i = 0; i < mainButtons.Count; i++)
        {
            mainButtons[i].interactable = false;
        }

        EventSystem.current.SetSelectedGameObject(settingFirstObject);
    }

    public void SettingClose()
    {
        settingUI.Hide();
        for (int i = 0; i < mainButtons.Count; i++)
        {
            mainButtons[i].interactable = true;
        }
        EventSystem.current.SetSelectedGameObject(mainFirstObject);
    }
    IEnumerator LoadYourAsyncScene(int buildNumber)
    {
        asyncLoad = SceneManager.LoadSceneAsync(buildNumber);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
