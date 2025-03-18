using UnityEngine;

public class ComingSoonUI : MonoBehaviour
{
    [SerializeField, Tooltip("自身のアニメーター")]
    private Animator comingSoonUIAnimator = null;

    static readonly int isShowId = Animator.StringToHash("isShow");

    public void Show()
    {
        comingSoonUIAnimator.SetTrigger(isShowId);
    }
}
