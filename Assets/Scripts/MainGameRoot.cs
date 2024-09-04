using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameRoot : RootParent
{
    [SerializeField]
    private Transform cameraWater = null;
    [SerializeField]
    private Transform cameraFire = null;
    [SerializeField]
    private Rigidbody2D playerWaterRigidbody2D = null;
    [SerializeField]
    private Rigidbody2D playerFireRigidbody2D = null;
    [SerializeField]
    private float lerpNum = 0.9f;


    private Vector3 cameraPosWater = Vector2.zero;
    private Vector3 cameraPosFire = Vector2.zero;

    private Vector3 tempVector3 = Vector3.zero;

    public static MainGameRoot Instance;
    public override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    private void Start()
    {
        cameraPosWater = (Vector2)cameraWater.position - playerWaterRigidbody2D.position;
        cameraPosFire = (Vector2)cameraFire.position - playerFireRigidbody2D.position;
    }

    private void Update()
    {
        tempVector3 = Vector2.Lerp(cameraWater.position, playerWaterRigidbody2D.position + (Vector2)cameraPosWater, lerpNum);
        tempVector3.y = 7.75f;
        tempVector3.z = -10;
        cameraWater.position = tempVector3;

        tempVector3 = Vector2.Lerp(cameraFire.position, playerFireRigidbody2D.position + (Vector2)cameraPosFire, lerpNum);
        tempVector3.y = -12.25f;
        tempVector3.z = -10;
        cameraFire.position = tempVector3;

    }

    public void SettingClose()
    {

    }



}