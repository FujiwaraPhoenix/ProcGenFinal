using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    public static PlayerControls pc;
    public Rigidbody2D hitbox;
    public float mvtSpd = 10f;
    public float accel = 20f;
    public float attackTimer, attackCD;
    //Default: Up, Down, Left, Right. In order.

    //For checking what direction you're facing.
    public float timeUp, timeDown, timeLeft, timeRight;
    protected static RaycastHit2D[] capTargets = new RaycastHit2D[10];

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

    public Animator anim;

    //For attacking.
    //As usual, Up/Down/Left/Right
    public BoxCollider2D[] atkHitboxes = new BoxCollider2D[4];

    //Item Interaction
    public bool canOpen;

	// Use this for initialization
	void Start () {
        hitbox = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!Controller.Instance.paused)
        {
            //The player will stay in place while attacking with this in place.
            if (!attacking)
            {
                tryToMove();
                playerOrientation();
                playerSpriteUpdate(playerDirection);
            }
            attack(playerDirection);
        }
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
                anim.SetBool("moving", true);
                timeUp += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moving = true;
                anim.SetBool("moving", true);
                timeDown += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moving = true;
                anim.SetBool("moving", true);
                timeLeft += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moving = true;
                anim.SetBool("moving", true);
                timeRight += Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                timeUp += Time.deltaTime;
            }
            else
            {
                timeUp = 0;
            }
            if (Input.GetKey(KeyCode.S))
            {
                timeDown += Time.deltaTime;
            }
            else
            {
                timeDown = 0;
            }
            if (Input.GetKey(KeyCode.A))
            {
                timeLeft += Time.deltaTime;
            }
            else
            {
                timeLeft = 0;
            }
            if (Input.GetKey(KeyCode.D))
            {
                timeRight += Time.deltaTime;
            }
            else
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
            if (!(Input.GetKey(KeyCode.W)) && !(Input.GetKey(KeyCode.S)) && !(Input.GetKey(KeyCode.A)) && !(Input.GetKey(KeyCode.D))){
                moving = false;
                anim.SetBool("moving", false);
            }
        }
        //Debug.Log(playerDirection);
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
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            if (hitbox.velocity != Vector2.zero)
            {
                switch (playerDirection)
                {
                    case pDir.Down:
                        hitbox.AddForce(Vector2.up * 250f);
                        break;
                    case pDir.Up:
                        hitbox.AddForce(Vector2.down * 250f);
                        break;
                    case pDir.Left:
                        hitbox.AddForce(Vector2.right * 250f);
                        break;
                    case pDir.Right:
                        hitbox.AddForce(Vector2.left * 250f);
                        break;
                }
            }
            else
            {
                switch (e.dir)
                {
                    case Enemy.eDir.Down:
                        hitbox.AddForce(Vector2.down * 250f);
                        break;
                    case Enemy.eDir.Up:
                        hitbox.AddForce(Vector2.up * 250f);
                        break;
                    case Enemy.eDir.Left:
                        hitbox.AddForce(Vector2.left * 250f);
                        break;
                    case Enemy.eDir.Right:
                        hitbox.AddForce(Vector2.right * 250f);
                        break;
                }
            }
        }
    }

    public void playerSpriteUpdate(pDir playerD){
        if (playerD == pDir.Up)
        {
            anim.SetInteger("direction", 0);
        }
        else if (playerD == pDir.Down)
        {
            anim.SetInteger("direction", 2);
        }
        else if (playerD == pDir.Left)
        {
            anim.SetInteger("direction", 3);
        }
        else
        {
            anim.SetInteger("direction", 1);
        }
    }

    public void attack(pDir currentDirection)
    {
        if (!attacking)
        {
            if (attackTimer <= 0 && Input.GetKeyDown(KeyCode.Space))
            {
                hitbox.velocity = Vector2.zero;
                if (currentDirection == pDir.Up)
                {
                    BoxCollider2D tempHitbox = atkHitboxes[0].GetComponent<BoxCollider2D>();
                    int targetCount = tempHitbox.Cast(Vector2.zero, capTargets);
                    for (int i = 0; i < targetCount && i < capTargets.Length; i++)
                    {
                        RaycastHit2D hit = capTargets[i];
                        Enemy tempE = hit.transform.GetComponent<Enemy>();
                        tempE.hp -= (Controller.Instance.baseATK + Controller.Instance.wepATK);
                        tempE.rb.AddForce(Vector2.up * Controller.Instance.knockbackForce);
                        Debug.Log("Swung up, hit something!");
                        //Play the animation.
                    }
                    if (targetCount == 0)
                    {
                        Debug.Log("Swung up, no targets!");
                    }
                }
                if (currentDirection == pDir.Down)
                {
                    BoxCollider2D tempHitbox = atkHitboxes[1].GetComponent<BoxCollider2D>();
                    int targetCount = tempHitbox.Cast(Vector2.zero, capTargets);
                    for (int i = 0; i < targetCount && i < capTargets.Length; i++)
                    {
                        RaycastHit2D hit = capTargets[i];
                        Enemy tempE = hit.transform.GetComponent<Enemy>();
                        tempE.hp -= (Controller.Instance.baseATK + Controller.Instance.wepATK);
                        tempE.rb.AddForce(Vector2.down * Controller.Instance.knockbackForce);
                        Debug.Log("Swung down, hit something!");
                        //Play the animation.
                    }
                    if (targetCount == 0)
                    {
                        Debug.Log("Swung down, no targets!");
                    }
                }
                if (currentDirection == pDir.Left)
                {
                    BoxCollider2D tempHitbox = atkHitboxes[2].GetComponent<BoxCollider2D>();
                    int targetCount = tempHitbox.Cast(Vector2.zero, capTargets);
                    for (int i = 0; i < targetCount && i < capTargets.Length; i++)
                    {
                        RaycastHit2D hit = capTargets[i];
                        Enemy tempE = hit.transform.GetComponent<Enemy>();
                        tempE.hp -= (Controller.Instance.baseATK + Controller.Instance.wepATK);
                        tempE.rb.AddForce(Vector2.left * Controller.Instance.knockbackForce);
                        Debug.Log("Swung left, hit something!");
                        //Play the animation.
                    }
                    if (targetCount == 0)
                    {
                        Debug.Log("Swung left, no targets!");
                    }
                }
                if (currentDirection == pDir.Right)
                {
                    BoxCollider2D tempHitbox = atkHitboxes[3].GetComponent<BoxCollider2D>();
                    int targetCount = tempHitbox.Cast(Vector2.zero, capTargets);
                    for (int i = 0; i < targetCount && i < capTargets.Length; i++)
                    {
                        RaycastHit2D hit = capTargets[i];
                        Enemy tempE = hit.transform.GetComponent<Enemy>();
                        tempE.hp -= (Controller.Instance.baseATK + Controller.Instance.wepATK);
                        tempE.rb.AddForce(Vector2.right * Controller.Instance.knockbackForce);
                        Debug.Log("Swung right, hit something!");
                        //Play the animation.
                    }
                    if (targetCount == 0)
                    {
                        Debug.Log("Swung right, no targets!");
                    }
                }
                attackTimer = attackCD;
                attacking = true;
                anim.SetBool("attacking", true);
            }
        }
        else if (attackTimer < 0 && attacking)
        {
            attacking = false;
            anim.SetBool("attacking", false);
        }
        attackTimer -= Time.deltaTime;
    }
}
