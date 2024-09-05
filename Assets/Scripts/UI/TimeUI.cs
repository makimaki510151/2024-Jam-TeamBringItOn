using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    [SerializeField, Tooltip("ナンバーUI")]
    private Sprite[] numbers = new Sprite[10];

    [SerializeField, Tooltip("表示する場所を指定")]
    private Image[] values = null;

    private int time;
    private int timeMin;

    void Update()
    {
        time = (int)(MainGameRoot.Instance.PlayTime * 100); // 現在の秒数を取得
        timeMin = 0;
        while (true)
        {
            if (time >= 60 * 100)
            {
                timeMin++;
                time -= 60 * 100;
            }
            else
            {
                break;
            }
        }
        for (int index = 0; index < 4; index++)
        {
            values[index].sprite = numbers[time % 10];
            time /= 10;
        }
        for (int index = 4; index < values.Length; index++)
        {
            values[index].sprite = numbers[timeMin % 10];
            timeMin /= 10;
        }
    }
}
