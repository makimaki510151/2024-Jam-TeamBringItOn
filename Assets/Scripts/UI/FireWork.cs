using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWork : MonoBehaviour
{
    [SerializeField, Tooltip("アニメーター")]
    private Animator fireWorkAnimator = null;

    static readonly int isLaunchId = Animator.StringToHash("isLaunch");

    public void LaunchFireworks()
    {
        fireWorkAnimator.SetTrigger(isLaunchId);
    }
}
