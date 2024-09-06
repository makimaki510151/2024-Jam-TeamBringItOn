using System.Collections;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField, Tooltip("�\���A�j���[�V����")]
    private AnimationClip showAnim = null;

    void Start()
    {
        StartCoroutine(OnStart());
    }

    IEnumerator OnStart()
    {
        yield return new WaitForSeconds(showAnim.length);

        Destroy(gameObject);
    }
}
