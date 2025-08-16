using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParryStockUI : MonoBehaviour
{
    [SerializeField, Tooltip("ナンバーUI")]
    private Sprite[] numbers = new Sprite[10];

    [SerializeField, Tooltip("表示する場所を指定")]
    private Image[] values = null;

    public void SetCount(int num)
    {
        for (int index = 0; index < values.Length; index++)
        {
            values[index].sprite = numbers[num % 10];
            num /= 10;
        }
    }
}
