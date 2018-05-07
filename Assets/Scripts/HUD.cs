using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public static HUD display;
    public GameObject stairBatch, dungeonPause, quickData;
    public Image bg;
    public Text stats, currentFloor, hpcount;
    public Text wepUp, armorUp, lvUp, lvBefore, lvAfter, potCount, atkBA, armorBA, potCountA, moneyCount;
    public enum currentScreen
    {
        LocChoice,
        SmithChoice,
        Wep,
        Armor,
        Potion,
        Level
    }

    public currentScreen cs;
    public GameObject lc, sc, w, a, p, lv;

    // Use this for initialization
    void Start () {
		
	}

    void Awake()
    {
        if (display == null)
        {
            DontDestroyOnLoad(gameObject);
            display = this;
        }
        else if (display != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		if (!Controller.Instance.inTown)
        {
            dungeonUI();
        }
        else
        {
            townUI();
            updateTxt();
        }
    }

    public void dungeonUI()
    {
        if (!Controller.Instance.paused)
        {
            bg.enabled = false;
            if (!Controller.Instance.stairActive)
            {
                dungeonPause.SetActive(false);
            }
        }
        else
        {
            bg.enabled = true;
            if (!Controller.Instance.stairActive)
            {
                dungeonPause.SetActive(true);
            }
        }
        currentFloor.text = "Current Floor: " + Controller.Instance.floorNumber;
        hpcount.text = "HP: " + Controller.Instance.HP + "/" + Controller.Instance.maxHP;
        potCount.text = "x" + Controller.Instance.potionCount;
        stats.text = "Zephesta\nLevel " + Controller.Instance.level + "\nATK: " + (Controller.Instance.baseATK + Controller.Instance.wepATK) + "\nDEF: " + Controller.Instance.DEF + "\nEXP: " + Controller.Instance.exp + "\nGold: " + Controller.Instance.moneyCount + "\nVaryl Shards: " + Controller.Instance.shardCount;
    }

    public void townUI()
    {
        switch (cs)
        {
            case currentScreen.LocChoice:
                lc.gameObject.SetActive(true);
                bg.enabled = true;
                quickData.gameObject.SetActive(false);
                sc.SetActive(false);
                w.SetActive(false);
                a.SetActive(false);
                p.SetActive(false);
                lv.SetActive(false);
                break;
            case currentScreen.SmithChoice:
                lc.SetActive(false);
                a.SetActive(false);
                w.SetActive(false);
                sc.SetActive(true);
                break;
            case currentScreen.Armor:
                sc.SetActive(false);
                a.SetActive(true);
                break;
            case currentScreen.Wep:
                sc.SetActive(false);
                w.SetActive(true);
                break;
            case currentScreen.Potion:
                p.SetActive(true);
                lc.SetActive(false);
                break;
            case currentScreen.Level:
                lc.SetActive(false);
                lv.SetActive(true);
                break;
        }
    }

    public void backToStart()
    {
        cs = currentScreen.LocChoice;
        
    }

    public void backToSmith()
    {
        cs = currentScreen.SmithChoice;
    }

    public void changeSetArmor()
    {
        cs = currentScreen.Armor;
    }
    public void changeSetWeapon()
    {
        cs = currentScreen.Wep;
    }
    public void potShop()
    {
        cs = currentScreen.Potion;
    }
    public void ruins()
    {
        cs = currentScreen.Level;
    }

    public void updateTxt()
    {
        wepUp.text = "Your weapon is currently level " + Controller.Instance.wepLv + ".\nIt'll cost " + (Controller.Instance.wepLv * Controller.Instance.wepLv) + " gold and " + (Controller.Instance.wepLv + 1) + " shards to refine. Proceed?";
        armorUp.text = "Your armor is currently level " + Controller.Instance.armorLV + ".\nIt'll cost " + (Controller.Instance.armorLV * Controller.Instance.armorLV) + " gold and " + (Controller.Instance.armorLV + 1) + " shards to refine. Proceed?";
        lvUp.text = "You are currently level " + Controller.Instance.level + ".\nIt'll cost " +(Controller.Instance.level* Controller.Instance.level) + " EXP to level up. Proceed?";
        lvBefore.text = "Level " + (Controller.Instance.level) + ":\nHP: " + (Controller.Instance.maxHP + "\nATK: " + Controller.Instance.baseATK);
        lvAfter.text = "Level " + (Controller.Instance.level + 1) + ":\nHP: " + Mathf.RoundToInt(Controller.Instance.maxHP * 1.01f+10) + "\nATK: " + (Controller.Instance.baseATK +2);
        moneyCount.text = "Current Gold: " + Controller.Instance.moneyCount;
        potCountA.text = "x" + Controller.Instance.potionCount;
        atkBA.text= "Current Atk: "+Controller.Instance.wepATK+"\nUpgraded Atk: "+ Mathf.RoundToInt(Controller.Instance.wepATK*1.02f+5)+ "\nCurrent Gold: " + Controller.Instance.moneyCount + "\nShards: "+Controller.Instance.shardCount;
        armorBA.text = "Current Def: " + Controller.Instance.DEF + "\nUpgraded Def: " + Mathf.RoundToInt(Controller.Instance.DEF * 1.01f + 1) + "\nCurrent Gold: " + Controller.Instance.moneyCount + "\nShards: " + Controller.Instance.shardCount;
    }

    
}
