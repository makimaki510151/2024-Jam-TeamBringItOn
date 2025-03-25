using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneModeMenu : MonoBehaviour
{
    [SerializeField, Tooltip("見上げるボタン")]
    private List<GameObject> lookUpButtonObjects = new List<GameObject>();

    [SerializeField, Tooltip("頭の角度")]
    private List<float> lookUpAngles = new List<float>();

    [SerializeField, Tooltip("無属の精の頭")]
    private Transform headTransform = null;

    private Vector3 headAngle = Vector3.zero;

    public void ShowTimeAttackMenu()
    {
        ModeSelectRoot.Instance.ShowMenu(4);
    }

    /// <summary>
    /// 前のメニューに戻ります
    /// </summary>
    public void ReturnMenu()
    {
        ModeSelectRoot.Instance.ShowMenu(0);
    }

    void Update()
    {
        for(int i = 0; i < lookUpButtonObjects.Count; i++)
        {
            if (lookUpButtonObjects[i] == ModeSelectRoot.Instance.EventSystem.currentSelectedGameObject.gameObject)
            {
                headAngle = headTransform.eulerAngles;
                headAngle.z = lookUpAngles[i];
                headTransform.eulerAngles = headAngle;
                break;
            }
        }
    }
}
