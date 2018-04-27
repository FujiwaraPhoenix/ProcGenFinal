using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    public static PlayerControls pc;
    public Rigidbody2D hitbox;
    public float mvtSpd = 10f;
    public float accel = 20f;
    //Default: Up, Down, Left, Right. In order.
    public Sprite[] playerSprites = new Sprite[4];
    public SpriteRenderer spr;

    //For checking what direction you're facing.
    public float timeUp, timeDown, timeLeft, timeRight;

    public enum pDir
    {
        Up,
        Down,
        Left,
        Right
    }

    public bool moving;
    public bool attacking;
    pDir playerDirection = pDir.Down;

	// Use this for initialization
	void Start () {
        hitbox = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        tryToMove();
        playerOrientation();
        playerSpriteUpdate(playerDirection);
    }


    void Awake()
    {
        if (pc == null)
        {
            DontDestroyOnLoad(gameObject);
            pc = this;
        }
        else if (pc != this)
        {
            Destroy(gameObject);
        }
    }

    public void playerOrientation()
    {
        //First, check if a button is down or not.
        if (!moving)
        {
            if (Input.GetKey(KeyCode.W))
            {
                moving = true;
                timeUp += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moving = true;
                timeDown += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moving = true;
                timeLeft += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moving = true;
                timeRight += Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                timeUp += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                timeDown += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                timeLeft += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                timeRight += Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                timeUp = 0;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                timeDown = 0;
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                timeLeft = 0;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                timeRight = 0;
            }
            if (timeUp > timeDown && timeUp > timeLeft && timeUp > timeRight)
            {
                playerDirection = pDir.Up;
            }
            if (timeDown > timeUp && timeDown > timeLeft && timeDown > timeRight)
            {
                playerDirection = pDir.Down;
            }
            if (timeRight > timeDown && timeRight > timeLeft && timeRight > timeUp)
            {
                playerDirection = pDir.Right;
            }
            if (timeLeft > timeDown && timeLeft > timeUp && timeLeft > timeRight)
            {
                playerDirection = pDir.Left;
            }
            if ((Input.GetKeyUp(KeyCode.W)) && (Input.GetKeyUp(KeyCode.S)) && (Input.GetKeyUp(KeyCode.A)) && Input.GetKeyUp(KeyCode.D)){
                moving = false;
            }
        }
        Debug.Log(playerDirection);
        //OnKeyUp, set that respective value to 0.
        //Then last, set direction to whatever is the largest (or leave it if all buttons are untouched and thus the player is not moving).
    }

    public void tryToMove()
    {
        bool tryUp = Input.GetKey(KeyCode.W);
        bool tryDown = Input.GetKey(KeyCode.S);
        bool tryLeft = Input.GetKey(KeyCode.A);
        bool tryRight = Input.GetKey(KeyCode.D);

        Vector2 mvtDir = Vector2.zero;
        if (tryUp)
        {
            mvtDir += Vector2.up;
        }
        if (tryDown)
        {
            mvtDir += Vector2.down;
        }
        if (tryLeft)
        {
            mvtDir += Vector2.left;
        }
        if (tryRight)
        {
            mvtDir += Vector2.right;
        }
        mvtDir.Normalize();
        Vector2 tempV = hitbox.velocity;
        
        hitbox.velocity = Vector2.MoveTowards(tempV, mvtDir*mvtSpd, accel*Time.fixedDeltaTime);
        //Debug.Log(hitbox.velocity);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            hitbox.velocity = Vector2.zero;
        }
    }

    public void playerSpriteUpdate(pDir playerD){
        if (playerD == pDir.Up)
        {
            spr.sprite = playerSprites[0];
        }
        else if (playerD == pDir.Down)
        {
            spr.sprite = playerSprites[1];
        }
        else if (playerD == pDir.Left)
        {
            spr.sprite = playerSprites[2];
        }
        else
        {
            spr.sprite = playerSprites[3];
        }
    }
}
