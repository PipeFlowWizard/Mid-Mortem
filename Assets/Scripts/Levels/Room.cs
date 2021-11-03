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
    private GameObject botRoom, topRoom, rightRoom, leftRoom;
    public Level _level;
    public Vector2 spawnArea;
    public Vector2 SpawnArea => spawnArea * new Vector2(transform.lossyScale.x,transform.lossyScale.y);
    public Color spawnAreaColor = Color.magenta;

    /// <summary>
    /// Sets room parameters
    /// </summary>
    /// <returns>Returns the configured room</returns>
    public GameObject createRoom(Vector3 pos, int type, int rightType, int topType, float ratio = 0.33f, bool leftWall = false, bool botWall=false)

    {

        if (topType == 6||topType==1) InstantiateWallDoor(topWallPref);
        else if (topType >= 2 || type == 3) InstantiateWallDoor(topDoorPref);
        else if (topType == 0)
        {
            //flip a coin
            
            bool coinFlip = Random.value > ratio;
            //Door
            if (coinFlip) InstantiateWallDoor(topDoorPref);
            //Wall
            else InstantiateWallDoor(topWallPref);
        }
        if (rightType == 6) InstantiateWallDoor(rightWallPref);
        else if (rightType>0||type>0)InstantiateWallDoor(rightDoorPref);
        else if (rightType == 0)
        {
            //flip a coin
            bool coinFlip = Random.value > ratio;
            //Door
            if (coinFlip) InstantiateWallDoor(rightDoorPref);
            //Wall
            else InstantiateWallDoor(rightWallPref);
            
        }
        if (botWall)InstantiateWallDoor(botWallPref);
        if (leftWall) InstantiateWallDoor(leftWallPref);
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

    /// <summary>
    /// Spawns a default enemy at random position in the current room
    /// </summary>
    public void SpawnEnemyInRoomRandom()
    {
        //TODO: Make this spawn enemies and items instead of a generic gameobject
        Vector3 point = Level.SamplePoint(transform.position, SpawnArea);
        Debug.Log("SEIRR");
        _level.SpawnEnemy(point);
    }
  private void InstantiateWallDoor(GameObject prefab)
    {
        GameObject door;
        door = Instantiate<GameObject>(prefab, transform);
        door.transform.SetParent(this.transform);
    }
    //Draw the spawning area
    private void OnDrawGizmos()
    {
        spawnAreaColor.a = 0.25f;
        Gizmos.color = spawnAreaColor;
        Gizmos.DrawCube(transform.position,new Vector3(SpawnArea.x,.1f,SpawnArea.y));
    }
}
