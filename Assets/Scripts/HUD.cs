using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public static HUD display;
    public Image bg;
    public Button y, n;
    public Text question;

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
		if (!Controller.Instance.paused)
        {
            bg.enabled = false;
        }
        else
        {
            bg.enabled = true;
        }
	}
}
