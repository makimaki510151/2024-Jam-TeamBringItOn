using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameRoot : RootParent
{
    [SerializeField]
    private EventSystem eventSystem = null;
    [SerializeField]
    private float startTime = 3;
    [SerializeField, Tooltip("ナンバーUI")]
    private Sprite[] numbers = new Sprite[10];
    [SerializeField]
    private Image startCountImage = null;

    public Transform cameraOneTransform = null;
    public Transform cameraTwoTransform = null;

    public Rigidbody2D playerOneRigidbody2D = null;
    public Rigidbody2D playerTwoRigidbody2D = null;

    // プレイヤースクリプト
    private Player playerOne;
    private Player playerTwo;

    [SerializeField]
    private float lerpNum = 0.9f;

    [SerializeField]
    private GameObject pauseUIObject = null;
    [SerializeField, Tooltip("最初に選択するボタン")]
    private Button firstSelectButton = null;

    [SerializeField]
    private GameObject settingUIObject = null;
    [SerializeField, Tooltip("オプションで選択するボタン")]
    private Button settingSelectButton = null;

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
    //[SerializeField]
    //private GameObject stageWaterEnemysObject = null;
    //[SerializeField]
    //private GameObject stageFireEnemysObject = null;

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

    public Transform waterGoalTransform = null;
    public Transform fireGoalTransform = null;

    [SerializeField]
    private RectTransform waterIconRectTransform = null;
    [SerializeField]
    private RectTransform fireIconRectTransform = null;

    [SerializeField, Tooltip("ゴールしてからアニメーションを開始するまでの時間")]
    private float goalShowDelay = 1.0f;

    [SerializeField, Tooltip("アニメーションしてからモードセレクトに戻るまでの時間")]
    private float goModeSelectDelay = 4.0f;

    [SerializeField, Tooltip("ゴールアニメーター")]
    private Animator goalAnimator = null;

    static readonly int isShowId = Animator.StringToHash("isShow");
    static readonly int isGoalOnePId = Animator.StringToHash("isGoalOneP");

    private bool isResult = false;  // リザルトが表示されたらフラグをオン
    private bool isPlayerOne = false;

    [SerializeField, Tooltip("タイムUI")]
    private TimeUI timeUI = null;

    [Header("パン食い競争設定")]

    [SerializeField, Tooltip("パン食い競争スクリプト")]
    private BreadEatingCompetitionRoot breadEatingCompetitionRoot = null;

    [SerializeField, Tooltip("パン食い競争モード")]
    private bool isBreadEatingCompetitionMode = false;

    [SerializeField, Tooltip("片方がゴールしてからの制限時間")]
    private float breadEatingCompetitionTimeLimit = 20.0f;

    [SerializeField, Tooltip("待機UI")]
    private List<GameObject> waitUIs = new List<GameObject>();
    private List<Animator> waitUIAnimators = new List<Animator>();

    static readonly int isShowIdForWaitUI = Animator.StringToHash("isShow");

    [SerializeField, Tooltip("結果UI")]
    private GameObject resultUI = null;

    private int goalCount = 0;

    [Header("音関係")]
    [SerializeField]
    private float bgmMainGameVol = 1.0f;
    [SerializeField]
    private AudioClip bgmMainGameClip = null;

    [SerializeField]
    private float bgmTimeAttackVol = 1.0f;
    [SerializeField]
    private AudioClip bgmTimeAttackClip = null;

    [SerializeField]
    private float seDecisionVol = 1.0f;
    [SerializeField]
    private AudioClip seDecisionClip = null;


    private List<StockUI> stockUIsWater = new();
    private List<StockUI> stockUIsFire = new();

    public float PlayTime { get; private set; }

    private Vector3 cameraPosWater = Vector2.zero;
    private Vector3 cameraPosFire = Vector2.zero;
    private bool isCoroutines = false;
    private AsyncOperation asyncLoad;
    private GameObject selectEndButtonObject;

    private float waterGoalRange = 1;
    private float fireGoalRange = 1;

    private float tempFloat = 1;
    private Vector3 tempVector3 = new(0, 0, 0);
    private bool isWaterHigherRank = false;
    private float deltaTime;

    private bool isWaitGoal = false;
    private StageSetter stageSetter = null;

    public static MainGameRoot Instance;

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started && startTime <= 0 && !isResult)
        {
            if (!isSetting)
            {
                if (!isPause)
                {
                    Time.timeScale = 0.0f;
                    isPause = true;
                    pauseUIObject.SetActive(true);
                    firstSelectButton.Select();
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

        stageSetter = GetComponent<StageSetter>();
        stageSetter.StageSetting();
        playerOneRigidbody2D = stageSetter.GetCharacterOne().GetComponent<Rigidbody2D>();
        playerTwoRigidbody2D = stageSetter.GetCharacterTwo().GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        cameraPosWater = (Vector2)cameraOneTransform.position - playerOneRigidbody2D.position;
        cameraPosFire = (Vector2)cameraTwoTransform.position - playerTwoRigidbody2D.position;
        waterCamera = cameraOneTransform.GetComponent<Camera>();
        fireCamera = cameraTwoTransform.GetComponent<Camera>();
        playerOne = playerOneRigidbody2D.gameObject.GetComponent<Player>();
        playerTwo = playerTwoRigidbody2D.gameObject.GetComponent<Player>();
        for(int i = 0; i < waitUIs.Count; i++)
        {
            waitUIAnimators.Add(waitUIs[i].GetComponent<Animator>());
            waitUIs[i].SetActive(false);
        }

        cameraTwoTransform.rotation = Quaternion.Euler(0, 0, dataScriptableObject.cameraRotation);

        PlayTime = 0;

        waterGoalRange = waterGoalTransform.position.x;
        fireGoalRange = fireGoalTransform.position.x;

        Time.timeScale = 0;

        if (dataScriptableObject.playType == DataScriptableObject.PlayType.Two)
        {
            AudioControl.Instance.SetBGMVol(bgmMainGameVol * dataScriptableObject.bgmVolSetting);
            AudioControl.Instance.PlayBGM(bgmMainGameClip);
        }
        else
        {
            AudioControl.Instance.SetBGMVol(bgmTimeAttackVol * dataScriptableObject.bgmVolSetting);
            AudioControl.Instance.PlayBGM(bgmTimeAttackClip);
        }
        if (dataScriptableObject.playType == DataScriptableObject.PlayType.One)
        {
            playerTwoRigidbody2D.gameObject.SetActive(false);
            waterStockCount = 5;
            isPlayerOne = true;
            fireIconRectTransform.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (startTime > 0)
        {
            startTime -= Time.unscaledDeltaTime;
            startCountImage.sprite = numbers[((int)startTime / 1) + 1];
            if (startTime <= 0)
            {
                Time.timeScale = 1;
                startCountImage.gameObject.SetActive(false);
            }
        }

        deltaTime = Time.deltaTime;

        // タイム計測
        PlayTime += deltaTime;

        tempVector3 = Vector2.Lerp(cameraOneTransform.position, playerOneRigidbody2D.position + (Vector2)cameraPosWater, lerpNum);
        tempVector3.y = 7.75f;
        tempVector3.z = -10;
        cameraOneTransform.position = tempVector3;

        tempVector3 = Vector2.Lerp(cameraTwoTransform.position, playerTwoRigidbody2D.position + (Vector2)cameraPosFire, lerpNum);
        tempVector3.y = -12.25f;
        tempVector3.z = -10;
        cameraTwoTransform.position = tempVector3;

        tempFloat = playerOneRigidbody2D.position.x / waterGoalRange;
        tempVector3 = new Vector3(1820 * tempFloat + 50, 540, 0);
        var posW = waterIconRectTransform.position;
        posW.x = tempVector3.x;
        waterIconRectTransform.position = posW;

        tempFloat = playerTwoRigidbody2D.position.x / fireGoalRange;
        tempVector3 = new Vector3(1820 * tempFloat + 50, 540, 0);
        var posF = fireIconRectTransform.position;
        posF.x = tempVector3.x;
        fireIconRectTransform.position = posF;

        JudgeRank();

        if (isPause)
        {
            if (selectEndButtonObject != eventSystem.currentSelectedGameObject && eventSystem.currentSelectedGameObject != null)
            {
                selectEndButtonObject = eventSystem.currentSelectedGameObject;
            }

            if (eventSystem.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(selectEndButtonObject);
            }
        }

        if (isWaitGoal)
        {
            breadEatingCompetitionTimeLimit -= deltaTime;
            if(breadEatingCompetitionTimeLimit <= 0)
            {
                isWaitGoal = false;
                BreadGoal();
            }
        }
    }

    /// <summary>
    /// 順位に応じてプレイヤーのパリィ受付時間を変更します
    /// </summary>
    private void JudgeRank()
    {
        tempFloat = playerOneRigidbody2D.position.x - playerTwoRigidbody2D.position.x;
        // 水の精霊が順位が上でフラグが立っていなければ、パリィ時間を変更する
        if(tempFloat > 0 && !isWaterHigherRank)
        {
            isWaterHigherRank = true;
            playerOne.SetParryTime(true);
            playerTwo.SetParryTime(false);
        }
        // 火の精霊が順位が上でフラグが立っていれば、パリィ時間を変更する
        else if (tempFloat < 0 && isWaterHigherRank)
        {
            isWaterHigherRank = false;
            playerOne.SetParryTime(false);
            playerTwo.SetParryTime(true);
        }
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
        settingSelectButton.Select();
    }
    public void SettingClose()
    {
        isSetting = false;
        pauseUIObject.SetActive(true);
        firstSelectButton.Select();
    }
    public void ButtonTitle()
    {
        if (isCoroutines) return;
        isCoroutines = true;
        Time.timeScale = 1.0f;

        AudioControl.Instance.SetSEVol(seDecisionVol * dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seDecisionClip);

        StartCoroutine(LoadYourAsyncScene("Title"));
    }
    public void ButtonModeSelect()
    {
        if (isCoroutines) return;
        isCoroutines = true;
        Time.timeScale = 1.0f;

        AudioControl.Instance.SetSEVol(seDecisionVol * dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seDecisionClip);

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
            case Player.PlayCharacter.One:
                if (waterStockCount < 5)
                {
                    waterStockCount++;
                    stockUIsWater.Add(myStock);
                    return waterStocks[waterStockCount - 1].position;
                }
                else if (isPlayerOne)
                {
                    return waterStockOver.position;
                }
                else
                {
                    return waterStockOver.position;
                    //StockEnemyShot(enemyCharacter);
                    //waterStockCount++;
                    //stockUIsWater.Add(myStock);
                    //return waterStocks[waterStockCount - 1].position;
                }
            case Player.PlayCharacter.Two:
            default:
                if (fireStockCount < 5)
                {
                    fireStockCount++;
                    stockUIsFire.Add(myStock);
                    return fireStocks[fireStockCount - 1].position;
                }
                else if (isPlayerOne)
                {
                    return fireStockOver.position;
                }
                else
                {
                    return fireStockOver.position;
                    //StockEnemyShot(enemyCharacter);
                    //fireStockCount++;
                    //stockUIsFire.Add(myStock);
                    //return fireStocks[fireStockCount - 1].position;
                }
        }
    }
    public bool GetStockDie(Player.PlayCharacter enemyCharacter)
    {
        switch (enemyCharacter)
        {
            case Player.PlayCharacter.One:
                if (waterStockCount < 5)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            case Player.PlayCharacter.Two:
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
            case Player.PlayCharacter.One:
                return waterStockCount;
            case Player.PlayCharacter.Two:
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
        // デュアルランナーモード
        if (!isBreadEatingCompetitionMode)
        {
            // 1Pがゴールしたら、1PゴールUIを表示する
            if (character == Player.PlayCharacter.One)
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
        // パン食い競争モード
        else
        {
            // 1Pがゴールしたら、1PゴールUIを表示する
            if (character == Player.PlayCharacter.One)
            {
                waitUIs[0].SetActive(true);
                waitUIAnimators[0].SetTrigger(isShowIdForWaitUI);
                breadEatingCompetitionRoot.SetTimeOver(0);
            }
            // 2Pがゴールしたら、2PゴールUIを表示する
            else
            {
                waitUIs[1].SetActive(true);
                waitUIAnimators[1].SetTrigger(isShowIdForWaitUI);
                breadEatingCompetitionRoot.SetTimeOver(1);
            }
            goalCount++;
            if(goalCount >= 2 && isWaitGoal)
            {
                BreadGoal();
            }
            else
            {
                isWaitGoal = true;
            }
        }
    }

    IEnumerator OnGoalPlayer()
    {
        timeUI.SetResultTime();

        yield return new WaitForSeconds(goalShowDelay);

        goalAnimator.SetTrigger(isShowId);

        yield return new WaitForSeconds(goModeSelectDelay);

        ButtonModeSelect();
    }

    private void BreadGoal()
    {
        breadEatingCompetitionRoot.SetResult();
        StartCoroutine(OnBreadResult());
    }

    IEnumerator OnBreadResult()
    {
        waitUIs[0].SetActive(false);
        waitUIs[1].SetActive(false);
        resultUI.SetActive(true);

        yield return new WaitForSeconds(1);

        if (breadEatingCompetitionRoot.CheckWinner())
        {
            goalAnimator.SetBool(isGoalOnePId, true);
        }
        else
        {
            goalAnimator.SetBool(isGoalOnePId, false);
        }

        StartCoroutine(OnGoalPlayer());
    }

    public void StockEnemyShot(Player.PlayCharacter playCharacter)
    {
        if (isPlayerOne)
        {
            return;
        }

        switch (playCharacter)
        {
            case Player.PlayCharacter.One:
                if (waterStockCount > 0)
                {
                    waterStockCount--;
                    tempVector3 = MainGameRoot.Instance.fireCamera.ScreenToWorldPoint(stockShotPosFire.position);
                    stockUIsWater[waterStockCount].StockShot(tempVector3);
                    stockUIsWater.Remove(stockUIsWater[waterStockCount]);

                }
                break;
            case Player.PlayCharacter.Two:
            default:
                if (fireStockCount > 0)
                {
                    fireStockCount--;
                    tempVector3 = MainGameRoot.Instance.waterCamera.ScreenToWorldPoint(stockShotPosWater.position);
                    Debug.Log(tempVector3);
                    stockUIsFire[fireStockCount].StockShot(tempVector3);
                    stockUIsFire.Remove(stockUIsFire[fireStockCount]);
                }
                break;
        }
    }
}