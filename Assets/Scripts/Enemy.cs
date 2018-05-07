using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public BoxCollider2D bc;
    public Rigidbody2D rb;
    public int dmg, maxHp, hp, storedXP, storedGold;
    public bool shard, potion;
    public float delayAction, delayTimer, mvtSpd, accel, aggroDist;
    public GameObject HPBar;
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
        Pursuer,
        MetalSlime
    }

    public Vector2 savedDir;
    public mvtType eMvt;
    public eDir dir;
    public Animator anim;

    // Use this for initialization
    void Start () {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        showPercentHP();
        if (hp < 1)
        {
            Controller.Instance.exp += storedXP;
            storedXP = 0;
            Controller.Instance.moneyCount += storedGold;
            storedGold = 0;
            if (shard)
            {
                Controller.Instance.shardCount++;
                shard = !shard;
                ItemGet item = Instantiate(Controller.Instance.ig, transform.position, Quaternion.identity);
                item.spr.sprite = item.shard;
            }
            else if (potion)
            {
                Controller.Instance.potionCount++;
                potion = !potion;
                ItemGet item = Instantiate(Controller.Instance.ig, transform.position, Quaternion.identity);
                item.spr.sprite = item.potion;
            }
            else
            {
                ItemGet item = Instantiate(Controller.Instance.ig, transform.position, Quaternion.identity);
                item.spr.sprite = item.coinBag;
            }
            Destroy(this.gameObject);
        }
        if (delayTimer < 0)
        {
            switch (eMvt)
            {
                case mvtType.Wanderer:
                    wander();
                    break;
                case mvtType.Pursuer:
                    pursue();
                    break;
                case mvtType.MetalSlime:
                    flee();
                    break;
            }
        }
        delayTimer -= Time.deltaTime;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Controller.Instance.invulnTimer < 0)
            {
                Controller.Instance.HP -= dmg;
                Controller.Instance.invulnTimer = 2f;
            }
        }
    }

    public void wander()
    {
        anim.SetBool("moving", false);
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
                anim.SetInteger("direction", 0);
                rb.velocity = Vector2.MoveTowards(tempVel, Vector2.up * mvtSpd, accel * Time.fixedDeltaTime);
            }
            else if (moveYN < .75f)
            {
                dir = eDir.Down;
                anim.SetInteger("direction", 2);
                rb.velocity = Vector2.MoveTowards(tempVel, Vector2.down * mvtSpd, accel * Time.fixedDeltaTime);
            }
            else if (moveYN < .825f)
            {
                dir = eDir.Right;
                anim.SetInteger("direction", 1);
                rb.velocity = Vector2.MoveTowards(tempVel, Vector2.right * mvtSpd, accel * Time.fixedDeltaTime);
            }
            else
            {
                dir = eDir.Left;
                anim.SetInteger("direction", 3);
                rb.velocity = Vector2.MoveTowards(tempVel, Vector2.left * mvtSpd, accel * Time.fixedDeltaTime);
            }
            anim.SetBool("moving", true);
            delayTimer = delayAction;
        }
    }

    public void pursue()
    {
        Vector3 pursuitRadius = new Vector2(PlayerControls.pc.transform.position.x - transform.position.x, PlayerControls.pc.transform.position.y - transform.position.y);
        if (pursuitRadius.magnitude < aggroDist)
        {
            pursuitRadius = pursuitRadius.normalized;
            transform.position += pursuitRadius*mvtSpd * Time.deltaTime;
            float ang = GlobalFxns.ToAng(pursuitRadius);
            Orientation(ang);
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }

    public void flee()
    {
        Vector3 pursuitRadius = new Vector2(transform.position.x - PlayerControls.pc.transform.position.x, transform.position.y - PlayerControls.pc.transform.position.y);
        if (pursuitRadius.magnitude < aggroDist)
        {
            pursuitRadius = pursuitRadius.normalized;
            transform.position += pursuitRadius * mvtSpd * Time.deltaTime;
        }
    }

    public void Orientation(float angle)
    {
        if (angle < 45 && angle >= -45)
        {
            anim.SetInteger("direction", 1);
        }
        if (angle >= 45 && angle < 135)
        {
            anim.SetInteger("direction", 0);
        }
        if (angle >= 135 && angle < 225)
        {
            anim.SetInteger("direction", 3);
        }
        if (angle >= -135 && angle < -45)
        {
            anim.SetInteger("direction", 2);
        }
    }

    public void showPercentHP()
    {
        if (hp > 0)
        {
            float tempHP = hp * 1f;
            float tempMax = maxHp * 1f;
            float percentHP = (tempHP / tempMax);
            HPBar.transform.localScale = new Vector3(percentHP*.5f, .5f, 0);
        }
        else
        {
            HPBar.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}
