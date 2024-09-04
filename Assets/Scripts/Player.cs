using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float defaultSpeed = 1.0f;
    [SerializeField]
    private float maxSpeed = 20f;
    
    [SerializeField]
    private float speedBuffItemCount = 0;
    [SerializeField]
    private float jumpPower = 1.0f;

    enum PlayCharacter
    {
        Water,
        Fire
    }

    [SerializeField]
    private PlayCharacter character = PlayCharacter.Water;


    private bool isJump = false;
    private bool isShot = false;

    private Vector2 tempVector2 = new(0, 0);

    private Rigidbody2D myRigidbody2D = null;
    private Transform myTransform = null;


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isJump = true;
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
    }

    void Update()
    {

        tempVector2 = (myTransform.right * defaultSpeed + (myTransform.right * defaultSpeed * (speedBuffItemCount / 100))) * Time.deltaTime;


        myRigidbody2D.velocity += tempVector2;

        if (isJump)
        {
            myRigidbody2D.AddForce(myTransform.up * jumpPower, ForceMode2D.Impulse);
            isJump = false;
        }

        tempVector2 = myRigidbody2D.velocity;
        if(tempVector2.x> maxSpeed)
        {
            tempVector2.x = maxSpeed;
            myRigidbody2D.velocity = tempVector2;
        }
    }
}
