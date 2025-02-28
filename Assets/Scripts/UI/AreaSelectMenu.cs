using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSelectMenu : MonoBehaviour
{
    public void SelectArea(int areaNumber)
    {

    }

    /// <summary>
    /// ‘O‚Ìƒƒjƒ…[‚É–ß‚è‚Ü‚·
    /// </summary>
    public void ReturnMenu()
    {
        ModeSelectRoot.Instance.ShowMenu(3);
    }
}
