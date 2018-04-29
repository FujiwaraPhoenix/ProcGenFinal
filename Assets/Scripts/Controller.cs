using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
