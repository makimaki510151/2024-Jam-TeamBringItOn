using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootParent : MonoBehaviour
{
    public DataScriptableObject dataScriptableObject = null;

    public virtual void Awake()
    {
        //FPS�l
        const int FPS = 60;

        // fps��ݒ�ł���悤�ɂ��邽�߂̏���
        // 0 : fps�ݒ�ł���悤�ɂ���
        QualitySettings.vSyncCount = 0;

        //FPS�̌Œ�
        //�����ɂČŒ肷��FPS�́AUpdate�̊Ԋu���w�肷����̂ł���
        //60fps�Œ�
        Application.targetFrameRate = FPS;
    }
}
