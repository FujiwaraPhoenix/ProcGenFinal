using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour {
    public RoomState spawner;
    public RoomState[,] mapLocs = new RoomState[10,10];
    public static int activeRooms = 1;
    public int startX, startY;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                mapLocs[i,j] = Instantiate(spawner, new Vector3((i * 5.9226f), (j * 5.9226f), 0f), Quaternion.identity);
            }
        }
        initializeMap();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void initializeMap()
    {
        int startXPos = Random.Range(0, 10);
        int startYPos = Random.Range(0, 10);
        startX = startXPos;
        startY = startYPos;
        mapLocs[startXPos, startYPos].playerSpawn = true;
        mapLocs[startXPos, startYPos].active = true;
        mapLocs[startXPos, startYPos].roomType = 0;
        //These two should never be < 0 or > 9
        int currentX = startXPos;
        int currentY = startYPos;
        float sdCap = .95f;
        bool genActive = true;
        //Makes the rooms with exits n stuff.
        while ((genActive) && (activeRooms < 10)) {
            float determineAction = Random.Range(0, 1);
            //Chance to turn
            //Move left one.
            if (determineAction < sdCap / 4)
            {
                if (currentX > 0)
                {
                    mapLocs[currentX, currentY].leftExit = true;
                    currentX--;
                    mapLocs[currentX, currentY].rightExit = true;
                }
            }
            //Move right one.
            else if (determineAction < sdCap / 2)
            {

                if (currentX < 10)
                {
                    mapLocs[currentX, currentY].rightExit = true;
                    currentX++;
                    mapLocs[currentX, currentY].leftExit = true;
                }
            }
            //Down 1
            else if (determineAction < sdCap * (3/4))
            {
                if (currentY > 0)
                {
                    mapLocs[currentX, currentY].downExit = true;
                    currentY--;
                    mapLocs[currentX, currentY].upExit = true;
                }
            }
            //Up 1
            else if (determineAction < sdCap)
            {
                if (currentY < 10)
                {
                    mapLocs[currentX, currentY].upExit = true;
                    currentY++;
                    mapLocs[currentX, currentY].downExit = true;
                }
            }
            //SD hit
            else
            {
                genActive = false;
            }
            if (mapLocs[currentX, currentY].active != true)
            {
                mapLocs[currentX, currentY].active = true;
                activeRooms++;
            }
        }
        //Run through mapLocs, if 
    }
    
}
