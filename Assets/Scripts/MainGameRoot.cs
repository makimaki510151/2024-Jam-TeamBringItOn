using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainGameRoot : RootParent
{
    [SerializeField]
    private Transform cameraWaterTransform = null;
    [SerializeField]
    private Transform cameraFireTransform = null;
    [SerializeField]
    private Rigidbody2D playerWaterRigidbody2D = null;
    [SerializeField]
    private Rigidbody2D playerFireRigidbody2D = null;
    [SerializeField]
    private float lerpNum = 0.9f;

    [SerializeField]
    private GameObject pauseUIObject = null;
    [SerializeField]
    private GameObject settingUIObject = null;

    [SerializeField]
    private List<RectTransform> waterStocks = new();
    [SerializeField]
    private RectTransform waterStockOver = null;
    [SerializeField]
    private List<RectTransform> fireStocks = new();
    [SerializeField]
    private RectTransform fireStockOver = null;

    [SerializeField]
    private int waterStockCount = 0;
    [SerializeField]
    private int fireStockCount = 0;

    [SerializeField]
    private GameObject canvasObject = null;

    [NonSerialized]
    public bool isPause = false;
    [NonSerialized]
    public bool isSetting = false;

    [NonSerialized]
    public Camera waterCamera = null;
    [NonSerialized]
    public Camera fireCamera = null;

    public float PlayTime { get; private set; }

    private Vector3 cameraPosWater = Vector2.zero;
    private Vector3 cameraPosFire = Vector2.zero;
    private bool isCoroutines = false;
    private AsyncOperation asyncLoad;

    private Vector3 tempVector3 = new(0,0,0);
    private float deltaTime;

    public static MainGameRoot Instance;

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!isSetting)
            {
                if (!isPause)
                {
                    Time.timeScale = 0.0f;
                    isPause = true;
                    pauseUIObject.SetActive(true);
                }
                else
                {
                    ButtonResume();
                }
            }
            else
            {
                settingUIObject.SetActive(false);
                SettingClose();
            }
        }
    }

    public override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    private void Start()
    {
        cameraPosWater = (Vector2)cameraWaterTransform.position - playerWaterRigidbody2D.position;
        cameraPosFire = (Vector2)cameraFireTransform.position - playerFireRigidbody2D.position;
        waterCamera = cameraWaterTransform.GetComponent<Camera>();
        fireCamera = cameraFireTransform.GetComponent<Camera>();

        PlayTime = 0;
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;

        // ƒ^ƒCƒ€Œv‘ª
        PlayTime += deltaTime;

        tempVector3 = Vector2.Lerp(cameraWaterTransform.position, playerWaterRigidbody2D.position + (Vector2)cameraPosWater, lerpNum);
        tempVector3.y = 7.75f;
        tempVector3.z = -10;
        cameraWaterTransform.position = tempVector3;

        tempVector3 = Vector2.Lerp(cameraFireTransform.position, playerFireRigidbody2D.position + (Vector2)cameraPosFire, lerpNum);
        tempVector3.y = -12.25f;
        tempVector3.z = -10;
        cameraFireTransform.position = tempVector3;
    }

    public void ButtonResume()
    {
        Time.timeScale = 1.0f;
        isPause = false;
        pauseUIObject.SetActive(false);
    }
    public void ButtonSetting()
    {
        settingUIObject.SetActive(true);
        pauseUIObject.SetActive(false);
        isSetting = true;
    }
    public void SettingClose()
    {
        isSetting = false;
        pauseUIObject.SetActive(true);
    }
    public void ButtonTitle()
    {
        Time.timeScale = 1.0f;
        if (isCoroutines) return;
        isCoroutines = true;
        StartCoroutine(LoadYourAsyncScene("Title"));
    }
    public void ButtonModeSelect()
    {
        Time.timeScale = 1.0f;
        if (isCoroutines) return;
        isCoroutines = true;
        StartCoroutine(LoadYourAsyncScene("ModeSelect"));
    }
    IEnumerator LoadYourAsyncScene(string name)
    {
        asyncLoad = SceneManager.LoadSceneAsync(name);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public Vector3 GetStockUIPos(Player.PlayCharacter enemyCharacter)
    {
        switch (enemyCharacter)
        {
            case Player.PlayCharacter.Water:
                if (waterStockCount < 5)
                {
                    waterStockCount++;
                    return waterStocks[waterStockCount - 1].position;
                }
                else
                {
                    return waterStockOver.position;
                }
            case Player.PlayCharacter.Fire:
            default:
                if (fireStockCount < 5)
                {
                    fireStockCount++;
                    return fireStocks[fireStockCount - 1].position;
                }
                else
                {
                    return fireStockOver.position;
                }
        }
    }
    public bool GetStockDie(Player.PlayCharacter enemyCharacter)
    {
        switch (enemyCharacter)
        {
            case Player.PlayCharacter.Water:
                if (waterStockCount < 5)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            case Player.PlayCharacter.Fire:
            default:
                if (fireStockCount < 5)
                {
                    return false;
                }
                else
                {
                    return true;
                }
        }
    }

    public int GetStockCount(Player.PlayCharacter enemyCharacter)
    {
        switch (enemyCharacter)
        {
            case Player.PlayCharacter.Water:
                return waterStockCount;
            case Player.PlayCharacter.Fire:
            default:
                return fireStockCount;
        }
    }
    public GameObject GetCanvas()
    {
        return canvasObject;
    }
}