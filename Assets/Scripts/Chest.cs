using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    public bool opened;
    public CircleCollider2D cc;
    public RaycastHit2D[] nearbyObjs = new RaycastHit2D[10];

	// Use this for initialization
	void Start () {
        cc = GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!opened)
        {
            int targetCount = cc.Cast(Vector2.zero, nearbyObjs);
            if (nearbyObjs != null) {
                for (int i = 0; i < targetCount && i < nearbyObjs.Length; i++)
                {
                    RaycastHit2D hit = nearbyObjs[i];
                    PlayerControls tempChk = hit.transform.GetComponent<PlayerControls>();
                    PlayerControls.pc.canOpen = true;
                    Debug.Log("Can open!");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!opened)
        {
            int targetCount = cc.Cast(Vector2.zero, nearbyObjs);
            for (int i = 0; i < targetCount && i < nearbyObjs.Length; i++)
            {
                RaycastHit2D hit = nearbyObjs[i];
                PlayerControls tempChk = hit.transform.GetComponent<PlayerControls>();
                PlayerControls.pc.canOpen = false;
                Debug.Log("Can't open!");
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
                cc.gameObject.SetActive(false);
            }
        }
    }
}
