using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BreadEatingCompetitionRoot : MonoBehaviour
{
    [SerializeField, Tooltip("小さいパンのスコア")]
    private int smallBreadScore = 1;

    private int bigBreadScore = 0;

    [SerializeField, Tooltip("ゴールに着かなかった時の減点数")]
    private int notReachedGoalDeductionScore = 10;

    [SerializeField, Tooltip("小さいパンの取得数を表示")]
    private List<TMP_Text> smallBreadCountTexts = new List<TMP_Text>();

    [SerializeField, Tooltip("大きいパンの取得数を表示")]
    private List<TMP_Text> bigBreadCountTexts = new List<TMP_Text>();

    [SerializeField, Tooltip("ゴールできたかを表示")]
    private List<GameObject> reachedGoalUIs = new List<GameObject>();

    [SerializeField, Tooltip("スコアを表示")]
    private List<TMP_Text> scoreTexts = new List<TMP_Text>();

    private int[] smallBreadCounts = new int[2];
    private int[] bigBreadCounts = new int[2];
    private bool[] timeOvers = new bool[2];
    private int[] playerScores = new int[2];

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
        smallBreadCountTexts[0].text = smallBreadCounts[0].ToString();
        smallBreadCountTexts[1].text = smallBreadCounts[1].ToString();
        bigBreadCountTexts[0].text = bigBreadCounts[0].ToString();
        bigBreadCountTexts[1].text = bigBreadCounts[1].ToString();
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
        scoreTexts[0].text = playerScores[0].ToString();
        scoreTexts[1].text = playerScores[1].ToString();
    }

    public bool CheckWinner()
    {
        if (playerScores[0] > playerScores[1])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
