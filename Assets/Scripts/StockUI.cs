using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockUI : MonoBehaviour
{
    [SerializeField]
    private float stockTime = 0.3f;
    private float stockTimer = 0;
    [SerializeField]
    private float rotateValue = 60;

    [SerializeField]
    private GameObject myEnemy = null;

    [Header("‰¹ŠÖŒW")]
    [SerializeField]
    private float seParryStockVol = 1.0f;
    [SerializeField]
    private AudioClip seParryStockClip = null;

    [SerializeField]
    private float seParryLauncherVol = 1.0f;
    [SerializeField]
    private AudioClip seParryLauncherClip = null;

    [NonSerialized]
    public Player.PlayCharacter character = Player.PlayCharacter.Water;

    private RectTransform myRectTransform;

    private Vector3 targetPos = Vector3.zero;
    private Vector3 stratPos = Vector3.zero;
    private bool myDie = false;
    private float tempFloat = 0f;

    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();

        stratPos = myRectTransform.position;
        myDie = MainGameRoot.Instance.GetStockDie(character);
        targetPos = MainGameRoot.Instance.GetStockUIPos(character, this);
    }

    // Update is called once per frame
    void Update()
    {
        if (stockTimer < stockTime)
        {
            myRectTransform.Rotate(0, 0, rotateValue);
            stockTimer += Time.deltaTime;
            tempFloat = stockTimer / stockTime;
            myRectTransform.position = Vector3.Lerp(stratPos, targetPos, tempFloat);
            if(stockTimer >= stockTime)
            {
                AudioControl.Instance.SetSEVol(seParryStockVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
                AudioControl.Instance.PlaySE(seParryStockClip);
            }
        }
        else if (myDie)
        {
            Destroy(gameObject);
        }
    }

    public void StockShot(Vector3 vector3,GameObject parentObject)
    {
        AudioControl.Instance.SetSEVol(seParryLauncherVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seParryLauncherClip);

        vector3.z = 0;
        Instantiate(myEnemy, vector3, Quaternion.identity).transform.parent = parentObject.transform;
        Destroy(gameObject);
    }
}
