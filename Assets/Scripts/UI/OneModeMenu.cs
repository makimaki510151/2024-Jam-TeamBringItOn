using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneModeMenu : MonoBehaviour
{
    public void ShowStoryMenu()
    {

    }

    public void ShowTimeAttackMenu()
    {
        ModeSelectRoot.Instance.ShowMenu(4);
    }

    /// <summary>
    /// ‘O‚Ìƒƒjƒ…[‚É–ß‚è‚Ü‚·
    /// </summary>
    public void ReturnMenu()
    {
        ModeSelectRoot.Instance.ShowMenu(0);
    }
}
