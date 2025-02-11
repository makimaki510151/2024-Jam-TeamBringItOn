using System;
using UnityEngine;


[CreateAssetMenu(fileName = "DataScriptableObject", menuName = "DataScriptableObject", order = 0)]
public class DataScriptableObject : ScriptableObject
{
    [SerializeField, Tooltip("SEのボリューム")]
    private float SeVolSetting = default;

    [NonSerialized]
    public float seVolSetting;

    [SerializeField, Tooltip("BGMのボリューム")]
    private float BgmVolSetting = default;

    [NonSerialized]
    public float bgmVolSetting;

    public enum PlayType
    {
        Two,
        One
    }

    [NonSerialized]
    public PlayType playType = PlayType.Two;

    [NonSerialized]
    public float cameraRotation = 0;

    [NonSerialized]
    public bool isBreadMode = false;

    [NonSerialized]
    public bool isTwoPlayer = false;

    [NonSerialized]
    public int playerAmountNumber = 0;

    [NonSerialized]
    public int operationNumber = 0;

    [NonSerialized]
    public int characterOneNumber = 0;

    [NonSerialized]
    public int characterTwoNumber = 0;

    [NonSerialized]
    public int stageOneNumber = 0;

    [NonSerialized]
    public int stageTwoNumber = 0;

    public void OnAfterDeserialize()
    {
        // Editor上では再生中に変更したScriptableObject内の値が実行終了時に消えない。
        // そのため、初期値と実行時に使う変数は分けておき、初期化する必要がある。
        seVolSetting = SeVolSetting;
        bgmVolSetting = BgmVolSetting;
    }

    public void OnBeforeSerialize() { /* do nothing */ }
}