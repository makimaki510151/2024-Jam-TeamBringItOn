using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootParent : MonoBehaviour
{
    public DataScriptableObject dataScriptableObject = null;

    public virtual void Awake()
    {
        //FPS値
        const int FPS = 60;

        // fpsを設定できるようにするための処理
        // 0 : fps設定できるようにする
        QualitySettings.vSyncCount = 0;

        //FPSの固定
        //ここにて固定するFPSは、Updateの間隔を指定するものである
        //60fps固定
        Application.targetFrameRate = FPS;
    }
}
