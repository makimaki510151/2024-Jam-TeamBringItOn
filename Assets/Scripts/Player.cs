using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float defaultSpeed = 10.0f;
    [SerializeField]
    private float knockbackLeft = 10.0f;
    [SerializeField]
    private float knockbackUp = 5.0f;
    [SerializeField]
    private float invincibleTime = 0.5f;
    private float invincibleTimer = 0;
    [SerializeField]
    private float maxSpeed = 20f;

    [SerializeField]
    private float speedBuffItemCount = 0;
    [SerializeField]
    private float normalJumpPower = 8.0f;
    [SerializeField]
    private float parryJumpPower = 5.0f;

    public enum PlayCharacter
    {
        Water,
        Fire
    }

    [SerializeField]
    private PlayCharacter character = PlayCharacter.Water;

    [SerializeField]
    private float parryTime = 0.25f;
    private float parryTimer = 0;

    [SerializeField]
    private Color transparent = new(1, 1, 1, 0.25f);


    private bool isJump = false;
    private bool isParry = false;
    private bool isParryHit = false;
    private bool isParryCancel = false;
    private bool isShot = false;
    private bool isGround = false;
    private bool isTransparent = false;

    
    private Vector2 tempVector2 = new(0, 0);

    private Rigidbody2D myRigidbody2D = null;
    private Transform myTransform = null;
    private SpriteRenderer mySpriteRenderer;


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isGround)
            {
                isJump = true;
            }
            else
            {
                if (!isParry && !isParryCancel)
                {
                    Debug.Log("èÄîı");
                    isParry = true;
                    parryTimer = parryTime;
                    gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isShot = true;
        }
    }

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myTransform = transform;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        tempVector2 = (myTransform.right * defaultSpeed + (myTransform.right * defaultSpeed * (speedBuffItemCount / 100))) * Time.deltaTime;


        myRigidbody2D.velocity += tempVector2;

        if (isJump)
        {
            myRigidbody2D.AddForce(myTransform.up * normalJumpPower, ForceMode2D.Impulse);
            isJump = false;
        }

        tempVector2 = myRigidbody2D.velocity;
        if (tempVector2.x > maxSpeed)
        {
            tempVector2.x = maxSpeed;
            myRigidbody2D.velocity = tempVector2;
        }
        if (isParry)
        {
            parryTimer -= Time.deltaTime;
            if (parryTimer < 0)
            {
                isParry = false;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                if (!isGround && !isParryHit)
                {
                    isParryCancel = true;
                }
            }
        }
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
            isTransparent = !isTransparent;
            if (isTransparent)
            {
                mySpriteRenderer.color = transparent;
            }
            else
            {
                mySpriteRenderer.color = Color.white;
            }
            if (invincibleTimer < 0)
            {
                mySpriteRenderer.color = Color.white;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGround = true;
            isParryCancel = false;
            isParryHit = false;
        }
        else if (collision.CompareTag("Enemy"))
        {
            if (isParry)
            {
                Debug.Log("ÉpÉäÉBÅI");
                isParryHit = true;
                collision.GetComponent<Enemy>().StockMove(character);
                myRigidbody2D.AddForce(myTransform.up * parryJumpPower, ForceMode2D.Impulse);
            }
            else if (invincibleTimer <= 0)
            {
                myRigidbody2D.AddForce(-myTransform.right * knockbackLeft, ForceMode2D.Impulse);
                myRigidbody2D.AddForce(myTransform.up * knockbackUp, ForceMode2D.Impulse);
                invincibleTimer = invincibleTime;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
}
