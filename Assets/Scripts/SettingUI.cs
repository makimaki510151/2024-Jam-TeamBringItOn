using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField]
    private Slider seSlider = null;
    [SerializeField]
    private Slider bgmSlider = null;

    enum SceneType
    {
        Title,
        MainGame
    }

    [SerializeField]
    private SceneType sceneType = SceneType.Title;
    private float tempFloat = 0f;

    public void SeChange()
    {
        switch (sceneType)
        {
            case SceneType.Title:
                TitleRoot.Instance.dataScriptableObject.seVolSetting = seSlider.value;
                break;
            case SceneType.MainGame:
                break;
        }
    }
    public void BgmChange()
    {
        tempFloat = AudioControl.Instance.GetBGMVol();
        if (tempFloat > 0f)
        {
            switch (sceneType)
            {
                case SceneType.Title:
                    tempFloat /= TitleRoot.Instance.dataScriptableObject.bgmVolSetting;
                    break;
                case SceneType.MainGame:
                    break;
            }
        }
        switch (sceneType)
        {
            case SceneType.Title:
                TitleRoot.Instance.dataScriptableObject.bgmVolSetting = bgmSlider.value;
                tempFloat *= TitleRoot.Instance.dataScriptableObject.bgmVolSetting;
                break;
            case SceneType.MainGame:
                break;
        }

        AudioControl.Instance.SetBGMVol(tempFloat);
    }

    public void ButtonReturn()
    {
        gameObject.SetActive(false);
        switch (sceneType)
        {
            case SceneType.Title:
                TitleRoot.Instance.SettingClose();
                break;
            case SceneType.MainGame:
                break;
        }
    }
}
