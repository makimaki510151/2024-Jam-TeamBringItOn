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

    [NonSerialized]
    public Player.PlayCharacter character = Player.PlayCharacter.Water;

    private RectTransform myRectTransform;

    private Vector3 targetPos = Vector3.zero;
    private Vector3 stratPos = Vector3.zero;
    private bool myDie = false;
    private int myStockCount = 0;
    private float tempFloat = 0f;
    private GameObject tempObject;

    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();

        stratPos = myRectTransform.position;
        myDie = MainGameRoot.Instance.GetStockDie(character);
        targetPos = MainGameRoot.Instance.GetStockUIPos(character, this);
        myStockCount = MainGameRoot.Instance.GetStockCount(character);
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
        }
        else if (myDie)
        {
            Destroy(gameObject);
        }
    }

    public void StockShot(Vector3 vector3,GameObject parentObject)
    {
        vector3.z = 0;
        Instantiate(myEnemy, vector3, Quaternion.identity).transform.parent = parentObject.transform;
        Destroy(gameObject);
    }
}
