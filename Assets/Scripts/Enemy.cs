using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject myUI = null;

    private Vector3 tempVector3 = new(0,0,0);
    private GameObject tempObject = null;

    private Transform myTransform = null;

    private void Start()
    {
        myTransform = transform;
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
