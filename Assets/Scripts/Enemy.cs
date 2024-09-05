using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject myUI = null;

    enum EnemyAiType
    {
        Idle,
        Crab
    }
    [SerializeField]
    private EnemyAiType aiType = EnemyAiType.Idle;
    [SerializeField]
    private float crabMoveTime = 1;
    private float crabMoveTimer = 0;
    [SerializeField]
    private float crabSpeed = 5;
    [SerializeField]
    private int crabCoefficient = 1;



    private Vector3 tempVector3 = new(0,0,0);
    private GameObject tempObject = null;

    private Rigidbody2D myRigidbody2D = null;
    private Transform myTransform = null;



    private void Start()
    {
        myTransform = transform;
        switch (aiType)
        {
            case EnemyAiType.Idle:
                break;
            case EnemyAiType.Crab:
                crabMoveTimer = crabMoveTime;
                myRigidbody2D = GetComponent<Rigidbody2D>();
                break;
        }
    }

    private void Update()
    {
        switch (aiType)
        {
            case EnemyAiType.Idle:
                break;
            case EnemyAiType.Crab:
                if(crabMoveTimer >0)
                {
                    crabMoveTimer -= Time.deltaTime;
                    myRigidbody2D.velocity = myTransform.right * crabCoefficient * crabSpeed;
                    if (crabMoveTimer <= 0)
                    {
                        crabMoveTimer = crabMoveTime;
                        crabCoefficient *= -1;
                    }
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
