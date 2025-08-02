using UnityEngine;

public abstract class PlayerSpecial : MonoBehaviour
{
    [Header("基本設定")]

    [SerializeField, Tooltip("プレイヤー")]
    protected Player player;

    [SerializeField]
    private float seActivationVol = 1.0f;
    [SerializeField]
    private AudioClip seActivationClip = null;

    protected bool isActive = false;

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Init()
    {
        SetSpecialAttackReadyUI(false);
    }

    public virtual void SetSpecialAttackReadyUI(bool isActive)
    {
        //MainGameRoot.Instance.SetSpecialAttackUI(player.character, 0, isActive);
    }

    /// <summary>
    /// 開始
    /// </summary>
    public virtual void StartSpecial()
    {
        isActive = true;
        AudioControl.Instance.SetSEVol(seActivationVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seActivationClip, transform);
        SetSpecialAttackReadyUI(true);
    }

    /// <summary>
    /// 終了
    /// </summary>
    public virtual void EndSpecial()
    {
        isActive = false;
        SetSpecialAttackReadyUI(false);
    }
}
