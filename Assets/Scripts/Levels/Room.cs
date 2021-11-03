using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    [SerializeField]  private GameObject bot, top, right, left,floor;
    [SerializeField]  private Material blue, green, pink, stone;
    [SerializeField]  private List<GameObject> xWalls, zWalls;
    
    public int x, z;
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
    public GameObject createRoom(Vector3 pos, bool topAssured = false, bool botAssured = false, bool rightAssured = false, bool leftAssured = false, float ratio = 0.33f,  bool enemy = true, bool boss = false, bool start = false)

    {
        //ScaleMode(3);
        if (topAssured)
        {
            topDoor = false;
        }
        else
        {
            topDoor = Random.value < ratio;
        }
        if (botAssured)
        {
            botDoor = false;
        }
        else
        {
            botDoor = Random.value < ratio;
        }
        if (rightAssured)
        {
            rightDoor = false;
        }
        else
        {
            rightDoor = Random.value < ratio;
        }
        if (leftAssured)
        {
            leftDoor = false;
        }
        else
        {
            leftDoor = Random.value < ratio;
        }

        transform.position = pos;
        
        enemySelf = enemy;
        bossRoomSelf = boss;
        startSelf = start;

        bot.GetComponent<MeshRenderer>().material = botDoor ? green : blue;
        top.GetComponent<MeshRenderer>().material = topDoor ? green : blue;
        left.GetComponent<MeshRenderer>().material = leftDoor ? green : blue;
        right.GetComponent<MeshRenderer>().material = rightDoor ? green : blue;


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



        //if (KeyRoom) spawnKey();
        //if (enemy) ;//spawnEnemy;
        //if (start) ;//spawnPlayer
        //if (bossRoom) ; //spawnBoss
        return this.gameObject;
    }
    
    public void spawnKey() {
        floor.GetComponent<MeshRenderer>().material = pink;
        keyRoomSelf = true;
    }
    public void openDoors()
    {
        bot.SetActive(botDoor);
        top.SetActive(topDoor);
        right.SetActive(rightDoor);
        left.SetActive(leftDoor);
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
  
    //Draw the spawning area
    private void OnDrawGizmos()
    {
        spawnAreaColor.a = 0.25f;
        Gizmos.color = spawnAreaColor;
        Gizmos.DrawCube(transform.position,new Vector3(SpawnArea.x,.1f,SpawnArea.y));
    }
}
