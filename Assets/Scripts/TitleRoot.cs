using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRoot : RootParent
{
    public static TitleRoot Instance;
    public override void Awake() 
    {  
        base.Awake();
        Instance = this;
    }

    public void SettingClose()
    {

    }

}
