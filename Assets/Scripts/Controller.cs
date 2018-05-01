using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {
    //The usual. This is an object that controls everything from the shadows.
    public static Controller Instance;
    public int HP, DEF, ATK, maxHP;
    public bool isAlive, paused;

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
		
	}

    public void pauseMenu()
    {
        if (!Instance.paused)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 0;
                //Load up the menu.
            }
        }
        else
        {
            //Enter also acts as unpause.
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1;
                //Close the menu.
            }
        }
    }
}
