using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
    //The usual. This is an object that controls everything from the shadows.
    public static Controller Instance;
    public int HP, DEF, ATK, maxHP, floorNumber;
    public bool isAlive, paused, stairActive;

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
        PauseMenu();
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
        HUD.display.y.gameObject.SetActive(false);
        HUD.display.n.gameObject.SetActive(false);
        HUD.display.question.gameObject.SetActive(false);
        Controller.Instance.floorNumber++;
        SceneManager.LoadScene("TestMap", LoadSceneMode.Single);
    }

    public void flickOff()
    {
        paused = false;
        HUD.display.y.gameObject.SetActive(false);
        HUD.display.n.gameObject.SetActive(false);
        HUD.display.question.gameObject.SetActive(false);
    }
}
