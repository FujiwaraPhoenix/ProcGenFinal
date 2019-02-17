using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
    //The usual. This is an object that controls everything from the shadows.
    public static Controller Instance;
    public int HP, DEF, baseATK, wepATK, wepLv, armorLV, maxHP, floorNumber, exp, shardCount, level;
    public int potionCount, moneyCount;
    public bool isAlive, paused, stairActive, inTown, GAMEOVERScreen, startScreen;
    public float knockbackForce, invulnTimer;
    public Camera cam;
    public Enemy e, chase, metal;
    public Chest ch;
    public ItemGet ig;

    //public List<> inventory;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        if (startScreen)
        {
            HUD.display.gameObject.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                startScreen = false;
                inTown = true;
                isAlive = true;
                PlayerControls.pc.gameObject.SetActive(true);
                HUD.display.gameObject.SetActive(true);
                SceneManager.LoadScene("Town");
            }
        }
        if (isAlive)
        {
            PauseMenu();
            chugPotion();
            Town();
            invulnTimer -= Time.deltaTime;
            if (HP < 1)
            {
                playerIsKill();
            }
        }
        else
        {
            HUD.display.gameObject.SetActive(false);
            if (!GAMEOVERScreen)
            {
                GAMEOVERScreen = !GAMEOVERScreen;
                //Move to game over screen
            }
            restart();
        }
	}

    public void PauseMenu()
    {
        if (!Instance.paused)
        {
            if (!stairActive)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Instance.paused = true;
                    Time.timeScale = 0;
                    //Load up the menu.
                }
            }
        }
        else
        {
            if (!stairActive)
            {
                //Enter also acts as unpause.
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Instance.paused = false;
                    Time.timeScale = 1;
                    //Close the menu.
                }
            }
        }
    }

    public void newFloor()
    {
        paused = false;
        stairActive = false;
        HUD.display.stairBatch.SetActive(false);
        HUD.display.quickData.SetActive(true);
        Controller.Instance.floorNumber++;
        SceneManager.LoadScene("TestMap", LoadSceneMode.Single);
    }

    public void flickOff()
    {
        paused = false;
        stairActive = false;
        HUD.display.stairBatch.SetActive(false);
        HUD.display.quickData.SetActive(true);
    }

    public void chugPotion()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            if ((potionCount > 0) && ((Controller.Instance.HP < Controller.Instance.maxHP)))
            {
                int recoveryVal = (Controller.Instance.maxHP / 3);
                Controller.Instance.HP += recoveryVal;
                if (Controller.Instance.HP > Controller.Instance.maxHP)
                {
                    Controller.Instance.HP = Controller.Instance.maxHP;
                }
                potionCount -= 1;
            }
        }
    }

    public void Town()
    {
        if (inTown)
        {
            cam.transform.parent = null;
            PlayerControls.pc.gameObject.SetActive(false);
        }
        else
        {
            PlayerControls.pc.gameObject.SetActive(true);
            cam.transform.parent = PlayerControls.pc.transform;
            cam.transform.localPosition = new Vector3(0, 0, -32);
            //PlayerControls.transform.position.x, PlayerControls.pc.transform.position.y
        }
    }

    public void levelUp()
    {
        if (exp - (Controller.Instance.level * Controller.Instance.level) > 0)
        {
            level++;
            maxHP = Mathf.RoundToInt(Controller.Instance.maxHP * 1.01f + 10);
            baseATK += 2;
            exp -= (Controller.Instance.level * Controller.Instance.level);
        }
    }

    public void weaponUp()
    {
        if ((moneyCount - (Controller.Instance.wepLv * Controller.Instance.wepLv) > 0) && (shardCount-(Controller.Instance.wepLv + 1) > 0))
        {
            wepLv++;
            wepATK = Mathf.RoundToInt(Controller.Instance.wepATK * 1.02f + 5);
            moneyCount -= (Controller.Instance.wepLv * Controller.Instance.wepLv);
            shardCount -= Controller.Instance.wepLv;
        }
    }

    public void armorUp()
    {
        if ((moneyCount - (Controller.Instance.armorLV * Controller.Instance.armorLV) > 0) && (shardCount - (Controller.Instance.armorLV + 1) > 0))
        {
            wepLv++;
            DEF = Mathf.RoundToInt(Controller.Instance.DEF * 1.01f + 1);
            moneyCount -= (Controller.Instance.armorLV * Controller.Instance.armorLV);
            shardCount -= Controller.Instance.armorLV;
        }
    }

    public void buy1pot()
    {
        if (moneyCount - 100 > 0)
        {
            moneyCount -= 100;
            potionCount++;
        }
    }

    public void buy5pots()
    {
        if (moneyCount - 500 > 0)
        {
            moneyCount -= 500;
            potionCount++;
        }
    }

    public void ReturnToTown()
    {
        paused = true;
        inTown = true;
        HP = maxHP;
        HUD.display.quickData.SetActive(false);
        HUD.display.dungeonPause.SetActive(false);
        SceneManager.LoadScene("Town");
    }

    public void EnterDungeon()
    {
        HUD.display.lc.SetActive(false);
        HUD.display.quickData.SetActive(true);
        floorNumber = 1;
        paused = false;
        inTown = false;
        SceneManager.LoadScene("TestMap");
    }

    public void restart()
    {
        if (GAMEOVERScreen)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                HP = 10;
                DEF = 0;
                baseATK = 0;
                wepATK = 1;
                wepLv = 1;
                armorLV = 1;
                maxHP = 10;
                floorNumber = 1;
                exp = 0;
                shardCount = 0;
                level = 0;
                moneyCount = 0;
                potionCount = 2;
                isAlive = true;
                paused = false;
                stairActive = false;
                GAMEOVERScreen = false;
                startScreen = true;
                SceneManager.LoadScene("Start Screen");
            }
        }
    }

    public void playerIsKill()
    {
        paused = true;
        isAlive = false;
        PlayerControls.pc.moving = false;
        PlayerControls.pc.hitbox.velocity = Vector2.zero;
        HUD.display.gameObject.SetActive(false);
        GAMEOVERScreen = true;
        SceneManager.LoadScene("GameOver");
    }
}
