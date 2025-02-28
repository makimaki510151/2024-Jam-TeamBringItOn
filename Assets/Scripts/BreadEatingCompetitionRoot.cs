using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreadEatingCompetitionRoot : MonoBehaviour
{
    [SerializeField, Tooltip("小さいパンのスコア")]
    private int smallBreadScore = 1;

    private int bigBreadScore = 0;

    [SerializeField, Tooltip("ゴールに着かなかった時の減点数")]
    private int notReachedGoalDeductionScore = 10;

    [SerializeField, Tooltip("ゴールできたかを表示")]
    private List<GameObject> reachedGoalUIs = new List<GameObject>();

    [SerializeField, Tooltip("ナンバーUI")]
    private Sprite[] numbers = new Sprite[10];

    [SerializeField, Tooltip("1Pの小さいパンのスコア")]
    private Image[] valuesSmallBreadOne = null;

    [SerializeField, Tooltip("2Pの小さいパンのスコア")]
    private Image[] valuesSmallBreadTwo = null;

    [SerializeField, Tooltip("1Pの大きいパンのスコア")]
    private Image[] valuesBigBreadOne = null;

    [SerializeField, Tooltip("2Pの大きいパンのスコア")]
    private Image[] valuesBigBreadTwo = null;

    [SerializeField, Tooltip("1Pの最終スコア")]
    private Image[] valuesScoreOne = null;

    [SerializeField, Tooltip("2Pの最終スコア")]
    private Image[] valuesScoreTwo = null;

    private int[] smallBreadCounts = new int[2];
    private int[] bigBreadCounts = new int[2];
    private bool[] timeOvers = new bool[2];
    private int[] playerScores = new int[2];

    private int score = 0;

    public static BreadEatingCompetitionRoot Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for(int i = 0; i < 2; i++)
        {
            playerScores[i] = 0;
            smallBreadCounts[i] = 0;
            bigBreadCounts[i] = 0;
            timeOvers[i] = true;
        }
        bigBreadScore = smallBreadScore + smallBreadScore;
    }

    public void CountScore(int player)
    {
        playerScores[player] += smallBreadScore;
        smallBreadCounts[player]++;
    }

    public void DoubleScore(int player)
    {
        playerScores[player] += bigBreadScore;
        bigBreadCounts[player]++;
    }

    public void SetTimeOver(int player)
    {
        timeOvers[player] = false;
    }

    public void SetResult()
    {
        for (int index = 0; index < valuesSmallBreadOne.Length; index++)
        {
            valuesSmallBreadOne[index].sprite = numbers[smallBreadCounts[0] % 10];
            smallBreadCounts[0] /= 10;
        }
        for (int index = 0; index < valuesSmallBreadTwo.Length; index++)
        {
            valuesSmallBreadTwo[index].sprite = numbers[smallBreadCounts[1] % 10];
            smallBreadCounts[1] /= 10;
        }
        for (int index = 0; index < valuesBigBreadOne.Length; index++)
        {
            valuesBigBreadOne[index].sprite = numbers[bigBreadCounts[0] % 10];
            bigBreadCounts[0] /= 10;
        }
        for (int index = 0; index < valuesBigBreadTwo.Length; index++)
        {
            valuesBigBreadTwo[index].sprite = numbers[bigBreadCounts[1] % 10];
            bigBreadCounts[1] /= 10;
        }
        if (timeOvers[0])
        {
            reachedGoalUIs[0].SetActive(false);
            reachedGoalUIs[1].SetActive(true);
            playerScores[0] -= notReachedGoalDeductionScore;
        }
        else
        {
            reachedGoalUIs[0].SetActive(true);
            reachedGoalUIs[1].SetActive(false);
        }
        if (timeOvers[1])
        {
            reachedGoalUIs[2].SetActive(false);
            reachedGoalUIs[3].SetActive(true);
            playerScores[1] -= notReachedGoalDeductionScore;
        }
        else
        {
            reachedGoalUIs[2].SetActive(true);
            reachedGoalUIs[3].SetActive(false);
        }

        if (playerScores[0] < 0)
        {
            playerScores[0] = 0;
        }
        else if (playerScores[1] < 0)
        {
            playerScores[1] = 0;
        }

        score = playerScores[0];
        for (int index = 0; index < valuesScoreOne.Length; index++)
        {
            valuesScoreOne[index].sprite = numbers[score % 10];
            score /= 10;
        }
        score = playerScores[1];
        for (int index = 0; index < valuesScoreTwo.Length; index++)
        {
            valuesScoreTwo[index].sprite = numbers[score % 10];
            score /= 10;
        }
    }

    public int CheckWinner()
    {
        if (playerScores[0] > playerScores[1])
        {
            return 0;
        }
        else if(playerScores[0] < playerScores[1])
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
}
