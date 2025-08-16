using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WholeMenu : MonoBehaviour
{
    [SerializeField, Tooltip("操作ガイド")]
    private Image guideImage = null;

    private GameObject guideObject = null;

    [SerializeField]
    private Sprite[] guideSprites = new Sprite[2];

    private int guideCount = 1;

    [SerializeField, Tooltip("操作ガイドを表示するボタン")]
    private Button gideButton = null;

    [SerializeField, Tooltip("操作ガイド表示時に選択するボタン")]
    private Button firstSelectGideButton = null;

    public void OnReturn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RetrunTitle();
        }
    }

    void Start()
    {
        guideObject = guideImage.gameObject;
        guideObject.SetActive(false);
    }

    /// <summary>
    /// タイトル画面に移行します
    /// </summary>
    public void RetrunTitle()
    {
        ModeSelectRoot.Instance.LoadScene(0);
    }

    /// <summary>
    /// 競争モード選択画面に移行します
    /// </summary>
    public void GoToCompetitiveMode()
    {
        ModeSelectRoot.Instance.ShowMenu(1);
    }

    /// <summary>
    /// 一人用モード選択画面に移行します
    /// </summary>
    public void GoToOneMode()
    {
        ModeSelectRoot.Instance.ShowMenu(3);
    }

    /// <summary>
    /// 操作ガイドを表示します
    /// </summary>
    public void ShowManual()
    {
        guideObject.SetActive(true);
        firstSelectGideButton.Select();
    }

    /// <summary>
    /// 操作ガイドを非表示にします
    /// </summary>
    public void HideManual()
    {
        guideObject.SetActive(false);
        gideButton.Select();
    }

    /// <summary>
    /// 操作ガイドの次のページに進みます
    /// </summary>
    public void NextManualPage()
    {
        guideImage.sprite = guideSprites[guideCount];
        guideCount++;
        if (guideCount == guideSprites.Length)
        {
            guideCount = 0;
        }
    }
}
