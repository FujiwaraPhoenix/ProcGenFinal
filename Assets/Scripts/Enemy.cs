using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public BoxCollider2D bc;
    public Rigidbody2D rb;
    public int dmg, hp;
    public float delayAction, delayTimer, mvtSpd, accel;
    public enum eDir
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum mvtType
    {
        Wanderer,
        Patroller,
        Pursuer
    }

    public Vector2 savedDir;
    public mvtType eMvt;
    public eDir dir;

    // Use this for initialization
    void Start () {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (delayTimer < 0)
        {
            wander();
        }
        delayTimer -= Time.deltaTime;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Controller.Instance.HP -= dmg;
            savedDir = collision.rigidbody.velocity.normalized*-1;
            collision.rigidbody.AddForce(savedDir * 250f);
        }
    }

    public void wander()
    {
        rb.velocity = Vector2.zero;
        float moveYN = Random.Range(0, 1f);
        Vector2 tempVel = rb.velocity;
        if (moveYN < .5f)
        {
            delayTimer = delayAction;
        }
        else
        {
            if (moveYN < .625f)
            {
                dir = eDir.Up;
                rb.velocity = Vector2.MoveTowards(tempVel, Vector2.up * mvtSpd, accel * Time.fixedDeltaTime);
            }
            else if (moveYN < .75f)
            {
                dir = eDir.Down;
                rb.velocity = Vector2.MoveTowards(tempVel, Vector2.down * mvtSpd, accel * Time.fixedDeltaTime);
            }
            else if (moveYN < .825f)
            {
                dir = eDir.Right;
                rb.velocity = Vector2.MoveTowards(tempVel, Vector2.right * mvtSpd, accel * Time.fixedDeltaTime);
            }
            else
            {
                dir = eDir.Left;
                rb.velocity = Vector2.MoveTowards(tempVel, Vector2.left * mvtSpd, accel * Time.fixedDeltaTime);
            }
            delayTimer = delayAction;
        }
    }
}
