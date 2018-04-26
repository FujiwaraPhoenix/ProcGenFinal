using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    public static PlayerControls pc;
    public Rigidbody2D hitbox;
    public float mvtSpd = 10f;
    public float accel = 20f;

	// Use this for initialization
	void Start () {
        hitbox = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        tryToMove();
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
}
