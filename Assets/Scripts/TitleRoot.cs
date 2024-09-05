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

    private GameObject selectEndButtonObject = null;

    public static TitleRoot Instance;
    private AsyncOperation asyncLoad;
    private bool isCoroutines = false;

    public override void Awake() 
    {  
        base.Awake();
        Instance = this;
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
        StartCoroutine(LoadYourAsyncScene("ModeSelect"));
    }

    public void ButtonGamedEnd()
    {
        Application.Quit();
    }
    public void ButtonSetting()
    {
        settingObject.SetActive(true);
        for(int i = 0; i < mainButtons.Count; i++)
        {
            mainButtons[i].interactable = false;
        }
        EventSystem.current.SetSelectedGameObject(settingFirstObject);
    }

    public void SettingClose()
    {
        for (int i = 0; i < mainButtons.Count; i++)
        {
            mainButtons[i].interactable = true;
        }
        EventSystem.current.SetSelectedGameObject(mainFirstObject);
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
