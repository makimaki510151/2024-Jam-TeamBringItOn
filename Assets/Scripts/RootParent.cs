using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RootParent : MonoBehaviour
{
    public DataScriptableObject dataScriptableObject = null;

    public virtual void Awake()
    {
        //FPS’l
        const int FPS = 60;

        // fps‚ğİ’è‚Å‚«‚é‚æ‚¤‚É‚·‚é‚½‚ß‚Ìˆ—
        // 0 : fpsİ’è‚Å‚«‚é‚æ‚¤‚É‚·‚é
        QualitySettings.vSyncCount = 0;

        //FPS‚ÌŒÅ’è
        //‚±‚±‚É‚ÄŒÅ’è‚·‚éFPS‚ÍAUpdate‚ÌŠÔŠu‚ğw’è‚·‚é‚à‚Ì‚Å‚ ‚é
        //60fpsŒÅ’è
        Application.targetFrameRate = FPS;
    }
}
