using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DataScriptableObject", menuName = "DataScriptableObject", order = 0)]
public class DataScriptableObject : ScriptableObject
{
    public float seVolSetting = 0.5f;

    public float bgmVolSetting = 0.5f;

    public enum PlayType
    {
        Two,
        One
    }

    public PlayType playType = PlayType.Two;

    public float cameraRotation = 0;

}