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
    public bool pSpawnPlaced = false;
    public bool stairsPlaced = false;
    public Sprite cornerInner;
    public Stairs stairway;

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
        while (!stairsPlaced)
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (!stairsPlaced)
                    {
                        spawnStairs(mapLocs[i, j]);
                    }
                }
            }
        }
        while (!pSpawnPlaced)
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (!pSpawnPlaced)
                    {
                        genPSpawn(mapLocs[i, j]);
                    }
                }
            }
        }
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                makeRoom(mapLocs[i, j]);
            }
        }
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                swapCorners(mapLocs[i, j]);
            }
        }
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (mapLocs[i, j].exitRoom)
                {
                    Stairs stairwell = Instantiate(stairway, mapLocs[i, j].transform.position, Quaternion.identity);
                }
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

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
                        thisRoom = Instantiate(rooms[8], r.transform.position, Quaternion.Euler(0, 0, 90));
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
                    thisRoom = Instantiate(rooms[8], r.transform.position, Quaternion.identity);
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
            r.roomTiles = r.transform.gameObject.GetComponentInChildren<Room>();
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

    public void swapCorners(RoomState r)
    {
        if (r.roomType == 0)
        {
            if (r.upExit)
            {
                if (r.downExit)
                {
                    if (r.leftExit)
                    {
                        if (r.rightExit)
                        {
                            //All
                            //Up
                            if (mapLocs[r.gridX, r.gridY + 1].roomType == 1)
                            {
                                r.roomTiles.CornerTiles[0].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[0].transform.localEulerAngles = new Vector3(0, 0, 90);
                                r.roomTiles.CornerTiles[1].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            }
                            //Right
                            if (mapLocs[r.gridX + 1, r.gridY].roomType == 1)
                            {
                                r.roomTiles.CornerTiles[2].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[3].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[2].transform.localEulerAngles = new Vector3(0, 0, 0);
                            }
                            //Down
                            if (mapLocs[r.gridX, r.gridY - 1].roomType == 1)
                            {
                                r.roomTiles.CornerTiles[4].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[5].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[4].transform.localEulerAngles = new Vector3(0, 0, -90);
                            }
                            //Left
                            if (mapLocs[r.gridX - 1, r.gridY].roomType == 1)
                            {
                                r.roomTiles.CornerTiles[6].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[7].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[6].transform.localEulerAngles = new Vector3(0, 0, 180);
                            }
                        }
                        else
                        {
                            //UDL
                            //Up
                            if (mapLocs[r.gridX, r.gridY + 1].roomType == 1)
                            {
                                r.roomTiles.CornerTiles[6].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[7].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[6].transform.localEulerAngles = new Vector3(0, 0, 180);
                            }
                            //Down
                            if (mapLocs[r.gridX, r.gridY - 1].roomType == 1)
                            {
                                r.roomTiles.CornerTiles[2].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[3].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[2].transform.localEulerAngles = new Vector3(0, 0, 0);
                            }
                            //Left
                            if (mapLocs[r.gridX - 1, r.gridY].roomType == 1)
                            {
                                r.roomTiles.CornerTiles[4].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[5].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[4].transform.localEulerAngles = new Vector3(0, 0, -90);
                            }
                        }
                    }
                    else
                    {
                        if (r.rightExit)
                        {
                            //UDR
                            //Up
                            if (mapLocs[r.gridX, r.gridY + 1].roomType == 1)
                            {
                                r.roomTiles.CornerTiles[2].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[2].transform.localEulerAngles = new Vector3(0, 0, 0);
                                r.roomTiles.CornerTiles[3].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            }
                            //Right
                            if (mapLocs[r.gridX + 1, r.gridY].roomType == 1)
                            {
                                r.roomTiles.CornerTiles[4].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[5].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[4].transform.localEulerAngles = new Vector3(0, 0, -90);
                            }
                            //Down
                            if (mapLocs[r.gridX, r.gridY - 1].roomType == 1)
                            {
                                r.roomTiles.CornerTiles[6].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[7].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[6].transform.localEulerAngles = new Vector3(0, 0, 180);
                            }
                        }
                        else
                        {
                            //UD
                            //Up
                            if (mapLocs[r.gridX, r.gridY + 1].roomType == 1)
                            {
                                r.roomTiles.CornerTiles[2].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[3].transform.localEulerAngles = new Vector3(0, 0, -90);
                                r.roomTiles.CornerTiles[3].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[2].transform.localEulerAngles = new Vector3(0, 0, 0);
                            }
                            //Down
                            if (mapLocs[r.gridX, r.gridY - 1].roomType == 1)
                            {
                                r.roomTiles.CornerTiles[6].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[6].transform.localEulerAngles = new Vector3(0, 0, 180);
                                r.roomTiles.CornerTiles[7].GetComponent<SpriteRenderer>().sprite = cornerInner;
                                r.roomTiles.CornerTiles[7].transform.localEulerAngles = new Vector3(0, 0, 90);
                            }
                        }
                    }
                }
                else if (r.leftExit)
                {
                    if (r.rightExit)
                    {
                        //URL
                        //Up
                        if (mapLocs[r.gridX, r.gridY + 1].roomType == 1)
                        {
                            r.roomTiles.CornerTiles[4].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[4].transform.localEulerAngles = new Vector3(0, 0, -90);
                            r.roomTiles.CornerTiles[5].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        }
                        //Right
                        if (mapLocs[r.gridX + 1, r.gridY].roomType == 1)
                        {
                            r.roomTiles.CornerTiles[6].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[7].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[6].transform.localEulerAngles = new Vector3(0, 0, 180);
                        }
                        //Left
                        if (mapLocs[r.gridX - 1, r.gridY].roomType == 1)
                        {
                            r.roomTiles.CornerTiles[2].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[3].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[2].transform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                    }
                    else
                    {
                        //UL
                        //Up
                        if (mapLocs[r.gridX, r.gridY + 1].roomType == 1)
                        {
                            r.roomTiles.CornerTiles[6].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[6].transform.localEulerAngles = new Vector3(0, 0, 180);
                            r.roomTiles.CornerTiles[7].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        }
                        //Left
                        if (mapLocs[r.gridX - 1, r.gridY].roomType == 1)
                        {
                            r.roomTiles.CornerTiles[4].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[5].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[4].transform.localEulerAngles = new Vector3(0, 0, -90);
                        }
                    }
                }
                else if (r.rightExit)
                {
                    //UR
                    //Up
                    if (mapLocs[r.gridX, r.gridY + 1].roomType == 1)
                    {
                        r.roomTiles.CornerTiles[4].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[4].transform.localEulerAngles = new Vector3(0, 0, -90);
                        r.roomTiles.CornerTiles[5].GetComponent<SpriteRenderer>().sprite = cornerInner;
                    }
                    //Right
                    if (mapLocs[r.gridX + 1, r.gridY].roomType == 1)
                    {
                        r.roomTiles.CornerTiles[6].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[7].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[6].transform.localEulerAngles = new Vector3(0, 0, 180);
                    }
                }
                else
                {
                    //Up only
                    //Up
                    if (mapLocs[r.gridX, r.gridY + 1].roomType == 1)
                    {
                        r.roomTiles.CornerTiles[4].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[4].transform.localEulerAngles = new Vector3(0, 0, -90);
                        r.roomTiles.CornerTiles[5].GetComponent<SpriteRenderer>().sprite = cornerInner;
                    }
                }
            }
            else if (r.downExit)
            {
                if (r.leftExit)
                {
                    if (r.rightExit)
                    {
                        //DLR
                        //Right
                        if (mapLocs[r.gridX + 1, r.gridY].roomType == 1)
                        {
                            r.roomTiles.CornerTiles[2].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[3].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[2].transform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                        //Down
                        if (mapLocs[r.gridX, r.gridY - 1].roomType == 1)
                        {
                            r.roomTiles.CornerTiles[4].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[5].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[4].transform.localEulerAngles = new Vector3(0, 0, -90);
                        }
                        //Left
                        if (mapLocs[r.gridX - 1, r.gridY].roomType == 1)
                        {
                            r.roomTiles.CornerTiles[6].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[7].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[6].transform.localEulerAngles = new Vector3(0, 0, 180);
                        }
                    }
                    else
                    {
                        //DL
                        //Down
                        if (mapLocs[r.gridX, r.gridY - 1].roomType == 1)
                        {
                            r.roomTiles.CornerTiles[4].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[5].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[4].transform.localEulerAngles = new Vector3(0, 0, -90);
                        }
                        //Left
                        if (mapLocs[r.gridX - 1, r.gridY].roomType == 1)
                        {
                            r.roomTiles.CornerTiles[6].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[7].GetComponent<SpriteRenderer>().sprite = cornerInner;
                            r.roomTiles.CornerTiles[6].transform.localEulerAngles = new Vector3(0, 0, 180);
                        }
                    }
                }
                else if (r.rightExit)
                {
                    //DR
                    //Right
                    if (mapLocs[r.gridX + 1, r.gridY].roomType == 1)
                    {
                        r.roomTiles.CornerTiles[4].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[5].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[4].transform.localEulerAngles = new Vector3(0, 0, -90);
                    }
                    //Down
                    if (mapLocs[r.gridX, r.gridY - 1].roomType == 1)
                    {
                        r.roomTiles.CornerTiles[6].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[7].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[6].transform.localEulerAngles = new Vector3(0, 0, 180);
                    }
                }
                else
                {
                    //D
                    //Down
                    if (mapLocs[r.gridX, r.gridY - 1].roomType == 1)
                    {
                        r.roomTiles.CornerTiles[4].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[5].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[4].transform.localEulerAngles = new Vector3(0, 0, -90);
                    }
                }
            }
            else if (r.leftExit)
            {
                if (r.rightExit)
                {
                    //LR
                    //Right
                    if (mapLocs[r.gridX + 1, r.gridY].roomType == 1)
                    {
                        r.roomTiles.CornerTiles[2].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[3].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[2].transform.localEulerAngles = new Vector3(0, 0, 0);
                    }
                    //Left
                    if (mapLocs[r.gridX - 1, r.gridY].roomType == 1)
                    {
                        r.roomTiles.CornerTiles[6].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[7].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[6].transform.localEulerAngles = new Vector3(0, 0, 180);
                    }
                }
                else
                {
                    //L
                    //Left
                    if (mapLocs[r.gridX - 1, r.gridY].roomType == 1)
                    {
                        r.roomTiles.CornerTiles[4].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[5].GetComponent<SpriteRenderer>().sprite = cornerInner;
                        r.roomTiles.CornerTiles[4].transform.localEulerAngles = new Vector3(0, 0, -90);
                    }
                }
            }
            else //if (r.rightExit)
            {
                //R
                //Right
                if (mapLocs[r.gridX + 1, r.gridY].roomType == 1)
                {
                    r.roomTiles.CornerTiles[4].GetComponent<SpriteRenderer>().sprite = cornerInner;
                    r.roomTiles.CornerTiles[5].GetComponent<SpriteRenderer>().sprite = cornerInner;
                    r.roomTiles.CornerTiles[4].transform.localEulerAngles = new Vector3(0, 0, -90);
                }
            }
        }
    }

    public void spawnStairs(RoomState r)
    {
        if (r.active && !r.playerSpawn && r.roomType == 0)
        {
            float randNo = Random.Range(0, 1f);
            if (randNo > .15f)
            {
                r.exitRoom = true;
                stairsPlaced = true;
            }
        }
    }

    public void genPSpawn(RoomState r)
    {
        if (r.active && !r.exitRoom && r.roomType == 0)
        {
            float randNo = Random.Range(0, 1f);
            if (randNo > .15f)
            {
                r.playerSpawn = true;
                pSpawnPlaced = true;
            }
        }
    }
}
