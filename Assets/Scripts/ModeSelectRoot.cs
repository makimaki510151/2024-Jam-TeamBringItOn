using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ModeSelectRoot : RootParent
{
    [SerializeField]
    private EventSystem eventSystem;

    private bool isCoroutines = false;
    private AsyncOperation asyncLoad;

    private GameObject selectEndButtonObject;

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
        StartCoroutine(LoadYourAsyncScene("MainGame"));
    }

    public void ButtonPlayOne()
    {
        if (isCoroutines) return;
        isCoroutines = true;
        dataScriptableObject.playType = DataScriptableObject.PlayType.One;
        dataScriptableObject.cameraRotation = 0;
        StartCoroutine(LoadYourAsyncScene("MainGame"));
    }

    public void ButtonManual()
    {
        Debug.Log("ÇﬂÇ…Ç„Å`");
    }

    public void ButtonOmake()
    {
        if (isCoroutines) return;
        isCoroutines = true;
        dataScriptableObject.playType = DataScriptableObject.PlayType.Two;
        dataScriptableObject.cameraRotation = 180;
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
