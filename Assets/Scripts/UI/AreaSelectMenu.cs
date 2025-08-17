using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AreaSelectMenu : MonoBehaviour
{
    private DataScriptableObject dataScriptableObject;

    public void OnReturn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ReturnMenu();
        }
    }

    void Start()
    {
        dataScriptableObject = ModeSelectRoot.Instance.dataScriptableObject;
    }

    public void SelectArea(int areaNumber)
    {
        dataScriptableObject.characterOneNumber = dataScriptableObject.stageOneNumber = areaNumber;
        ModeSelectRoot.Instance.ButtonPlayOne();
    }

    /// <summary>
    /// ëOÇÃÉÅÉjÉÖÅ[Ç…ñﬂÇËÇ‹Ç∑
    /// </summary>
    public void ReturnMenu()
    {
        ModeSelectRoot.Instance.ShowMenu(3);
    }
}
