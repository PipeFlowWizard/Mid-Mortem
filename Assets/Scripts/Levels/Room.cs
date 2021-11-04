using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    [SerializeField]  private GameObject botWallPref, topWallPref, rightWallPref, leftWallPref,floor,topDoorPref,rightDoorPref;
    [SerializeField]  private Material blue, green, pink, stone;
   
    
   
    public bool botDoor, topDoor, rightDoor, leftDoor, bossRoomSelf,keyRoomSelf, startSelf, enemySelf;
    private Room botRoom, topRoom, rightRoom, leftRoom;
    public Level _level;
    public Vector2 spawnArea;
    public Vector2 SpawnArea => spawnArea * new Vector2(transform.lossyScale.x,transform.lossyScale.y);
    public Color spawnAreaColor = Color.magenta;
    [SerializeField]
    private List<Door> doors;
    public int currentEnemyCount = 0;
    public bool isCleared = false;
    public int CurrentEnemyCount
    {
        get => currentEnemyCount;
        set
        {
            currentEnemyCount = value;
            if (currentEnemyCount == 0)
            {
                isCleared = true;
                //call event
                Debug.Log("OpenDoors");
                foreach (var door in doors)
                {
                    door.openDoor();
                }
            }
        }
    }

    public Door botDoorRef, topDoorRef, rightDoorRef, leftDoorRef;
    /// <summary>
    /// Sets room parameters
    /// </summary>
    /// <returns>Returns the configured room</returns>
    public GameObject createRoom(Vector3 pos, int type, int rightType, int topType, float ratio = 0.33f, bool leftWall = false, bool botWall=false)

    {
        botDoorRef = null;
        topDoorRef = null;
        rightDoorRef = null;
        leftDoorRef = null;

        if (topType == 6||topType==1) InstantiateWall(topWallPref);
        else if (topType >= 2 || type == 3) topDoorRef = InstantiateDoor(topDoorPref);
        else if (topType == 0)
        {
            //flip a coin
            
            bool coinFlip = Random.value > ratio;
            //Door
            if (coinFlip) topDoorRef = InstantiateDoor(topDoorPref);
            //Wall
            else InstantiateWall(topWallPref);
        }
        if (rightType == 6) InstantiateWall(rightWallPref);
        else if (rightType>0||type>0) rightDoorRef = InstantiateDoor(rightDoorPref);
        else if (rightType == 0)
        {
            //flip a coin
            bool coinFlip = Random.value > ratio;
            //Door
            if (coinFlip) rightDoorRef = InstantiateDoor(rightDoorPref);
            //Wall
            else InstantiateWall(rightWallPref);
            
        }
        if (botWall)InstantiateWall(botWallPref);
        if (leftWall) InstantiateWall(leftWallPref);
        //check for 0 if top type is 0, flip a coin
        
       
        bossRoomSelf = type==5;
        startSelf = type==4;
        enemySelf = bossRoomSelf==startSelf;
      


        if (enemySelf)
        {
            floor.GetComponent<MeshRenderer>().material = stone;

        }
        else if (startSelf)
        {
            floor.GetComponent<MeshRenderer>().material = blue;
        }
        else if (bossRoomSelf)
        {
            floor.GetComponent<MeshRenderer>().material = green;
        }
        if (keyRoomSelf)
        {
            floor.GetComponent<MeshRenderer>().material = pink;
        }


        return this.gameObject;
    }
    
    public void spawnKey() {
        floor.GetComponent<MeshRenderer>().material = pink;
        keyRoomSelf = true;
    }
  
    
    public void SetLevel(Level level)
    {
        this._level = level;
    }
    public void SetLeftDoor(Door Door)
    {
        this.leftDoorRef = Door;
    }
    public Door GetRightDoor()
    {
        return this.rightDoorRef;
    }
    public Door GetTopDoor()
    {
        return this.topDoorRef;
    }
    public void SetBotDoor(Door Door)
    {
        this.botDoorRef = Door;
    }
    public void SetTopRoom(Room room)
    {
        this.topRoom = room;
    }
    public void SetBotRoom(Room room)
    {
        this.botRoom = room;
    }
    public void SetLeftRoom(Room room)
    {
        this.leftRoom = room;
    }
    public void SetRightRoom(Room room)
    {
        this.rightRoom = room;
    }
    /// <summary>
    /// Spawns a default enemy at random position in the current room
    /// </summary>
    public void SpawnEnemyInRoomRandom()
    {
        //TODO: Make this spawn enemies and items instead of a generic gameobject
        Vector3 point = Level.SamplePoint(transform.position, SpawnArea);
        Debug.Log("SEIRR");
        _level.SpawnEnemy(point,this);
        currentEnemyCount++;
    }
  private Door InstantiateDoor(GameObject prefab)
    {
        GameObject door;
        door = Instantiate<GameObject>(prefab, transform);
        door.transform.SetParent(this.transform);

        return door.GetComponent<Door>();
    }
    private void InstantiateWall(GameObject prefab)
    {
        GameObject wall;
        wall = Instantiate<GameObject>(prefab, transform);
        wall.transform.SetParent(this.transform);

    }
    //Draw the spawning area
    private void OnDrawGizmos()
    {
        spawnAreaColor.a = 0.25f;
        Gizmos.color = spawnAreaColor;
        Gizmos.DrawCube(transform.position,new Vector3(SpawnArea.x,.1f,SpawnArea.y));
    }
}
