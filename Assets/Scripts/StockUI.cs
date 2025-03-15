using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StockUI : MonoBehaviour
{
    [SerializeField]
    private float stockTime = 0.3f;
    private float stockTimer = 0;
    [SerializeField]
    private float rotateValue = 60;

    [SerializeField]
    private GameObject myEnemy = null;

    [SerializeField]
    private float yPosGap = 3.5f;

    [Header("パリィ砲設定")]

    [SerializeField, Tooltip("パリィ砲の移動時間")]
    private float parryShotTime = 0.1f;

    [SerializeField, Tooltip("パリィ砲の初速度")]
    private Vector2 parryShotInitialVelocity = Vector2.zero;

    [SerializeField, Tooltip("パリィ砲本体1P")]
    private GameObject parryShotOnePrefab = null;

    [SerializeField, Tooltip("パリィ砲本体2P")]
    private GameObject parryShotTwoPrefab = null;

    [SerializeField, Tooltip("パリィ砲エンド1P")]
    private GameObject parryShotEndOnePrefab = null;

    [SerializeField, Tooltip("パリィ砲エンド2P")]
    private GameObject parryShotEndTwoPrefab = null;

    [SerializeField, Tooltip("パリィ砲フラッシュ")]
    private GameObject parryShotFlashPrefab = null;

    [Header("音関係")]
    [SerializeField]
    private float seParryStockVol = 1.0f;
    [SerializeField]
    private AudioClip seParryStockClip = null;

    [SerializeField]
    private float seParryLauncherVol = 1.0f;
    [SerializeField]
    private AudioClip seParryLauncherClip = null;

    [NonSerialized]
    public Player.PlayCharacter character = Player.PlayCharacter.One;

    private RectTransform myRectTransform;
    private Image myImage;

    private Vector3 targetPos = Vector3.zero;
    private Vector3 stratPos = Vector3.zero;
    private bool myDie = false;
    private float tempFloat = 0f;
    private bool isShot = false;
    private Player.PlayCharacter playCharacter;
    private Transform canvasTransform;
    private RectTransform prefabRectTransform;
    private GameObject parryShotObject;
    private Transform enemyTransform;
    private Vector3 pos;
    private Vector3 acceleration;
    private Vector3 velocity;
    private float deltaTime;
    private Vector3 diff;
    private Vector3 Vector3_zero = Vector3.zero;

    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
        myImage = GetComponent<Image>();

        stratPos = myRectTransform.position;
        myDie = MainGameRoot.Instance.GetStockDie(character);
        targetPos = MainGameRoot.Instance.GetStockUIPos(character, this);
        canvasTransform = GameObject.Find("Canvas").transform;
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime = Time.deltaTime;

        if (stockTimer < stockTime)
        {
            myRectTransform.Rotate(0, 0, rotateValue);
            stockTimer += deltaTime;
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

        if (isShot)
        {
            // レーザー追尾
            // velocity 速度
            // period   時間
            // diff     距離
            // acceleration 加速度
            acceleration = Vector3_zero;

            var diff = targetPos - pos;
            acceleration += (diff - velocity * parryShotTime) * 2f
                             / (parryShotTime * parryShotTime);

            parryShotTime -= deltaTime;
            if(parryShotTime < 0f)
            {
                Destroy(parryShotObject);

                if (playCharacter == Player.PlayCharacter.One)
                {
                    prefabRectTransform = Instantiate(parryShotEndOnePrefab, targetPos, Quaternion.identity).GetComponent<RectTransform>();
                    prefabRectTransform.transform.SetParent(canvasTransform, false);
                }
                else
                {
                    prefabRectTransform = Instantiate(parryShotEndTwoPrefab, targetPos, Quaternion.identity).GetComponent<RectTransform>();
                    prefabRectTransform.transform.SetParent(canvasTransform, false);
                }
                prefabRectTransform.position = myRectTransform.position;
                prefabRectTransform = Instantiate(parryShotFlashPrefab, targetPos, Quaternion.identity).GetComponent<RectTransform>();
                prefabRectTransform.transform.SetParent(canvasTransform, false);
                prefabRectTransform.position = myRectTransform.position;

                if (playCharacter == Player.PlayCharacter.One)
                {
                    targetPos = MainGameRoot.Instance.cameraTwo.ScreenToWorldPoint(targetPos);
                }
                else
                {
                    targetPos = MainGameRoot.Instance.cameraOne.ScreenToWorldPoint(targetPos);
                }
                enemyTransform = Instantiate(myEnemy, targetPos, Quaternion.identity).transform;
                MainGameRoot.Instance.AddEnemyInEnemysTransform(enemyTransform, playCharacter);
                Destroy(gameObject);
            }

            velocity += acceleration * deltaTime;
            pos += velocity * deltaTime;
            myRectTransform.position = pos;
        }
    }

    public void StockShot(Vector3 vector3, Player.PlayCharacter character)
    {
        AudioControl.Instance.SetSEVol(seParryLauncherVol * MainGameRoot.Instance.dataScriptableObject.seVolSetting);
        AudioControl.Instance.PlaySE(seParryLauncherClip);

        vector3.z = 0;
        // カンシャクダマ以外なら、ランダムで縦位置を変更する
        if (myEnemy.GetComponent<Enemy>().AiType != Enemy.EnemyAiType.MrFireWorks) vector3.y += Random.Range(0, yPosGap);
        else vector3.y -= 0.3f;
        targetPos = vector3;
        isShot = true;
        stratPos = myRectTransform.position;
        myRectTransform.rotation = Quaternion.identity;
        myImage.enabled = false;

        playCharacter = character;
        if (playCharacter == Player.PlayCharacter.One)
        {
            targetPos = MainGameRoot.Instance.cameraTwo.WorldToScreenPoint(vector3);
            prefabRectTransform = Instantiate(parryShotOnePrefab, stratPos, Quaternion.identity).GetComponent<RectTransform>();
            prefabRectTransform.transform.SetParent(myRectTransform.transform, false);
        }
        else
        {
            targetPos = MainGameRoot.Instance.cameraOne.WorldToScreenPoint(vector3);
            prefabRectTransform = Instantiate(parryShotTwoPrefab, stratPos, Quaternion.identity).GetComponent<RectTransform>();
            prefabRectTransform.transform.SetParent(myRectTransform.transform, false);
        }
        parryShotObject = prefabRectTransform.gameObject;
        prefabRectTransform.position = myRectTransform.position;
        prefabRectTransform = Instantiate(parryShotFlashPrefab, stratPos, Quaternion.identity).GetComponent<RectTransform>();
        prefabRectTransform.transform.SetParent(canvasTransform, false);
        prefabRectTransform.position = myRectTransform.position;
        pos = myRectTransform.position;
        velocity = parryShotInitialVelocity;
    }
}
