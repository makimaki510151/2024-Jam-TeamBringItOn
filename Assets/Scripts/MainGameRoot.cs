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
    private PauseUI pauseUI = null;

    private GameObject pauseUIObject = null;

    [SerializeField, Tooltip("最初に選択するボタン")]
    private Button firstSelectButton = null;

    [SerializeField]
    private GameObject settingUIObject = null;
    [SerializeField, Tooltip("オプションで選択するボタン")]
    private Button settingSelectButton = null;

    [SerializeField]
    private List<RectTransform> oneStocks = new();
    [SerializeField]
    private RectTransform oneStockOver = null;
    [SerializeField]
    private List<RectTransform> twoStocks = new();
    [SerializeField]
    private RectTransform twoStockOver = null;

    [SerializeField]
    private RectTransform stockShotPosWater = null;
    [SerializeField]
    private RectTransform stockShotPosFire = null;
    //[SerializeField]
    //private GameObject stageWaterEnemysObject = null;
    //[SerializeField]
    //private GameObject stageFireEnemysObject = null;

    private int oneStockCount = 0;
    public int twoStockCount { get; private set; }

    [SerializeField]
    private GameObject canvasObject = null;

    [NonSerialized]
    public bool isPause = false;
    [NonSerialized]
    public bool isSetting = false;

    [NonSerialized]
    public Camera cameraOne = null;
    [NonSerialized]
    public Camera cameraTwo = null;

    public Transform goalTransformOne = null;
    public Transform goalTransformTwo = null;

    [SerializeField]
    private RectTransform iconOneRectTransform = null;
    [SerializeField]
    private RectTransform iconTwoRectTransform = null;

    [SerializeField, Tooltip("アイコン画像")]
    private List<Sprite> iconSprites = new List<Sprite>();

    [SerializeField, Tooltip("ゴールしてからアニメーションを開始するまでの時間")]
    private float goalShowDelay = 1.0f;

    [SerializeField, Tooltip("アニメーションしてからモードセレクトに戻るまでの時間")]
    private float goModeSelectDelay = 4.0f;

    [SerializeField, Tooltip("ゴールアニメーター")]
    private Animator goalAnimator = null;

    static readonly int isShowId = Animator.StringToHash("isShow");
    static readonly int isWaterId = Animator.StringToHash("isWater");
    static readonly int isFireId = Animator.StringToHash("isFire");
    static readonly int isAttributeNessId = Animator.StringToHash("isAttributeNess");
    static readonly int isDrawId = Animator.StringToHash("isDraw");

    [SerializeField, Tooltip("紙吹雪")]
    private List<Confetti> confettis = new List<Confetti>();

    private Confetti confetti;
    private Transform confettiTransform;
    
    public bool isResult { get; private set; }  // リザルトが表示されたらフラグをオン
    private bool isPlayerOne = false;

    [SerializeField, Tooltip("タイムUI")]
    private TimeUI timeUI = null;

    [SerializeField, Tooltip("敵をまとめるオブジェクト")]
    private Transform[] enemysTransforms = new Transform[2];

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

    [SerializeField]
    private float bgmWinnerVol = 1.0f;
    [SerializeField]
    private AudioClip bgmWinnerClip = null;

    [SerializeField]
    private float seGoalVol = 1.0f;
    [SerializeField]
    private AudioClip seGoalClip = null;

    private List<StockUI> stockUIsWater = new();
    private List<StockUI> stockUIsFire = new();

    public float PlayTime { get; private set; }

    private Vector3 cameraPosWater = Vector2.zero;
    private Vector3 cameraPosFire = Vector2.zero;
    private bool isCoroutines = false;
    private AsyncOperation asyncLoad;
    private GameObject selectEndButtonObject;

    private float goalRangeOne = 1;
    private float goalRangeTwo = 1;

    private float tempFloat = 1;
    private Vector3 tempVector3 = new(0, 0, 0);
    private bool isWaterHigherRank = false;
    private float deltaTime;

    private bool isWaitGoal = false;
    private StageSetter stageSetter = null;
    private int winBread = 0;
    private Image iconImage = null;
    private Transform goalUITransform = null;
    private GameObject[] breadsObjects = new GameObject[2];
    private bool[] isGoals = new bool[2];

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
                    pauseUI.Show();
                    pauseUIObject.transform.SetAsLastSibling();
                    settingUIObject.transform.SetAsLastSibling();
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
        isResult = false;

        // パン食い競争モードがオンなら、パンを表示する
        breadsObjects = stageSetter.GetBreadsObjects();
        if (dataScriptableObject.isBreadMode)
        {
            isBreadEatingCompetitionMode = true;
            for(int i = 0; i < breadsObjects.Length; i++)
            {
                breadsObjects[i].gameObject.SetActive(true);
            }
            confetti = confettis[1];
        }
        else
        {
            isBreadEatingCompetitionMode = false;
            for (int i = 0; i < breadsObjects.Length; i++)
            {
                breadsObjects[i].gameObject.SetActive(false);
            }
            confetti = confettis[0];
        }
        enemysTransforms = stageSetter.GetEnemysTransforms();
        goalUITransform = goalAnimator.gameObject.transform;
        confettiTransform = confetti.gameObject.transform;
        twoStockCount = 0;

        isGoals[0] = false;
        isGoals[1] = false;
        cameraPosWater = (Vector2)cameraOneTransform.position - playerOneRigidbody2D.position;
        cameraPosFire = (Vector2)cameraTwoTransform.position - playerTwoRigidbody2D.position;
        cameraOne = cameraOneTransform.GetComponent<Camera>();
        cameraTwo = cameraTwoTransform.GetComponent<Camera>();
        playerOne = playerOneRigidbody2D.gameObject.GetComponent<Player>();
        playerTwo = playerTwoRigidbody2D.gameObject.GetComponent<Player>();
        pauseUIObject = pauseUI.gameObject;
        for(int i = 0; i < waitUIs.Count; i++)
        {
            waitUIAnimators.Add(waitUIs[i].GetComponent<Animator>());
            waitUIs[i].SetActive(false);
        }

        cameraTwoTransform.rotation = Quaternion.Euler(0, 0, dataScriptableObject.cameraRotation);

        PlayTime = 0;

        goalRangeOne = goalTransformOne.position.x;
        goalRangeTwo = goalTransformTwo.position.x;

        // アイコン画像を変更
        iconImage = iconOneRectTransform.gameObject.GetComponent<Image>();
        iconImage.sprite = iconSprites[dataScriptableObject.characterOneNumber];
        iconImage = iconTwoRectTransform.gameObject.GetComponent<Image>();
        iconImage.sprite = iconSprites[dataScriptableObject.characterTwoNumber];

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
            oneStockCount = 5;
            isPlayerOne = true;
            iconOneRectTransform.gameObject.SetActive(false);
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

        tempFloat = playerOneRigidbody2D.position.x / goalRangeOne;
        tempVector3 = new Vector3(1820 * tempFloat + 50, 540, 0);
        var posW = iconOneRectTransform.position;
        posW.x = tempVector3.x;
        iconOneRectTransform.position = posW;

        tempFloat = playerTwoRigidbody2D.position.x / goalRangeTwo;
        tempVector3 = new Vector3(1820 * tempFloat + 50, 540, 0);
        var posF = iconTwoRectTransform.position;
        posF.x = tempVector3.x;
        iconTwoRectTransform.position = posF;

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

        // リザルト中はゴール画像を一番前に出し、敵を消去する
        if (isResult)
        {
            goalUITransform.SetAsLastSibling();
            confettiTransform.SetAsLastSibling();
            for(int i = 0; i < enemysTransforms.Length; i++)
            {
                foreach(Transform child in enemysTransforms[i])
                {
                    Destroy(child.gameObject);
                }
            }
        }
        // 各自がゴールしたとき、敵を消去する
        else
        {
            if (isGoals[0])
            {
                foreach (Transform child in enemysTransforms[0])
                {
                    Destroy(child.gameObject);
                }
            }
            if (isGoals[1])
            {
                foreach (Transform child in enemysTransforms[1])
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// 順位に応じてプレイヤーのパリィ受付時間を変更します
    /// </summary>
    private void JudgeRank()
    {
        tempFloat = playerOneRigidbody2D.position.x - playerTwoRigidbody2D.position.x;
        // 1Pが順位が上でフラグが立っていなければ、パリィ時間を変更する
        if(tempFloat > 0 && !isWaterHigherRank)
        {
            isWaterHigherRank = true;
            playerOne.SetParryTime(true);
            playerTwo.SetParryTime(false);
        }
        // 2Pが順位が上でフラグが立っていれば、パリィ時間を変更する
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
        pauseUI.Hide();
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
                if (oneStockCount < 5)
                {
                    oneStockCount++;
                    stockUIsWater.Add(myStock);
                    return oneStocks[oneStockCount - 1].position;
                }
                else if (isPlayerOne)
                {
                    return oneStockOver.position;
                }
                else
                {
                    return oneStockOver.position;
                    //StockEnemyShot(enemyCharacter);
                    //waterStockCount++;
                    //stockUIsWater.Add(myStock);
                    //return waterStocks[waterStockCount - 1].position;
                }
            case Player.PlayCharacter.Two:
            default:
                if (twoStockCount < 5)
                {
                    twoStockCount++;
                    stockUIsFire.Add(myStock);
                    return twoStocks[twoStockCount - 1].position;
                }
                else if (isPlayerOne)
                {
                    return twoStockOver.position;
                }
                else
                {
                    return twoStockOver.position;
                    //StockEnemyShot(enemyCharacter);
                    //twoStockCount++;
                    //stockUIsFire.Add(myStock);
                    //return fireStocks[twoStockCount - 1].position;
                }
        }
    }
    public bool GetStockDie(Player.PlayCharacter enemyCharacter)
    {
        switch (enemyCharacter)
        {
            case Player.PlayCharacter.One:
                if (oneStockCount < 5)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            case Player.PlayCharacter.Two:
            default:
                if (twoStockCount < 5)
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
                return oneStockCount;
            case Player.PlayCharacter.Two:
            default:
                return twoStockCount;
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
                switch(dataScriptableObject.characterOneNumber)
                {
                    case 0:
                        goalAnimator.SetTrigger(isWaterId);
                        break;
                    case 1:
                        goalAnimator.SetTrigger(isFireId);
                        break;
                    case 2:
                        goalAnimator.SetTrigger(isAttributeNessId);
                        break;
                }
                isGoals[0] = true;
            }
            // 2Pがゴールしたら、2PゴールUIを表示する
            else
            {
                switch (dataScriptableObject.characterTwoNumber)
                {
                    case 0:
                        goalAnimator.SetTrigger(isWaterId);
                        break;
                    case 1:
                        goalAnimator.SetTrigger(isFireId);
                        break;
                    case 2:
                        goalAnimator.SetTrigger(isAttributeNessId);
                        break;
                }
                isGoals[1] = true;
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
                isGoals[0] = true;
            }
            // 2Pがゴールしたら、2PゴールUIを表示する
            else
            {
                waitUIs[1].SetActive(true);
                waitUIAnimators[1].SetTrigger(isShowIdForWaitUI);
                breadEatingCompetitionRoot.SetTimeOver(1);
                isGoals[1] = true;
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

        if (!isResult)
        {
            AudioControl.Instance.SetSEVol(seGoalVol * dataScriptableObject.seVolSetting);
            AudioControl.Instance.PlaySE(seGoalClip);
        }

        yield return new WaitForSeconds(goalShowDelay);

        isResult = true;

        AudioControl.Instance.SetBGMVol(bgmWinnerVol * dataScriptableObject.bgmVolSetting);
        AudioControl.Instance.PlayBGM(bgmWinnerClip);

        goalAnimator.SetTrigger(isShowId);
        confetti.Show();

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

        winBread = breadEatingCompetitionRoot.CheckWinner();
        if (winBread == 0)
        {
            switch (dataScriptableObject.characterOneNumber)
            {
                case 0:
                    goalAnimator.SetTrigger(isWaterId);
                    break;
                case 1:
                    goalAnimator.SetTrigger(isFireId);
                    break;
                case 2:
                    goalAnimator.SetTrigger(isAttributeNessId);
                    break;
            }
        }
        else if(winBread == 1)
        {
            switch (dataScriptableObject.characterTwoNumber)
            {
                case 0:
                    goalAnimator.SetTrigger(isWaterId);
                    break;
                case 1:
                    goalAnimator.SetTrigger(isFireId);
                    break;
                case 2:
                    goalAnimator.SetTrigger(isAttributeNessId);
                    break;
            }
        }
        else
        {
            goalAnimator.SetTrigger(isDrawId);
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
                if (oneStockCount > 0)
                {
                    oneStockCount--;
                    tempVector3 = cameraTwo.ScreenToWorldPoint(stockShotPosFire.position);
                    stockUIsWater[oneStockCount].StockShot(tempVector3, playCharacter);
                    stockUIsWater.Remove(stockUIsWater[oneStockCount]);
                }
                break;
            case Player.PlayCharacter.Two:
            default:
                if (twoStockCount > 0)
                {
                    twoStockCount--;
                    tempVector3 = cameraOne.ScreenToWorldPoint(stockShotPosWater.position);
                    stockUIsFire[twoStockCount].StockShot(tempVector3, playCharacter);
                    stockUIsFire.Remove(stockUIsFire[twoStockCount]);
                }
                break;
        }
    }

    public void AddEnemyInEnemysTransform(Transform enemy,  Player.PlayCharacter character)
    {
        if(character == Player.PlayCharacter.One)
        {
            enemy.SetParent(enemysTransforms[1]);
        }
        else
        {
            enemy.SetParent(enemysTransforms[0]);
        }
    }
}