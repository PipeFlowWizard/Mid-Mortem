using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class LevelCreation : MonoBehaviour
{
    private int baseConstantRatio = 10;
    
    public GameObject roomPrefab;
    [SerializeField] public LevelData data;

    public int gridN;
    public int gridM;
    //first two ints are coordinates x and z, third int is the type of the room 
    //type is as follows: 0: free-for-all; 1 guaranteed left-right, 2 guaranteed left right bot, 3 guaranteed left right top,4 start,5 end
    //resource used http://tinysubversions.com/spelunkyGen/
    int[,] grid;
    GameObject[,] placedGrid;
    private List<GameObject> levelRooms;
    private GameObject currentLevel;
        
   
    void createLevel(int keys=1, int roomSize=10, int gridN=4, int gridM=4)
    {
        //guarantee there is a key
       //return list
        pathToVictory();
        placedGrid = new GameObject[gridN, gridM];
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < gridN; i++)
        {
            for (int j = 0; j < gridM; j++)
            {
                sb.Append(grid[i, j]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());

        Vector3 pos;
        GameObject room = null;
        List<Room> rooms = new List<Room>();
        currentLevel = new GameObject("Level");
        var level = currentLevel.AddComponent<Level>();
        level.data = data;
        int rightDoor, topDoor;
        for (int i = 0; i < gridN; i++)
        {

            for (int j = 0; j < gridM; j++)
            {
                
               
                pos = new Vector3(i * baseConstantRatio, 0, j * baseConstantRatio);
                room = Instantiate<GameObject>(roomPrefab, pos, Quaternion.identity);
                room.transform.SetParent(currentLevel.transform);
                rightDoor = topDoor = 6;
               
                topDoor = (j < gridM - 1) ? grid[i, j + 1] : 6;
                rightDoor = (i < gridN - 1) ? grid[i + 1, j] : 6;
                room.GetComponent<Room>().createRoom(pos, grid[i, j], rightType: rightDoor, topType: topDoor, leftWall:i==0,botWall:j==0);
                placedGrid[i, j] = room;
                room.name = "Room " + "[" + i + "," + j + "]";
                rooms.Add(room.GetComponent<Room>());
            }
        }
        addKey(keys);
        
        level.Rooms = rooms;
        foreach (var levelRoom in rooms)
        {
            
            levelRoom.SetLevel(level);
        }
    }
    private void addKey(int nbKeys)
    {
        while (nbKeys >= 1)
        {
            int x = Random.Range(0,gridN);
            int z = Random.Range(0, gridM);
            GameObject room = placedGrid[x, z];
            int theTypeOfRoom = grid[x, z];
            if((theTypeOfRoom == 1|| theTypeOfRoom == 2 || theTypeOfRoom == 3) && !room.GetComponent<Room>().keyRoomSelf)
            {
                room.GetComponent<Room>().spawnKey();
                nbKeys--;
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    //booleans are for keys, doors, enemy, start, end flags; ratio is to play with the randomness of the rooms. If you want more open rooms than closed one, use a smaller ratio
   
    private void pathToVictory()
    {
        grid = new int[gridN, gridM];
        bool endDoor =false;
        //choose starting point
        int initialX = Random.Range(0, gridN);
        int initialZ = 0;
        grid[initialX, initialZ] = 4;
        while (!endDoor)
        {
            int whereToGo = Random.Range(1, 6);
            if (whereToGo < 3)
            {
                //move left
                if (initialX != 0)
                {
                    
                    initialX--;
                    if (grid[initialX, initialZ] == 4) { grid[initialX, initialZ] = 4; }
                    
                    else grid[initialX, initialZ] = 1;
                    
                }

            }
            else if (whereToGo < 5 && whereToGo > 2)
            {
                //move right
                if (initialX != (gridN-1))
                {
                    
                    initialX++;
                    if (grid[initialX, initialZ] == 4) { grid[initialX, initialZ] = 4; }
                    else grid[initialX, initialZ] = 1;
                }
            }
            else if (whereToGo == 5)
            {
                if (initialZ == (gridM-1))
                {
                    grid[initialX, initialZ] = 5;
                    endDoor = true;
                    break;
                }
                //move up
                if (grid[initialX, initialZ] == 4) { grid[initialX, initialZ] = 4; }

                else grid[initialX, initialZ] = 3;
                initialZ++;
                grid[initialX, initialZ] = 2;
            }


        }
        
    }

    public void GenerateLevel()
    {
        DestroyImmediate(currentLevel);
        createLevel();
    }
}
