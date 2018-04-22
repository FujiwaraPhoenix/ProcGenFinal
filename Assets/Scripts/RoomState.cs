using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this is literally just saving data for the mapgen
public class RoomState : MonoBehaviour {
    //roomtype: 0 is room, 1 is corridor.
    public int roomType = -1;
    public bool upExit, downExit, leftExit, rightExit, playerSpawn, active, exitRoom;
    public Room roomTiles;
	public int gridX;
	public int gridY;
    //public Sprite cornerInner;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
