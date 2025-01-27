using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickGround : MonoBehaviour
{
    public enum GimmickGroundType
    {
        Ice,
        Magma,
    }
    public GimmickGroundType GroundType { get => groundType; private set => groundType = value; }

    [SerializeField]
    private GimmickGroundType groundType = GimmickGroundType.Magma;

    [Header("•Xİ’è")]

    [SerializeField, Tooltip("•XŒ‹ŠÔ")]
    private float frozenTime = 1.5f;

    public float SetFrozenTime()
    {
        return frozenTime;
    }
}
