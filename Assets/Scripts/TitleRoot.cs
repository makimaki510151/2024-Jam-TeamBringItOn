using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleRoot : RootParent
{
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

    [SerializeField]
    private Animator TitleUIAnimator = null;

    static readonly int isShowButtonId = Animator.StringToHash("isShowButton");

    [SerializeField, Tooltip("タイトルアニメーション")]
    private AnimationClip titleAnimationClip = null;

    [SerializeField, Tooltip("ボタン表示アニメーション")]
    private AnimationClip showButtonAnimationClip = null;

    private float titleAnimationTimer = 0;

    [Header("音関係")]
    [SerializeField]
    private float bgmTitleVol = 1.0f;
    [SerializeField]
    private AudioClip bgmTitleClip = null;

    [SerializeField]
    private float seDecisionVol = 1.0f;
    [SerializeField]
    private AudioClip seDecisionClip = null;

    [SerializeField]
    private float seCursorVol = 1.0f;
    [SerializeField]
    private AudioClip seCursorClip = null;


    private GameObject selectEndButtonObject = null;

    public static TitleRoot Instance;
    private AsyncOperation asyncLoad;
    private bool isCoroutines = false;
    private const string InitialLoadingConfirmationId = "InitialLoadingConfirmation";
    private bool isTitleAnimation = true;
    private bool isWait = true;
    private GameObject oldSelectButtonObject;

    public override void Awake() 
    {  
        base.Awake();
        Instance = this;
    }

    public void OnReturn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SettingClose();
        }
    }

    private void Start()
    {
        AudioControl.Instance.SetBGMVol(bgmTitleVol * dataScriptableObject.bgmVolSetting);
        AudioControl.Instance.PlayBGM(bgmTitleClip);
    }

    private void Update()
    {
        if (isTitleAnimation)
        {
            EventSystem.current.SetSelectedGameObject(null);

            titleAnimationTimer += Time.deltaTime;
            if(titleAnimationTimer >= titleAnimationClip.length || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire1"))
            {
                StartCoroutine(OnEndTitleAnimation());
            }
        }
        else
        {
            if(!isWait)
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
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        if (eventSystem.currentSelectedGameObject != oldSelectButtonObject)
        {
            AudioControl.Instance.SetSEVol(seCursorVol * dataScriptableObject.seVolSetting);
            AudioControl.Instance.PlaySE(seCursorClip);
        }
        oldSelectButtonObject = eventSystem.currentSelectedGameObject;
    }

    IEnumerator OnEndTitleAnimation()
    {
        isTitleAnimation = false;
        TitleUIAnimator.SetTrigger(isShowButtonId);

        yield return new WaitForSeconds(showButtonAnimationClip.length);

        EventSystem.current.SetSelectedGameObject(mainFirstObject);
        isWait = false;
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
