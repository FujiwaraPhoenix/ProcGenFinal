using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGet : MonoBehaviour {
    public SpriteRenderer spr;
    public Sprite coinBag, shard, potion;
    public float timeToSD;
	// Use this for initialization
	void Start () {
        spr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (timeToSD <= 0)
        {
            Destroy(this.gameObject);
        }
        else if (timeToSD>.5f)
        {
            transform.position += new Vector3(0, .01f, 0);
        }
        else
        {

        }
        timeToSD -= Time.deltaTime;
	}
}
