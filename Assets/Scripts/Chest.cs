using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    public bool opened;
    public CircleCollider2D cc;
    public RaycastHit2D[] nearbyObjs = new RaycastHit2D[10];
    public bool potion, shard, gold;
    public SpriteRenderer spr;
    public Sprite openSprite;

	// Use this for initialization
	void Start () {
        cc = GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        popChest();
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!opened)
            {
                int targetCount = cc.Cast(Vector2.zero, nearbyObjs);
                if (nearbyObjs != null)
                {
                    for (int i = 0; i < targetCount && i < nearbyObjs.Length; i++)
                    {
                        RaycastHit2D hit = nearbyObjs[i];
                        PlayerControls tempChk = hit.transform.GetComponent<PlayerControls>();
                        PlayerControls.pc.canOpen = true;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            if (!opened)
            {
                int targetCount = cc.Cast(Vector2.zero, nearbyObjs);
                for (int i = 0; i < targetCount && i < nearbyObjs.Length; i++)
                {
                    RaycastHit2D hit = nearbyObjs[i];
                    PlayerControls tempChk = hit.transform.GetComponent<PlayerControls>();
                    PlayerControls.pc.canOpen = false;
                    //Debug.Log("Can't open!");
                }
            }
        }
    }

    public void popChest()
    {
        if (PlayerControls.pc.canOpen)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                opened = true;
                cc.enabled = false;
                if (potion)
                {
                    Controller.Instance.potionCount++;
                    ItemGet item = Instantiate(Controller.Instance.ig, transform.position, Quaternion.identity);
                    item.spr.sprite = item.potion;
                }
                if (shard)
                {
                    Controller.Instance.shardCount++;
                    ItemGet item = Instantiate(Controller.Instance.ig, transform.position, Quaternion.identity);
                    item.spr.sprite = item.shard;
                }
                if (gold)
                {
                    Controller.Instance.moneyCount += Controller.Instance.floorNumber * 20;
                    ItemGet item = Instantiate(Controller.Instance.ig, transform.position, Quaternion.identity);
                    item.spr.sprite = item.coinBag;
                }
                PlayerControls.pc.canOpen = false;
                //Change sprite
                spr.sprite = openSprite;
                //Send message that you got something.
            }
        }
    }
}
