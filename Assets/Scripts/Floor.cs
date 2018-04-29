using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {
    public SpriteRenderer spr;
    public Sprite a, b;

	// Use this for initialization
	void Start () {
        spr = GetComponent<SpriteRenderer>();
        if (Random.Range(0,1f) < .5f)
        {
            spr.sprite = a;
        }
        else
        {
            spr.sprite = b;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
