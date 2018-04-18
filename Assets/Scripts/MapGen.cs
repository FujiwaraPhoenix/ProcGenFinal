using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour {
    public RoomState spawner;
    public static int mapSize = 10;
    public RoomState[,] mapLocs = new RoomState[mapSize,mapSize];
    public GameObject[] rooms = new GameObject[10];
    public static int activeRooms = 1;
    public int startX, startY, currentX, currentY;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                mapLocs[i,j] = Instantiate(spawner, new Vector3((i * 5.9226f), (j * 5.9226f), 0f), Quaternion.identity);
				mapLocs[i,j].gridX = i;
				mapLocs[i,j].gridY = j;
			}
        }
        //initializeMap();
        altInitMap();
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                makeRoom(mapLocs[i, j]);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    /*void initializeMap()
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
        while ((genActive) || (activeRooms < 25)) {
            float determineAction = Random.Range(0, 1f);
            //Move left one.
            try
            {
                if (determineAction < (sdCap / 4))
                {
                    if (currentX > -1)
                    {
                        mapLocs[currentX, currentY].leftExit = true;
                        currentX--;
                        mapLocs[currentX, currentY].rightExit = true;
                    }
                }
                //Move right one.
                else if (determineAction < (sdCap / 2))
                {

                    if (currentX < 9)
                    {
                        mapLocs[currentX, currentY].rightExit = true;
                        currentX++;
                        mapLocs[currentX, currentY].leftExit = true;
                    }
                }
                //Down 1
                else if (determineAction < sdCap * ((3 / 4)))
                {
                    if (currentY > -1)
                    {
                        mapLocs[currentX, currentY].downExit = true;
                        currentY--;
                        mapLocs[currentX, currentY].upExit = true;
                    }
                }
                //Up 1
                else if (determineAction < sdCap)
                {
                    if (currentY < 9)
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
                    Debug.Log(mapLocs[currentX, currentY]);
                    Debug.Log(activeRooms);
                }
            }
            catch
            {
                continue;
            }
        }
        //Run through mapLocs, if active, set the room up.
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //Make the room a room or corridor
                if (mapLocs[i, j].active)
                {
                    if (mapLocs[i, j].roomType == -1)
                    {
                        float determineType = Random.Range(0, 1f);
                        if (determineType < .25f)
                        {
                            mapLocs[i, j].roomType = 0;
                        }
                        else
                        {
                            mapLocs[i, j].roomType = 1;
                        }
                    }
                }
            }
        }
    }*/

    //Alternate iniitalization function: Recursive branching paths.
    void altInitMap()
    {
        int startXPos = mapSize / 2;
        int startYPos = mapSize / 2;
        startX = startXPos;
        startY = startYPos;
        mapLocs[startXPos, startYPos].active = true;
        //These two should never be < 0 or > mapSize
        int currentX = startXPos;
        int currentY = startYPos;
        float branchChance = .9f;
        makeFirstRoom(mapLocs[startX, startY], branchChance, startX, startY);
    }

    void makeFirstRoom(RoomState r, float branchRate, int currX, int currY)
    {
        //First, we determine what sort of room this is; a regular room, or a corridor.
        float determineType = Random.Range(0, 1f);
        if (determineType < .5f)
        {
            r.roomType = 0;
        }
        else
        {
            r.roomType = 1;
        }
        //Now, we check which exits are open.
        float doorCount = Random.Range(0, 1);
        if (doorCount < .25f)
        {
            //Give the room 4 exits
            r.upExit = true;
            r.downExit = true;
            r.leftExit = true;
            r.rightExit = true;
        }
        else if (doorCount < .5f)
        {
            //Give the room 3 exits
            int missingDoor = Random.Range(0, 4);
            switch (missingDoor)
            {
                case 0:
                    r.upExit = true;
                    r.downExit = true;
                    r.leftExit = true;
                    break;
                case 1:
                    r.downExit = true;
                    r.leftExit = true;
                    r.rightExit = true;
                    break;
                case 2:
                    r.upExit = true;
                    r.downExit = true;
                    r.rightExit = true;
                    break;
                case 3:
                    r.upExit = true;
                    r.leftExit = true;
                    r.rightExit = true;
                    break;

            }
        }
        else if (doorCount < .75f)
        {
            //Give the room 2 exits
            int roomSelect = Random.Range(0, 6);
            switch (roomSelect)
            {
                case 0:
                    r.upExit = true;
                    r.leftExit = true;
                    break;
                case 1:
                    r.upExit = true;
                    r.rightExit = true;
                    break;
                case 2:
                    r.upExit = true;
                    r.downExit = true;
                    break;
                case 3:
                    r.rightExit = true;
                    r.leftExit = true;
                    break;
                case 4:
                    r.downExit = true;
                    r.leftExit = true;
                    break;
                case 5:
                    r.rightExit = true;
                    r.downExit = true;
                    break;
            }
        }
        else
        {
            int roomSelect = Random.Range(0, 4);
            switch (roomSelect)
            {
                case 0:
                    r.upExit = true;
                    break;
                case 1:
                    r.rightExit = true;
                    break;
                case 2:
                    r.downExit = true;
                    break;
                case 3:
                    r.leftExit = true;
                    break;
            }
        }
        if (r.leftExit)
        {
            branchOffshoot(1f, currX-1, currY, 3);
        }
        if (r.rightExit)
        {
            branchOffshoot(1f, currX + 1, currY, 2);
        }
        if (r.upExit)
        {
            branchOffshoot(1f, currX, currY + 1, 1);
        }
        if (r.downExit)
        {
            branchOffshoot(1f, currX, currY - 1, 0);
        }
    }

    void branchOffshoot(float branchRate, int currX, int currY, int lastVisited)
    {
		if (currX < 0 || currX >= mapSize || currY < 0 || currY >= mapSize) {
			return;
		}

		RoomState r = mapLocs[currX, currY];
        try
        {
            if (!(r.active))
            {
                r.active = true;
                //Roomtype
                float branchChk = Random.Range(0, 1f);
                float determineType = Random.Range(0, 1f);
                if (determineType < .5f)
                {
                    r.roomType = 0;
                }
                else
                {
                    r.roomType = 1;
                }
                int openChk = 0;
                float tempChk = branchRate * branchChk;
                if (tempChk >= .75f)
                {
                    openChk = 0;
                }
                else if (tempChk >= .5f)
                {
                    openChk = 1;
                }
                else if (tempChk >= .25f)
                {
                    openChk = 2;
                }
                else
                {
                    openChk = 3;
                }
                //lastVisited is, obv, the last room visited. 0 is above, 1 is below, 2 is left, 3 right.
                switch (lastVisited)
                {
                    case 0:
                        r.upExit = true;
                        //Check for L/R/D
                        switch (openChk)
                        {
                            //3 rooms open
                            case 0:
								r.leftExit = true;
                                branchOffshoot(branchRate - .05f, currX - 1, currY, 3);
								r.rightExit = true;
                                branchOffshoot(branchRate - .05f, currX + 1, currY, 2);
                                r.downExit = true;
                                branchOffshoot(branchRate - .05f, currX, currY - 1, 0);
                                break;
                            //2 rooms open
                            case 1:
                                int roomSet = Random.Range(0, 3);
                                switch (roomSet)
                                {
                                    case 0:
										r.leftExit = true;
                                        branchOffshoot(branchRate - .05f, currX - 1, currY, 3);

										r.rightExit = true;
                                        branchOffshoot(branchRate - .05f, currX + 1, currY, 2);
                                        break;
                                    case 1:
										r.leftExit = true;
                                        branchOffshoot(branchRate - .05f, currX - 1, currY, 3);

										r.downExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY - 1, 0);
                                        break;
                                    case 2:
										r.rightExit = true;
                                        branchOffshoot(branchRate - .05f, currX + 1, currY, 2);

										r.downExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY - 1, 0);
                                        break;
                                }
                                break;
                            //1 room open
                            case 2:
                                int roomChoice = Random.Range(0, 3);
                                switch (roomChoice)
                                {
                                    case 0:
                                        r.leftExit = true;
                                        branchOffshoot(branchRate - .05f, currX - 1, currY, 3);
                                        break;
                                    case 1:
										r.downExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY - 1, 0);
                                        break;
                                    case 2:
										r.rightExit = true;
                                        branchOffshoot(branchRate - .05f, currX + 1, currY, 2);
                                        break;
                                }
                                break;
                            //Nil.
                            case 3:
                                break;
                        }
                        break;
                    case 1:
                        r.downExit = true;
                        //Check for L/R/U
                        switch (openChk)
                        {
                            //3 rooms open
                            case 0:
								r.leftExit = true;
                                branchOffshoot(branchRate - .05f, currX - 1, currY, 3);
								r.rightExit = true;
                                branchOffshoot(branchRate - .05f, currX + 1, currY, 2);
								r.upExit = true;
                                branchOffshoot(branchRate - .05f, currX, currY + 1, 1);
                                break;
                            //2 rooms open
                            case 1:
                                int roomSet = Random.Range(0, 3);
                                switch (roomSet)
                                {
                                    case 0:
										r.leftExit = true;
                                        branchOffshoot(branchRate - .05f, currX - 1, currY, 3);
										r.rightExit = true;
                                        branchOffshoot(branchRate - .05f, currX + 1, currY, 2);
                                        break;
                                    case 1:
										r.leftExit = true;
                                        branchOffshoot(branchRate - .05f, currX - 1, currY, 3);
										r.upExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY + 1, 1);
                                        break;
                                    case 2:
										r.rightExit = true;
                                        branchOffshoot(branchRate - .05f, currX + 1, currY, 2);
										r.upExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY + 1, 1);
                                        break;
                                }
                                break;
                            //1 room open
                            case 2:
                                int roomChoice = Random.Range(0, 3);
                                switch (roomChoice)
                                {
                                    case 0:
										r.leftExit = true;
                                        branchOffshoot(branchRate - .05f, currX - 1, currY, 3);
                                        break;
                                    case 1:
										r.upExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY + 1, 1);
                                        break;
                                    case 2:
										r.rightExit = true;
                                        branchOffshoot(branchRate - .05f, currX + 1, currY, 2);
                                        break;
                                }
                                break;
                            case 3:
                                break;
                        }
                        break;
                    case 2:
                        r.leftExit = true;
                        //Check for U/R/D
                        switch (openChk)
                        {
                            //3 rooms open
                            case 0:
								r.rightExit = true;
                                branchOffshoot(branchRate - .05f, currX + 1, currY, 2);
								r.upExit = true;
                                branchOffshoot(branchRate - .05f, currX, currY + 1, 1);
								r.downExit = true;
                                branchOffshoot(branchRate - .05f, currX, currY - 1, 0);
                                break;
                            //2 rooms open
                            case 1:
                                int roomSet = Random.Range(0, 3);
                                switch (roomSet)
                                {
                                    case 0:
										r.upExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY + 1, 1);

										r.rightExit = true;
                                        branchOffshoot(branchRate - .05f, currX + 1, currY, 2);
                                        break;
                                    case 1:
										r.downExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY - 1, 0);

										r.upExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY + 1, 1);
                                        break;
                                    case 2:
										r.rightExit = true;
                                        branchOffshoot(branchRate - .05f, currX + 1, currY, 2);

										r.downExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY - 1, 0);
                                        break;
                                }
                                break;
                            //1 room open
                            case 2:
                                int roomChoice = Random.Range(0, 3);
                                switch (roomChoice)
                                {
                                    case 0:
                                        r.downExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY - 1, 0);
                                        break;
                                    case 1:
                                        r.upExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY + 1, 1);
                                        break;
                                    case 2:
										r.rightExit = true;
                                        branchOffshoot(branchRate - .05f, currX + 1, currY, 2);
                                        break;
                                }
                                break;
                            case 3:
                                break;
                        }
                        break;
                    case 3:
                        r.rightExit = true;
                        //Check for L/U/D
                        switch (openChk)
                        {
                            //3 rooms open
                            case 0:
								r.leftExit = true;
                                branchOffshoot(branchRate - .05f, currX - 1, currY, 3);
					
								r.upExit = true;
                                branchOffshoot(branchRate - .05f, currX, currY + 1, 1);

								r.downExit = true;
                                branchOffshoot(branchRate - .05f, currX, currY - 1, 0);
                                break;
                            //2 rooms open
                            case 1:
                                int roomSet = Random.Range(0, 3);
                                switch (roomSet)
                                {
                                    case 0:
										r.upExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY + 1, 1);
										r.leftExit = true;
                                        branchOffshoot(branchRate - .05f, currX - 1, currY, 3);
                                        break;
                                    case 1:
										r.downExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY - 1, 0);
										r.upExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY + 1, 1);
                                        break;
                                    case 2:
										r.leftExit = true;
                                        branchOffshoot(branchRate - .05f, currX - 1, currY, 3);
										r.downExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY - 1, 0);
                                        break;
                                }
                                break;
                            //1 room open
                            case 2:
                                int roomChoice = Random.Range(0, 3);
                                switch (roomChoice)
                                {
                                    case 0:
										r.downExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY - 1, 0);
                                        break;
                                    case 1:
										r.upExit = true;
                                        branchOffshoot(branchRate - .05f, currX, currY + 1, 1);
                                        break;
                                    case 2:
										r.leftExit = true;
                                        branchOffshoot(branchRate - .05f, currX - 1, currY, 3);
                                        break;
                                }
                                break;
                            case 3:
                                break;
                        }
                        break;
                }
            }
            else
            {
                //Decide whether or not to randomly open up the room. Or not.
                float open = Random.Range(0, 1f);
				//Debug.Log("Room at " + currX + "," + currY + " visited from " + lastVisited);
                switch (lastVisited)
                {
                    case 0:
                        if (open < .5f)
                        {
                            r.upExit = true;
							mapLocs[currX, currY+1].downExit = true;
                        }
                        else
                        {
							r.upExit = false;
                            mapLocs[currX, currY + 1].downExit = false;
                        }
                        break;
                    case 1:
                        if (open < .5f)
                        {
                            r.downExit = true;
							mapLocs[currX, currY-1].upExit = true;
                        }
                        else
                        {
							r.downExit = false;
                            mapLocs[currX, currY - 1].upExit = false;
                        }
                        break;
                    case 2:
                        if (open < .5f)
						{	
                            r.leftExit = true;
							mapLocs[currX-1, currY].rightExit = true;
                        }
                        else
                        {
							r.leftExit = false;
                            mapLocs[currX - 1, currY].rightExit = false;
                        }
                        break;
                    case 3:
                        if (open < .5f)
                        {
                            r.rightExit = true;
							mapLocs[currX+1, currY].leftExit = true;
                        }
                        else
                        {
							r.rightExit = false;
                            mapLocs[currX + 1, currY].leftExit = false;
                        }
                        break;
                }
            }
        }
		catch (System.Exception e) 
        {
			Debug.Log(e);
			Debug.Log(currX + "," + currY);

            return;
        }
    }

    //Drops a room down on a position
    void makeRoom(RoomState r)
    {
        if (r.gridX == 0)
        {
            r.leftExit = false;
        }
        if (r.gridY == 0)
        {
            r.downExit = false;
        }
        if (r.gridX == mapSize-1)
        {
            r.rightExit = false;
        }
        if (r.gridY == mapSize-1)
        {
            r.upExit = false;
        }
        GameObject thisRoom = null;
        if (r.roomType == 0)
        {
            if (r.upExit)
            {
                if (r.downExit)
                {
                    if (r.rightExit)
                    {
                        if (r.leftExit)
                        {
                            thisRoom = Instantiate(rooms[0], r.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            thisRoom = Instantiate(rooms[1], r.transform.position, Quaternion.Euler(0, 0, 90));
                        }
                    }
                    else if (r.leftExit)
                    {
                        thisRoom = Instantiate(rooms[1], r.transform.position, Quaternion.Euler(0, 0, -90));
                    }
                    else
                    {
                        thisRoom = Instantiate(rooms[3], r.transform.position, Quaternion.Euler(0, 0, 90));
                    }
                }
                //No down
                else if (r.rightExit)
                {
                    if (r.leftExit)
                    {
                        thisRoom = Instantiate(rooms[1], r.transform.position, Quaternion.Euler(0, 0, 180));
                    }
                    else
                    {
                        thisRoom = Instantiate(rooms[2], r.transform.position, Quaternion.Euler(0, 0, 180));
                    }
                }
                else if (r.leftExit)
                {
                    thisRoom = Instantiate(rooms[2], r.transform.position, Quaternion.Euler(0, 0, -90));
                }
                else
                {
                    thisRoom = Instantiate(rooms[4], r.transform.position, Quaternion.Euler(0, 0, 180));
                }
            }
            //No up
            else if (r.downExit)
            {
                if (r.rightExit)
                {
                    if (r.leftExit)
                    {
                        thisRoom = Instantiate(rooms[1], r.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        thisRoom = Instantiate(rooms[2], r.transform.position, Quaternion.Euler(0, 0, 90));
                    }
                }
                else if (r.leftExit)
                {
                    thisRoom = Instantiate(rooms[2], r.transform.position, Quaternion.identity);
                }
                else
                {
                    thisRoom = Instantiate(rooms[4], r.transform.position, Quaternion.identity);
                }
            }
            //No up/down
            else if (r.rightExit)
            {
                if (r.leftExit)
                {
                    thisRoom = Instantiate(rooms[3], r.transform.position, Quaternion.identity);
                }
                else
                {
                    thisRoom = Instantiate(rooms[4], r.transform.position, Quaternion.Euler(0, 0, 90));
                }
            }
            else
            {
                thisRoom = Instantiate(rooms[4], r.transform.position, Quaternion.Euler(0, 0, -90));
            }
        }
        //For corridors:
        if (r.roomType == 1)
        {
            if (r.upExit)
            {
                if (r.downExit)
                {
                    if (r.rightExit)
                    {
                        if (r.leftExit)
                        {
                            thisRoom = Instantiate(rooms[5], r.transform.position, Quaternion.identity);
                        }
                        else
                        {
                            thisRoom = Instantiate(rooms[6], r.transform.position, Quaternion.Euler(0, 0, 90));
                        }
                    }
                    else if (r.leftExit)
                    {
                        thisRoom = Instantiate(rooms[6], r.transform.position, Quaternion.Euler(0, 0, -90));
                    }
                    else
                    {
                        thisRoom = Instantiate(rooms[3], r.transform.position, Quaternion.Euler(0, 0, 90));
                    }
                }
                //No down
                else if (r.rightExit)
                {
                    if (r.leftExit)
                    {
                        thisRoom = Instantiate(rooms[6], r.transform.position, Quaternion.Euler(0, 0, 180));
                    }
                    else
                    {
                        thisRoom = Instantiate(rooms[7], r.transform.position, Quaternion.Euler(0, 0, 180));
                    }
                }
                else if (r.leftExit)
                {
                    thisRoom = Instantiate(rooms[7], r.transform.position, Quaternion.Euler(0, 0, -90));
                }
                else
                {
                    thisRoom = Instantiate(rooms[9], r.transform.position, Quaternion.Euler(0, 0, -90));
                }
            }
            //No up
            else if (r.downExit)
            {
                if (r.rightExit)
                {
                    if (r.leftExit)
                    {
                        thisRoom = Instantiate(rooms[6], r.transform.position, Quaternion.identity);
                    }
                    else
                    {
                        thisRoom = Instantiate(rooms[7], r.transform.position, Quaternion.Euler(0, 0, 90));
                    }
                }
                else if (r.leftExit)
                {
                    thisRoom = Instantiate(rooms[7], r.transform.position, Quaternion.identity);
                }
                else
                {
                    thisRoom = Instantiate(rooms[9], r.transform.position, Quaternion.Euler(0, 0, 90));
                }
            }
            //No up/down
            else if (r.rightExit)
            {
                if (r.leftExit)
                {
                    thisRoom = Instantiate(rooms[3], r.transform.position, Quaternion.identity);
                }
                else
                {
                    thisRoom = Instantiate(rooms[9], r.transform.position, Quaternion.Euler(0, 0, 180));
                }
            }
            else
            {
                thisRoom = Instantiate(rooms[9], r.transform.position, Quaternion.identity);
            }
        }
		if (thisRoom != null) {
			thisRoom.transform.parent = r.transform;
		}
    }

    void chkValid(RoomState r, int currX, int currY)
    {
        //Check rightExit
        try
        {
            if (((mapLocs[currX + 1, currY].leftExit) || r.rightExit) && (mapLocs[currX + 1, currY].active))
            {
                r.rightExit = true;
                mapLocs[currX + 1, currY].leftExit = true;
            }
            else
            {
                r.rightExit = false;
            }
        }
        catch
        {
            r.rightExit = false;
        }
        //Check leftExit
        try
        {
            if (((mapLocs[currX - 1, currY].rightExit) || r.leftExit) && (mapLocs[currX - 1, currY].active))
            {
                r.leftExit = true;
                mapLocs[currX - 1, currY].rightExit = true;
            }
            else
            {
                r.leftExit = false;
            }
        }
        catch
        {
            r.leftExit = false;
        }
        //Check upExit
        try
        {
            if ((mapLocs[currX, currY + 1].downExit) || r.upExit)
            {
                r.upExit = true;
                mapLocs[currX, currY + 1].downExit = true;
            }
            else
            {
                r.upExit = false;
            }
        }
        catch
        {
            r.upExit = false;
        }
        //Check downExit
        try
        {
            if ((mapLocs[currX - 1, currY].upExit)|| r.downExit)
            {
                r.downExit = true;
                mapLocs[currX - 1, currY].upExit = true;
            }
            else
            {
                r.downExit = false;
            }
        }
        catch
        {
            r.downExit = false;
        }
    }
}
