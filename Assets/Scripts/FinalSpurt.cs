using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSpurt : MonoBehaviour
{
    [Header("‰¹ŠÖŒW")]
    [SerializeField]
    private float bgmMainGameLastSpurtVol = 1.0f;
    [SerializeField]
    private AudioClip bgmMainGameLastSpurtClip = null;

    private bool isBgm = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&!isBgm)
        {
            isBgm = true;
            AudioControl.Instance.SetBGMVol(bgmMainGameLastSpurtVol * MainGameRoot.Instance.dataScriptableObject.bgmVolSetting);
            AudioControl.Instance.PlayBGM(bgmMainGameLastSpurtClip);
        }
    }
}
