﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    public float friction;
    public float runForce;
    public float jumpForce;
    public float maxRunSpeed = 20f;
    public bool hasSword;
    public bool facingRight;

    public RuntimeAnimatorController animStanding;
    public RuntimeAnimatorController animStandingWithSword;
    public RuntimeAnimatorController animRunning;
    public RuntimeAnimatorController animRunningWithSword;
    public RuntimeAnimatorController animJumping;
    public RuntimeAnimatorController animJumpingWithSword;
    public RuntimeAnimatorController animSwingingSword;
    public RuntimeAnimatorController animPunching;

    public Animator anim;

    private Rigidbody2D rb2d;
    private Collider2D c2d;
    private bool grounded;
    private bool running;
    private bool attacking;
    private bool jump = false;
    private GameObject movingPlatform;
    private GameObject SwordSwing;



    // Use this for initialization

    void Animate()
    {
        if (hasSword)
        {
            if (grounded == true && running == false)
            {
                this.GetComponent<Animator>().runtimeAnimatorController = animStandingWithSword as RuntimeAnimatorController;

            }
            else if (grounded == true && running == true)
            {
                this.GetComponent<Animator>().runtimeAnimatorController = animRunningWithSword as RuntimeAnimatorController;

            }
            else if (jump == true || grounded == false)
            {
                this.GetComponent<Animator>().runtimeAnimatorController = animJumpingWithSword as RuntimeAnimatorController;

            }
            else if (attacking == true)
            {
                this.GetComponent<Animator>().runtimeAnimatorController = animSwingingSword as RuntimeAnimatorController;

            }
        }
        else
        {
            if (grounded == true && running == false)
            {
                this.GetComponent<Animator>().runtimeAnimatorController = animStanding as RuntimeAnimatorController;

            }
            else if (grounded == true && running == true)
            {
                this.GetComponent<Animator>().runtimeAnimatorController = animRunning as RuntimeAnimatorController;

            }
            else if (jump == true || grounded == false)
            {
                this.GetComponent<Animator>().runtimeAnimatorController = animJumping as RuntimeAnimatorController;

            }
            else if (attacking == true)
            {
                this.GetComponent<Animator>().runtimeAnimatorController = animPunching as RuntimeAnimatorController;

            }
        }


    }

    void Movement()
    {
        //IF USER IS PRESSING ARROW KEYS
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
        {
            running = true;
            Animate();
            Vector2 velocity = rb2d.velocity;
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveHorizontal, 0f);
            float currentspeed = moveHorizontal * rb2d.velocity.x;
            if (currentspeed < maxRunSpeed)
            {
                rb2d.AddForce(movement * runForce);
            }
        }
        else if (grounded == true && !Input.GetKey(KeyCode.Space))
        {
            rb2d.velocity = friction * rb2d.velocity;
        }

        running = false;
    }

    void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(new Vector2(0f, jumpForce));
        jump = false;


        if (hasSword)
        {
            this.GetComponent<Animator>().runtimeAnimatorController = animJumpingWithSword as RuntimeAnimatorController;

        }
        else
        {
            this.GetComponent<Animator>().runtimeAnimatorController = animJumping as RuntimeAnimatorController;

        }

    }

    void CollectSword()
    {
        hasSword = true;
        this.GetComponent<Animator>().runtimeAnimatorController = animStandingWithSword as RuntimeAnimatorController;
    }

    void Attack()
    {
        attacking = true;
        if (hasSword)
        {
            SwordSwing.SetActive(true);
        }
    }

    void Start()
    {

        animStanding 
        rb2d = GetComponent<Rigidbody2D>();
        c2d = GetComponent<PolygonCollider2D>();
    }


    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            jump = true;
        }
    }



    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BasicPlatform"))
        {
            grounded = true;
        }
        else if (other.gameObject.CompareTag("DisPlatform"))
        {
            grounded = true;
        }
        else if (other.gameObject.CompareTag("FallingPlatform"))
        {
            grounded = true;
        }
        else if (other.gameObject.CompareTag("MovingPlatform"))
        {
            grounded = true;
        }
        else if (other.gameObject.CompareTag("Lava"))
        {
            Time.timeScale = 0;
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Time.timeScale = 0;
        }else if (other.gameObject.CompareTag("Sword"))
        {
            CollectSword();
            other.gameObject.SetActive(false);
        }

        if (grounded == true)
        {
            this.GetComponent<Animator>().runtimeAnimatorController = animStandingWithSword as RuntimeAnimatorController;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BasicPlatform"))
        {
            grounded = false;
        }
        else if (other.gameObject.CompareTag("DisPlatform"))
        {
            grounded = false;
        }
        else if (other.gameObject.CompareTag("FallingPlatform"))
        {
            grounded = false;
        }
        else if (other.gameObject.CompareTag("MovingPlatform"))
        {
            grounded = false;
        }
    }

    void FixedUpdate()
    {

        if (Input.anyKeyDown)
        {
            Animate();
        }

        Movement();

        if (jump)
        {
            Jump();
        }

    }
}

