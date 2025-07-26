using UnityEngine;

public class TorpedoController : MonoBehaviour
{
    [Header("‹›—‹İ’è")]

    [SerializeField, Tooltip("ˆÚ“®‘¬“x")]
    private float torpedSpeed = 1.0f;

    Player player;
    Rigidbody2D myRigidbody2D;
    Transform myTransform;
    SpriteRenderer mySpriteRenderer;
    bool isShot = false;

    public void Init(Player player)
    {
        this.player = player;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myTransform = transform;
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Shot()
    {
        isShot = true;
    }

    void Update()
    {
        if(isShot)
        {
            if(!mySpriteRenderer.isVisible)
            {
                isShot = false;
                Destroy(gameObject);
            }

            // ˆÚ“®
            myRigidbody2D.velocity = myTransform.right * torpedSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // “G‚Éƒqƒbƒg
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().DirectShot(player.character);
            Destroy(gameObject);
        }
    }
}
