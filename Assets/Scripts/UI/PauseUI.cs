using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField, Tooltip("ポーズアニメーター")]
    private Animator pauseAnimator = null;

    static readonly int isShowId = Animator.StringToHash("isShow");
    static readonly int isHideId = Animator.StringToHash("isHide");

    public void Show()
    {
        pauseAnimator.SetTrigger(isShowId);
    }

    public void Hide()
    {
        pauseAnimator.SetTrigger(isHideId);
    }
}
