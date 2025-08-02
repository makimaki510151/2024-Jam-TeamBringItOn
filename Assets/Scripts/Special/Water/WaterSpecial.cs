using UnityEngine;

public class WaterSpecial : PlayerSpecial
{
    [Header("…‚Ì¸Ý’è")]

    [SerializeField, Tooltip("‹›—‹ƒvƒŒƒnƒu")]
    private GameObject torpedoPrefab;

    [SerializeField, Tooltip("”­ŽË”")]
    private int shotNum;

    [SerializeField, Tooltip("”­ŽËŽžŠÔ")]
    private float shotTime;

    [SerializeField, Tooltip("”­ŽËyŽ²")]
    private float shotPosY;

    private float shotInterval = 0;
    private float shotIntervalElapsed = 0;
    private float shotElapsed = 0;

    public override void Init()
    {
        base.Init();

        // ”­ŽËŠÔŠu‚ðŽæ“¾
        shotInterval = shotTime / (shotNum - 1);

        shotPosY = transform.position.y;
    }

    public override void SetSpecialAttackReadyUI(bool isActive)
    {
        MainGameRoot.Instance.SetSpecialAttackUI(player.character, 0, isActive);
    }

    public override void StartSpecial()
    {
        base.StartSpecial();
        MainGameRoot.Instance.ShowSpecialAnimator(player.character, 0);
    }

    public override void EndSpecial()
    {
        base.EndSpecial();
        shotElapsed = 0;
        shotIntervalElapsed = 0;
        MainGameRoot.Instance.EndSpecialAnimator(player.character, 0);
    }

    void Update()
    {
        if (isActive)
        {
            shotElapsed += Time.deltaTime;
            if(shotElapsed >= shotTime)
            {
                EndSpecial();
            }

            shotIntervalElapsed += Time.deltaTime;
            if(shotIntervalElapsed >= shotInterval)
            {
                shotIntervalElapsed = 0;
                var go = Instantiate(torpedoPrefab);
                var pos = go.transform.position;
                pos = transform.position;
                pos.x -= 2;
                pos.y = shotPosY + Random.Range(0, 3.0f);
                go.transform.position = pos;
                TorpedoController torped = go.GetComponent<TorpedoController>();
                torped.Init(player);
                torped.Shot();
            }
        }
    }
}
