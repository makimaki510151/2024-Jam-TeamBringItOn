using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectRoot : RootParent
{
    private bool isCoroutines = false;
    private AsyncOperation asyncLoad;

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
        StartCoroutine(LoadYourAsyncScene("MainGame"));
    }

    public void ButtonPlayOne()
    {
        if (isCoroutines) return;
        isCoroutines = true;
        dataScriptableObject.playType = DataScriptableObject.PlayType.One;
        StartCoroutine(LoadYourAsyncScene("MainGame"));
    }

    public void ButtonManual()
    {
        Debug.Log("ÇﬂÇ…Ç„Å`");
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
