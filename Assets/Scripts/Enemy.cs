using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject myUI = null;

    public enum EnemyAiType
    {
        Idle,
        Crab,
        Octopus,
        Amemusi,
        Bullet,
        MrFireWorks,
        Candra,
    }
    public EnemyAiType AiType { get => aiType; private set => aiType = value; }

    [SerializeField]
    private EnemyAiType aiType = EnemyAiType.Idle;

    [Header("カニ設定")]

    [SerializeField]
    private float crabMoveTime = 1;
    private float crabMoveTimer = 0;
    [SerializeField]
    private float crabSpeed = 5;
    [SerializeField]
    private int crabCoefficient = 1;

    [Header("タコ設定")]

    [SerializeField, Tooltip("Water側のタコスミ")]
    private OctopusInc waterOctopusInc = null;
    [SerializeField, Tooltip("Fire側のタコスミ")]
    private OctopusInc fireOctopusInc = null;

    [Header("カンシャクダマ設定")]

    [SerializeField, Tooltip("カンシャクダマの横移動速度")]
    private float mrFireWorksMoveSpeed = 2.0f;

    [SerializeField, Tooltip("カンシャクダマの横回転速度")]
    private float mrFireWorksRotateSpeed = 2.0f;

    [SerializeField, Tooltip("Water側の花火")]
    private FireWork waterFireWork = null;

    [SerializeField, Tooltip("Fire側の花火")]
    private FireWork fireFireWork = null;

    [Header("アメムシ設定")]

    [SerializeField, Tooltip("弾の発射間隔")]
    private float bulletDelayTime = 0.5f;
    private float bulletDelayTimer = 0;

    [SerializeField, Tooltip("弾")]
    private GameObject bulletPrefab = null;
    private Transform bulletTransform = null;

    [SerializeField, Tooltip("弾の移動速度")]
    private float bulletSpeed = 1.0f;
    [SerializeField, Tooltip("弾の寿命")]
    private float bulletLifeTime = 5f;

    [Header("キャンドラ設定")]

    [SerializeField, Tooltip("基本当たり判定サイズ")]
    private Vector2 basicColliderSize = Vector2.one;

    [SerializeField, Tooltip("基本当たり判定位置")]
    private Vector2 basicColliderOffset = Vector2.zero;

    [SerializeField, Tooltip("最大当たり判定サイズ")]
    private Vector2 maxColliderSize = Vector2.one;

    [SerializeField, Tooltip("最大当たり判定位置")]
    private Vector2 maxColliderOffset = Vector2.zero;

    [SerializeField, Tooltip("変更が終わるまでの時間")]
    private float changeSizeTime = 0.2f;

    [SerializeField, Tooltip("伸びるまでの時間")]
    private float changeTimeForExtend = 0.7f;

    [SerializeField, Tooltip("縮むまでの時間")]
    private float changeTimeForShrink = 0.5f;
    
    private float changeTimer = 0;
    private int changeState = 0;

    [SerializeField, Tooltip("伸縮させる頭の火")]
    private Transform headFireTransform = null;

    [SerializeField, Tooltip("基本頭の火サイズ")]
    private Vector2 basicHeadFireSize = Vector2.one;

    [SerializeField, Tooltip("基本頭の火位置")]
    private Vector2 basicHeadFirePosition = Vector2.zero;

    [SerializeField, Tooltip("最大頭の火サイズ")]
    private Vector2 maxHeadFireSize = Vector2.one;

    [SerializeField, Tooltip("最大頭の火位置")]
    private Vector2 maxHeadFirePosition = Vector2.zero;

    private Vector3 tempVector3 = new(0, 0, 0);
    private Vector3 Vector3_left = Vector3.left;
    private Vector2 Vector2_zero = Vector2.zero;
    private GameObject tempObject = null;
    private float deltaTime;
    private Transform playerWaterTransform;
    private Transform playerFireTransform;

    private Rigidbody2D myRigidbody2D = null;
    private Transform myTransform = null;
    private BoxCollider2D myCollider = null;

    private void Start()
    {
        myTransform = transform;
        switch (AiType)
        {
            case EnemyAiType.Idle:
                break;
            case EnemyAiType.Crab:
                crabMoveTimer = crabMoveTime;
                myRigidbody2D = GetComponent<Rigidbody2D>();
                break;
            case EnemyAiType.Octopus:
                break;
            case EnemyAiType.Amemusi:
                bulletDelayTimer = bulletDelayTime;
                playerWaterTransform = MainGameRoot.Instance.playerWaterRigidbody2D.transform;
                playerFireTransform = MainGameRoot.Instance.playerFireRigidbody2D.transform;
                break;
            case EnemyAiType.Bullet:
                myRigidbody2D = GetComponent<Rigidbody2D>();
                break;
            case EnemyAiType.MrFireWorks:
                myRigidbody2D = GetComponent<Rigidbody2D>();
                playerWaterTransform = MainGameRoot.Instance.playerWaterRigidbody2D.transform;
                playerFireTransform = MainGameRoot.Instance.playerFireRigidbody2D.transform;
                break;
            case EnemyAiType.Candra:
                myCollider = GetComponent<BoxCollider2D>();
                break;
        }
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;

        switch (AiType)
        {
            case EnemyAiType.Idle:
                break;
            case EnemyAiType.Crab:
                UpdateForCrab();
                break;
            case EnemyAiType.Octopus:
                break;
            case EnemyAiType.Amemusi:
                UpdateForAmemusi();
                break;
            case EnemyAiType.Bullet:
                UpdateForBullet();
                break;
            case EnemyAiType.MrFireWorks:
                UpdateForMrFireWorks();
                break;
            case EnemyAiType.Candra:
                UpdateForCandra();
                break;
        }
    }

    private void UpdateForCrab()
    {
        if (crabMoveTimer > 0)
        {
            crabMoveTimer -= deltaTime;
            myRigidbody2D.velocity = myTransform.right * crabCoefficient * crabSpeed;
            if (crabMoveTimer <= 0)
            {
                crabMoveTimer = crabMoveTime;
                crabCoefficient *= -1;
            }
        }
    }

    public void HitOctopus(Player.PlayCharacter playCharacter)
    {
        if (playCharacter == Player.PlayCharacter.Water)
        {
            waterOctopusInc.SplashInc();
        }
        else
        {
            fireOctopusInc.SplashInc();
        }
    }

    private void UpdateForAmemusi()
    {
        if ((myTransform.position.y - playerWaterTransform.position.y) <= 5 && (myTransform.position.x - playerWaterTransform.position.x) <= 30 ||
           (myTransform.position.y - playerFireTransform.position.y) <= 5 && (myTransform.position.x - playerFireTransform.position.x) <= 30)
        {
            if (bulletDelayTimer > 0)
            {
                bulletDelayTimer -= deltaTime;
                if (bulletDelayTimer <= 0)
                {
                    bulletTransform = Instantiate(bulletPrefab).transform;
                    bulletTransform.position = transform.position;
                    bulletDelayTimer = bulletDelayTime;
                }
            }
        }
    }

    private void UpdateForBullet()
    {
        myRigidbody2D.velocity = -myTransform.right * bulletSpeed;
        bulletLifeTime -= deltaTime;
        if (bulletLifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateForMrFireWorks()
    {
        if ((myTransform.position.y - playerWaterTransform.position.y) <= 5 && (myTransform.position.x - playerWaterTransform.position.x) <= 18 ||
            (myTransform.position.y - playerFireTransform.position.y) <= 5 && (myTransform.position.x - playerFireTransform.position.x) <= 18)
        {
            myRigidbody2D.velocity = Vector3_left * mrFireWorksMoveSpeed;
            myTransform.Rotate(0, 0, mrFireWorksRotateSpeed);
        }
        else
        {
            myRigidbody2D.velocity = Vector2_zero;
        }
    }

    public void HitMrFireWorks(Player.PlayCharacter playCharacter)
    {
        if (playCharacter == Player.PlayCharacter.Water)
        {
            waterFireWork.LaunchFireworks();
        }
        else
        {
            fireFireWork.LaunchFireworks();
        }
        Destroy(gameObject);
    }

    private void UpdateForCandra()
    {
        changeTimer += deltaTime;
        switch(changeState)
        {
            // 縮み
            case 0:
                // 指定した時間が経ったら、伸び始める
                if(changeTimer >= changeTimeForExtend)
                {
                    changeState++;
                    changeTimer = 0;
                }
                break;
            // 縮み→伸び
            case 1:
                myCollider.size = Vector2.Lerp(basicColliderSize, maxColliderSize, changeTimer / changeSizeTime);
                myCollider.offset = Vector2.Lerp(basicColliderOffset, maxColliderOffset, changeTimer / changeSizeTime);
                headFireTransform.localScale = Vector2.Lerp(basicHeadFireSize, maxHeadFireSize, changeTimer / changeSizeTime);
                headFireTransform.localPosition = Vector2.Lerp(basicHeadFirePosition, maxHeadFirePosition, changeTimer / changeSizeTime);
                // 指定した時間が経ったら、最大サイズに固定する
                if(changeTimer >= changeSizeTime)
                {
                    changeState++;
                    changeTimer = 0;
                    myCollider.size = maxColliderSize;
                }
                break;
            // 伸び
            case 2:
                // 指定した時間が経ったら、縮み始める
                if (changeTimer >= changeTimeForShrink)
                {
                    changeState++;
                    changeTimer = 0;
                }
                break;
            // 伸び→縮み
            case 3:
                myCollider.size = Vector2.Lerp(maxColliderSize, basicColliderSize, changeTimer / changeSizeTime);
                myCollider.offset = Vector2.Lerp(maxColliderOffset, basicColliderOffset, changeTimer / changeSizeTime);
                headFireTransform.localScale = Vector2.Lerp(maxHeadFireSize, basicHeadFireSize, changeTimer / changeSizeTime);
                headFireTransform.localPosition = Vector2.Lerp(maxHeadFirePosition, basicHeadFirePosition, changeTimer / changeSizeTime);
                // 指定した時間が経ったら、基本サイズに固定する
                if (changeTimer >= changeSizeTime)
                {
                    changeState = 0;
                    changeTimer = 0;
                    myCollider.size = basicColliderSize;
                }
                break;
        }
    }

    public void StockMove(Player.PlayCharacter playCharacter)
    {
        switch (playCharacter)
        {
            case Player.PlayCharacter.Water:
                tempVector3 = MainGameRoot.Instance.waterCamera.WorldToScreenPoint(myTransform.position);
                tempObject = Instantiate(myUI, tempVector3, Quaternion.identity);
                tempObject.GetComponent<StockUI>().character = playCharacter;
                tempObject.transform.SetParent(MainGameRoot.Instance.GetCanvas().GetComponent<RectTransform>());
                Destroy(gameObject);
                break;
            case Player.PlayCharacter.Fire:
                tempVector3 = MainGameRoot.Instance.fireCamera.WorldToScreenPoint(myTransform.position);
                tempObject = Instantiate(myUI, tempVector3, Quaternion.identity);
                tempObject.GetComponent<StockUI>().character = playCharacter;
                tempObject.transform.SetParent(MainGameRoot.Instance.GetCanvas().GetComponent<RectTransform>());
                Destroy(gameObject);
                break;
        }
    }
}
