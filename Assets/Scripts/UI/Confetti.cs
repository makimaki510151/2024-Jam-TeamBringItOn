using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    [SerializeField, Tooltip("アニメーター")]
    private Animator confettiAnimator = null;

    static readonly int isShowId = Animator.StringToHash("isShow");

    public void Show()
    {
        confettiAnimator.SetTrigger(isShowId);
    }
}
