using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSetter : MonoBehaviour
{
    [SerializeField, Tooltip("キャラクター")]
    private List<GameObject> characters = new List<GameObject>();

    [SerializeField, Tooltip("ステージ")]
    private List<GameObject> stages = new List<GameObject>();

    [SerializeField, Tooltip("キャラクターの初期位置")]
    private Vector2 characterPosition = Vector2.zero;

    [SerializeField, Tooltip("ステージの初期位置")]
    private List<Vector2> stagePositions = new List<Vector2>();

    [SerializeField, Tooltip("NonPlayerControllerオブジェクト")]
    private GameObject nonPlayerControllerPrefab = null;

    private DataScriptableObject dataScriptableObject = null;
    private GameObject stageObject = null;
    private GameObject characterOneObject = null;
    private GameObject characterTwoObject = null;
    private BackGround backGround = null;
    private GameObject[] breadsObjects = new GameObject[2];
    private Transform[] enemysTransforms = new Transform[2];

    public void StageSetting()
    {
        dataScriptableObject = MainGameRoot.Instance.dataScriptableObject;

        // ステージ、キャラクターの設置
        stageObject = Instantiate(stages[dataScriptableObject.stageOneNumber]);
        stageObject.transform.position = stagePositions[0];
        characterOneObject = Instantiate(characters[dataScriptableObject.characterOneNumber]);
        characterOneObject.transform.parent = stageObject.transform;
        characterOneObject.transform.localPosition = characterPosition;
        characterOneObject.GetComponent<Player>().character = Player.PlayCharacter.One;
        foreach(Transform child in stageObject.transform)
        {
            backGround = child.GetComponent<BackGround>();
            if (backGround != null)
            {
                backGround.SetInformation(characterOneObject.transform, MainGameRoot.Instance.cameraOneTransform);
            }
        }
        MainGameRoot.Instance.goalTransformOne = stageObject.transform.Find("Goal").transform;
        breadsObjects[0] = stageObject.transform.Find("BreadsObject").gameObject;
        enemysTransforms[0] = stageObject.transform.Find("Enemys").transform;

        stageObject = Instantiate(stages[dataScriptableObject.stageTwoNumber]);
        stageObject.transform.position = stagePositions[1];
        characterTwoObject = Instantiate(characters[dataScriptableObject.characterTwoNumber]);
        characterTwoObject.transform.parent = stageObject.transform;
        characterTwoObject.transform.localPosition = characterPosition;
        characterTwoObject.GetComponent<Player>().character = Player.PlayCharacter.Two;
        foreach (Transform child in stageObject.transform)
        {
            backGround = child.GetComponent<BackGround>();
            if (backGround != null)
            {
                backGround.SetInformation(characterTwoObject.transform, MainGameRoot.Instance.cameraTwoTransform);
            }
        }
        MainGameRoot.Instance.goalTransformTwo = stageObject.transform.Find("Goal").transform;
        breadsObjects[1] = stageObject.transform.Find("BreadsObject").gameObject;
        enemysTransforms[1] = stageObject.transform.Find("Enemys").transform;

        if (dataScriptableObject.playerAmountNumber == 0)
        {
            Instantiate(nonPlayerControllerPrefab).transform.parent = characterTwoObject.transform;
        }
    }

    public GameObject GetCharacterOne()
    {
        return characterOneObject;
    }

    public GameObject GetCharacterTwo()
    {
        return characterTwoObject;
    }

    public GameObject[] GetBreadsObjects()
    {
        return breadsObjects;
    }

    public Transform[] GetEnemysTransforms()
    {
        return enemysTransforms;
    }
}
