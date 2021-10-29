using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LevelRoomCreation : MonoBehaviour
{
    public GameObject[] possibleLevels;
    public int roomXSize;
    public int roomZsize;
    public GameObject leftRight;
    public GameObject leftRightBot;
    public GameObject leftRightTop;

    public int gridN;
    public int gridM;
    //first two ints are coordinates x and z, third int is the type of the room 
    //type is as follows: 0: free-for-all; 1 guaranteed left-right, 2 guaranteed left right bot, 3 guaranteed left right top,4 start,5 end
    //resource used http://tinysubversions.com/spelunkyGen/
    int[,] grid;
        
    // Start is called before the first frame update
    void Start()
    {
        pathToVictory();

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
        for (int i = 0; i <gridN; i ++)
        {
            
            for(int j = 0; j <gridM; j ++)
            {

                if (grid[i, j] == 0)
                {
                    ////random
                    int randIndex = Random.Range(0, possibleLevels.Length);

                    Instantiate<GameObject>(possibleLevels[randIndex], new Vector3(i * roomXSize, 0, j * roomZsize), Quaternion.identity);
                }
                else if (grid[i, j] == 1)
                {
                    //guaranteed left right
                   

                    Instantiate<GameObject>(leftRight, new Vector3(i * roomXSize, 0, j * roomZsize), Quaternion.identity);
                }
                else if (grid[i, j] == 2)
                {
                    //guaranteed left right bot
                    int randIndex = Random.Range(0, possibleLevels.Length);

                    Instantiate<GameObject>(leftRightBot, new Vector3(i * roomXSize, 0, j * roomZsize), Quaternion.identity);
                }
                else if (grid[i, j] == 3)
                {
                    //guaranteed left right top
                    int randIndex = Random.Range(0, possibleLevels.Length);

                    Instantiate<GameObject>(leftRightTop, new Vector3(i * roomXSize, 0, j * roomZsize), Quaternion.identity);
                }
                else if (grid[i, j] == 4)
                {
                    //start room
                }
                else if(grid[i, j] == 5)
                {
                   //end room
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void pathToVictory()
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
}
