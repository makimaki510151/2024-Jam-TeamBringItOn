using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OctopusInc : MonoBehaviour
{
    [SerializeField, Tooltip("�^�R�X�~�̃C���[�W")]
    private Image octopusIncImage = null;

    [SerializeField, Tooltip("�^�R�X�~����")]
    private float octopusIncTime = 0.75f;
    private float octopusIncTimer = 0;

    [SerializeField, Tooltip("�A�j���[�^�[")]
    private Animator myAnimator = null;
    [SerializeField, Tooltip("�X�v���b�V���A�j���[�V����")]
    private AnimationClip splashAnim = null;

    static readonly int isSplashId = Animator.StringToHash("isSplash");

    void Update()
    {
        if (octopusIncTimer > 0)
        {
            octopusIncImage.color = new Color(1, 1, 1, octopusIncTimer / octopusIncTime);

            octopusIncTimer -= Time.deltaTime;
            if (octopusIncTimer <= 0)
            {
                octopusIncImage.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void SplashInc()
    {
        StartCoroutine(OnSplashInc());
    }

    IEnumerator OnSplashInc()
    {
        myAnimator.SetTrigger(isSplashId);

        yield return new WaitForSeconds(splashAnim.length - 0.1f);

        octopusIncTimer = octopusIncTime;
    }
}
