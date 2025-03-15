using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField]
    private Slider seSlider = null;
    [SerializeField]
    private Slider bgmSlider = null;

    [SerializeField]
    private Button voiceButton = null;

    private Image voiceButtonImage = null;

    [SerializeField]
    private Sprite voiceButtonOffSprite = null;

    [SerializeField]
    private Sprite voiceButtonOnSprite = null;

    [SerializeField]
    private Sprite voiceButtonOffSelectSprite = null;

    [SerializeField]
    private Sprite voiceButtonOnSelectSprite = null;

    [SerializeField, Tooltip("設定アニメーター")]
    private Animator settingAnimator = null;

    static readonly int isShowId = Animator.StringToHash("isShow");
    static readonly int isHideId = Animator.StringToHash("isHide");

    [Header("音関係")]
    [SerializeField]
    private float seDecisionVol = 1.0f;
    [SerializeField]
    private AudioClip seDecisionClip = null;

    private DataScriptableObject dataScriptableObject;
    private float tempFloat = 0f;
    SpriteState spriteState;

    private void Start()
    {
        if (TitleRoot.Instance)
        {
            dataScriptableObject = TitleRoot.Instance.dataScriptableObject;
        }
        else
        {
            dataScriptableObject = MainGameRoot.Instance.dataScriptableObject;
        }

        voiceButtonImage = voiceButton.GetComponent<Image>();
        if (dataScriptableObject.isVoiceSetting)
        {
            voiceButtonImage.sprite = voiceButtonOnSprite;
            spriteState = voiceButton.spriteState;
            spriteState.selectedSprite = voiceButtonOnSelectSprite;
            voiceButton.spriteState = spriteState;
        }
        else
        {
            voiceButtonImage.sprite = voiceButtonOffSprite;
            spriteState = voiceButton.spriteState;
            spriteState.selectedSprite = voiceButtonOffSelectSprite;
            voiceButton.spriteState = spriteState;
        }
        seSlider.value = dataScriptableObject.seVolSetting;
        bgmSlider.value = dataScriptableObject.bgmVolSetting;
    }

    public void SeChange()
    {
        dataScriptableObject.seVolSetting = seSlider.value;
    }

    public void BgmChange()
    {
        tempFloat = AudioControl.Instance.GetBGMVol();
        if (tempFloat > 0f)
        {
            tempFloat /= dataScriptableObject.bgmVolSetting;
        }
        else if (bgmSlider.value > 0)
        {
            tempFloat = 1;
        }
        dataScriptableObject.bgmVolSetting = bgmSlider.value;
        tempFloat *= dataScriptableObject.bgmVolSetting;

        AudioControl.Instance.SetBGMVol(tempFloat);
    }

    public void ButtonPreview()
    {
        AudioControl.Instance.SetSEVol(seDecisionVol * dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seDecisionClip);
    }

    public void ButtonVoice()
    {
        if (dataScriptableObject.isVoiceSetting)
        {
            dataScriptableObject.isVoiceSetting = false;

            voiceButtonImage.sprite = voiceButtonOffSprite;
            spriteState = voiceButton.spriteState;
            spriteState.selectedSprite = voiceButtonOffSelectSprite;
            voiceButton.spriteState = spriteState;
        }
        else
        {
            dataScriptableObject.isVoiceSetting = true;

            voiceButtonImage.sprite = voiceButtonOnSprite;
            spriteState = voiceButton.spriteState;
            spriteState.selectedSprite = voiceButtonOnSelectSprite;
            voiceButton.spriteState = spriteState;
        }
    }

    public void ButtonReturn()
    {
        if (TitleRoot.Instance)
        {
            TitleRoot.Instance.SettingClose();
        }
        else
        {
            MainGameRoot.Instance.SettingClose();
        }
    }

    public void Show()
    {
        settingAnimator.SetTrigger(isShowId);
    }

    public void Hide()
    {
        settingAnimator.SetTrigger(isHideId);
    }
}
