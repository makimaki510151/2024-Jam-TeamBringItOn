using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeSelectRoot : RootParent
{
    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField]
    private Image guideImage = null;
    private GameObject guideObject = null;
    [SerializeField]
    private Sprite[] guideSprites = new Sprite[2];
    private int guideCount = 1;

    [Header("‰¹ŠÖŒW")]
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

    private void Start()
    {
        AudioControl.Instance.SetBGMVol(bgmModeSelectVol * dataScriptableObject.bgmVolSetting);
        AudioControl.Instance.PlayBGM(bgmModeSelectClip);

        guideObject = guideImage.gameObject;
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

    public void ButtonRetrun()
    {
        if (isCoroutines) return;
        isCoroutines = true;

        StartCoroutine(LoadYourAsyncScene("Title"));
    }

    public void ButtonPlayTwo()
    {
        if (isCoroutines) return;
        isCoroutines = true;
        dataScriptableObject.playType = DataScriptableObject.PlayType.Two;
        dataScriptableObject.cameraRotation = 0;

        AudioControl.Instance.SetSEVol(seDecisionVol * dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seDecisionClip);

        StartCoroutine(LoadYourAsyncScene("MainGame"));
    }

    public void ButtonPlayOne()
    {
        if (isCoroutines) return;
        isCoroutines = true;
        dataScriptableObject.playType = DataScriptableObject.PlayType.One;
        dataScriptableObject.cameraRotation = 0;

        AudioControl.Instance.SetSEVol(seDecisionVol * dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seDecisionClip);

        StartCoroutine(LoadYourAsyncScene("MainGame"));
    }

    public void ButtonManual()
    {
        guideObject.SetActive(true);
    }
    public void ButtonManualClose()
    {
        guideObject.SetActive(false);
    }

    public void ButtonManualPage()
    {
        guideImage.sprite = guideSprites[guideCount];
        guideCount++;
        if(guideCount == guideSprites.Length)
        {
            guideCount = 0;
        }
    }


    public void ButtonOmake()
    {
        if (isCoroutines) return;
        isCoroutines = true;
        dataScriptableObject.playType = DataScriptableObject.PlayType.Two;
        dataScriptableObject.cameraRotation = 180;

        AudioControl.Instance.SetSEVol(seDecisionVol * dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seDecisionClip);

        StartCoroutine(LoadYourAsyncScene("MainGame"));
    }
    IEnumerator LoadYourAsyncScene(string name)
    {
        asyncLoad = SceneManager.LoadSceneAsync(name);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
