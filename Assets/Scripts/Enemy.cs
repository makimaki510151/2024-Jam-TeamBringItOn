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
    }
    public EnemyAiType AiType { get => aiType; private set => aiType = value; }

    [SerializeField]
    private EnemyAiType aiType = EnemyAiType.Idle;

    [SerializeField]
    private float crabMoveTime = 1;
    private float crabMoveTimer = 0;
    [SerializeField]
    private float crabSpeed = 5;
    [SerializeField]
    private int crabCoefficient = 1;

    [SerializeField, Tooltip("Water‘¤‚Ìƒ^ƒRƒXƒ~")]
    private OctopusInc waterOctopusInc = null;
    [SerializeField, Tooltip("Fire‘¤‚Ìƒ^ƒRƒXƒ~")]
    private OctopusInc fireOctopusInc = null;

    [SerializeField, Tooltip("’e‚Ì”­ŽËŠÔŠu")]
    private float bulletDelayTime = 0.5f;
    private float bulletDelayTimer = 0;

    [SerializeField, Tooltip("’e")]
    private GameObject bulletPrefab = null;
    private Transform bulletTransform = null;

    [SerializeField, Tooltip("’e‚ÌˆÚ“®‘¬“x")]
    private float bulletSpeed = 1.0f;
    [SerializeField, Tooltip("’e‚ÌŽõ–½")]
    private float bulletLifeTime = 5f;

    private Vector3 tempVector3 = new(0, 0, 0);
    private GameObject tempObject = null;
    private float deltaTime;
    private Transform playerWaterTransform;
    private Transform playerFireTransform;

    private Rigidbody2D myRigidbody2D = null;
    private Transform myTransform = null;

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
