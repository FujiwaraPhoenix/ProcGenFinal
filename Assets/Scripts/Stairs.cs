﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {
    public BoxCollider2D bc;

	// Use this for initialization
	void Start () {
        bc = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Pause
            //Ask if the player wants to move to the next floor.
            //If yes, run a function in the controller that loads the scene over.
            //if no, just unpause lol.
        }
    }
}
