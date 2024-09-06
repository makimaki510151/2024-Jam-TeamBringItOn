using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameRoot : RootParent
{
    [SerializeField]
    private float startTime = 3;
    [SerializeField, Tooltip("ナンバーUI")]
    private Sprite[] numbers = new Sprite[10];
    [SerializeField]
    private Image startCountImage = null;

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
    private RectTransform stockShotPosWater = null;
    [SerializeField]
    private RectTransform stockShotPosFire = null;
    [SerializeField]
    private GameObject stageWaterEnemysObject = null;
    [SerializeField]
    private GameObject stageFireEnemysObject = null;

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

    [SerializeField]
    private Transform waterGoalTransform = null;
    [SerializeField]
    private Transform fireGoalTransform = null;

    [SerializeField]
    private RectTransform waterIconRectTransform = null;
    [SerializeField]
    private RectTransform fireIconRectTransform = null;

    [SerializeField, Tooltip("ゴールしてからアニメーションを開始するまでの時間")]
    private float goalShowDelay = 1.0f;

    [SerializeField, Tooltip("ゴールアニメーター")]
    private Animator goalAnimator = null;

    static readonly int isShowId = Animator.StringToHash("isShow");
    static readonly int isGoalOnePId = Animator.StringToHash("isGoalOneP");

    private bool isResult = false;  // リザルトが表示されたらフラグをオン

    [SerializeField, Tooltip("タイムUI")]
    private TimeUI timeUI = null;

    private List<StockUI> stockUIsWater = new();
    private List<StockUI> stockUIsFire = new();

    public float PlayTime { get; private set; }

    private Vector3 cameraPosWater = Vector2.zero;
    private Vector3 cameraPosFire = Vector2.zero;
    private bool isCoroutines = false;
    private AsyncOperation asyncLoad;

    private float waterGoalRange = 1;
    private float fireGoalRange = 1;

    private float tempFloat = 1;
    private Vector3 tempVector3 = new(0, 0, 0);
    private float deltaTime;

    public static MainGameRoot Instance;

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started&& startTime<=0&&!isResult)
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

    public void OnResult(InputAction.CallbackContext context)
    {
        // リザルト中にボタンが押されたら、モードセレクトシーンへ移行する
        if (context.started && isResult)
        {
            ButtonModeSelect();
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

        cameraFireTransform.rotation = Quaternion.Euler(0, 0, dataScriptableObject.cameraRotation);

        PlayTime = 0;

        waterGoalRange = waterGoalTransform.position.x;
        fireGoalRange = fireGoalTransform.position.x;

        Time.timeScale = 0;
    }

    private void Update()
    {
        if (startTime > 0)
        {
            startTime -= Time.unscaledDeltaTime;
            Debug.Log(startTime);
            startCountImage.sprite = numbers[((int)startTime / 1)+1];
            if(startTime <= 0)
            {
                Time.timeScale = 1;
                startCountImage.gameObject.SetActive(false);
            }
        }

        deltaTime = Time.deltaTime;

        // タイム計測
        PlayTime += deltaTime;

        tempVector3 = Vector2.Lerp(cameraWaterTransform.position, playerWaterRigidbody2D.position + (Vector2)cameraPosWater, lerpNum);
        tempVector3.y = 7.75f;
        tempVector3.z = -10;
        cameraWaterTransform.position = tempVector3;

        tempVector3 = Vector2.Lerp(cameraFireTransform.position, playerFireRigidbody2D.position + (Vector2)cameraPosFire, lerpNum);
        tempVector3.y = -12.25f;
        tempVector3.z = -10;
        cameraFireTransform.position = tempVector3;

        tempFloat = playerWaterRigidbody2D.position.x / waterGoalRange;
        tempVector3 = new Vector3(1820 * tempFloat+50, 540, 0);
        waterIconRectTransform.position = tempVector3;

        tempFloat = playerFireRigidbody2D.position.x / fireGoalRange;
        tempVector3 = new Vector3(1820 * tempFloat + 50, 540, 0);
        fireIconRectTransform.position = tempVector3;
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

    public Vector3 GetStockUIPos(Player.PlayCharacter enemyCharacter, StockUI myStock)
    {
        switch (enemyCharacter)
        {
            case Player.PlayCharacter.Water:
                if (waterStockCount < 5)
                {
                    waterStockCount++;
                    stockUIsWater.Add(myStock);
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
                    stockUIsFire.Add(myStock);
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

    public void GoalPlayer(Player.PlayCharacter character)
    {
        // 1Pがゴールしたら、1PゴールUIを表示する
        if (character == Player.PlayCharacter.Water)
        {
            goalAnimator.SetBool(isGoalOnePId, true);
        }
        // 2Pがゴールしたら、2PゴールUIを表示する
        else
        {
            goalAnimator.SetBool(isGoalOnePId, false);
        }

        StartCoroutine(OnGoalPlayer());
    }

    IEnumerator OnGoalPlayer()
    {
        timeUI.SetResultTime();

        yield return new WaitForSeconds(goalShowDelay);

        goalAnimator.SetTrigger(isShowId);

        isResult = true;
    }

    public void StockEnemyShot(Player.PlayCharacter playCharacter)
    {
        switch (playCharacter)
        {
            case Player.PlayCharacter.Water:
                if (waterStockCount > 0)
                {
                    waterStockCount--;
                    tempVector3 = MainGameRoot.Instance.fireCamera.ScreenToWorldPoint(stockShotPosFire.position);
                    stockUIsWater[waterStockCount].StockShot(tempVector3, stageWaterEnemysObject);
                    stockUIsWater.Remove(stockUIsWater[waterStockCount]);

                }
                break;
            case Player.PlayCharacter.Fire:
            default:
                if (fireStockCount > 0)
                {
                    fireStockCount--;
                    tempVector3 = MainGameRoot.Instance.waterCamera.ScreenToWorldPoint(stockShotPosWater.position);
                    stockUIsFire[fireStockCount].StockShot(tempVector3, stageFireEnemysObject);
                    stockUIsFire.Remove(stockUIsFire[fireStockCount]);
                }
                break;
        }
    }
}