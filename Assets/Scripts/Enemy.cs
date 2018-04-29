using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public BoxCollider2D bc;
    public int dmg, hp;
    public enum eDir
    {
        Up,
        Down,
        Left,
        Right
    }
    public eDir dir;
    public Vector2 savedDir;

    // Use this for initialization
    void Start () {
        bc = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Controller.Instance.HP -= dmg;
            collision.rigidbody.AddForce(savedDir * 250f);
        }
    }
}
