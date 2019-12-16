using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirConsoleController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rgby;
    SpriteRenderer spriteRenderer;

    bool isGrounded;

    bool movingRight;
    bool movingLeft;
    bool jump;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    private LayerMask m_WhatIsGround;

    [SerializeField]
    private float runSpeed = 20f;
    private float jumpForce = 20f;
    private float horizontalMove = 0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rgby = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        DontDestroyOnLoad(this);
    }

    public void ButtonInput(string input)
    {

        switch (input)
        {
            case "right":
                movingRight = true;
                movingLeft = false;
                break;
            case "left":
                movingLeft = true;
                movingRight = false;
                break;
            
            case "right-up":
                movingRight = false;
                movingLeft = false;
                jump = false;
                break;
            case "left-up":
                movingLeft = false;
                movingRight = false;
                jump = false;
                break;
            
            case "jump":
                jump = true;
                break;
            case "jump-up":
                jump = false;
                break;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        isGrounded = false;

        animator.SetFloat("Speed", horizontalMove);


        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
            }
        }


        if(movingRight && !movingLeft)
        {
            runSpeed = 8f;
            rgby.velocity = new Vector2(runSpeed, rgby.velocity.y);

            if (isGrounded) {
                horizontalMove = 10f;
                animator.SetBool("IsJumping", false);
            }

            spriteRenderer.flipX = false;
        }
        else if (movingLeft && !movingRight)
        {
            runSpeed = 8f;
            rgby.velocity = new Vector2(-runSpeed, rgby.velocity.y);

            if (isGrounded) {
                horizontalMove = 10f;
                animator.SetBool("IsJumping", false);
            }

            spriteRenderer.flipX = true;
        }
        else
        {
            if (isGrounded)
                animator.SetBool("IsJumping", false);

            rgby.velocity = new Vector2(0, rgby.velocity.y);
        }
        
        if(jump && isGrounded)
        {
            rgby.velocity = new Vector2(rgby.velocity.x, jumpForce);
            animator.SetBool("IsJumping", true);
            isGrounded = false;
        }

        if(!movingLeft && !movingRight)
        {
            horizontalMove = 0f;
            runSpeed = 0f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Moving Platform"))
            this.transform.parent = collision.transform;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Moving Platform"))
            this.transform.parent = null;
    }
}
