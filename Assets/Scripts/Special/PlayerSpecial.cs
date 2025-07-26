using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSpecial : MonoBehaviour
{
    [Header("基本設定")]

    [SerializeField, Tooltip("プレイヤー")]
    protected Player player;

    protected bool isActive = false;

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Init()
    {

    }

    /// <summary>
    /// 開始
    /// </summary>
    public virtual void StartSpecial()
    {
        isActive = true;
    }

    /// <summary>
    /// 終了
    /// </summary>
    public virtual void EndSpecial()
    {
        isActive = false;
    }
}
