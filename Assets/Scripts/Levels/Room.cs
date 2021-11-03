using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    public bool botDoor, topDoor, rightDoor, leftDoor, bossRoomSelf,keyRoomSelf, startSelf, enemySelf;
    [SerializeField]
    private GameObject bot, top, right, left,floor;
    [SerializeField]
    private Material blue, green, pink, stone;
    [SerializeField]
    public int x, z;
    [SerializeField]
    private List<GameObject> xWalls, zWalls;
    private GameObject botRoom, topRoom, rightRoom, leftRoom;
    public Level _level;
    public Vector2 spawnArea;

    public Vector2 SpawnArea => spawnArea * new Vector2(transform.lossyScale.x,transform.lossyScale.y);

    void Start()
    {
        spawnArea = new Vector2(9,9);
        
    }
    //can create fucntion and call it from levelcreation
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

    // Update is called once per frame
    void Update()
    {
        
    }
    void ObjectPlacement()
    {

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

    public void SpawnEnemyInRoomRandom()
    {
        //TODO: Make this spawn enemies and items instead of a generic gameobject
        Vector3 point = Level.SamplePoint(transform.position, SpawnArea);
        Debug.Log("SEIRR");
        _level.SpawnEnemy(point);
    }
    private void ScaleMode(int sizeRatio)
    {
        transform.position = new Vector3(transform.position.x * sizeRatio,0,transform.position.z * sizeRatio);
        /*//scale
        floor.transform.position = new Vector3(floor.transform.position.x * sizeRatio, 0, floor.transform.position.z * sizeRatio);
        floor.transform.localScale = new Vector3(floor.transform.localScale.x * sizeRatio, 0, floor.transform.localScale.z * sizeRatio);
      
        foreach (GameObject wall in xWalls)
        {
            wall.transform.position = new Vector3(wall.transform.position.x * sizeRatio, 0, wall.transform.position.z * sizeRatio);
            wall.transform.localScale = new Vector3(wall.transform.localScale.x + (sizeRatio - 1) * 4, 1, wall.transform.localScale.z);
            

        }
        foreach (GameObject wall in zWalls)
        {
            wall.transform.position = new Vector3(wall.transform.position.x * sizeRatio, 0, wall.transform.position.z * sizeRatio);
            wall.transform.localScale = new Vector3(wall.transform.localScale.x, 1, wall.transform.localScale.z + (sizeRatio - 1) * 4);
           

        }

        left.transform.position = new Vector3(left.transform.position.x * sizeRatio, 0, left.transform.position.z * sizeRatio);
        left.transform.localScale = new Vector3(left.transform.localScale.x, 1, left.transform.localScale.z + (sizeRatio - 1) * 2);
        


        right.transform.position = new Vector3(right.transform.position.x * sizeRatio, 0, right.transform.position.z * sizeRatio);
        right.transform.localScale = new Vector3(right.transform.localScale.x, 1, right.transform.localScale.z + (sizeRatio - 1) * 2);
       

        top.transform.position = new Vector3(top.transform.position.x * sizeRatio, 0, top.transform.position.z * sizeRatio);
        top.transform.localScale = new Vector3(top.transform.localScale.x + (sizeRatio - 1) * 2, 1, top.transform.localScale.z);
       

        bot.transform.position = new Vector3(bot.transform.position.x * sizeRatio, 0, bot.transform.position.z * sizeRatio);
        bot.transform.localScale = new Vector3(bot.transform.localScale.x + (sizeRatio - 1) * 2, 1, bot.transform.localScale.z);*/
        

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position,new Vector3(SpawnArea.x,.1f,SpawnArea.y));
    }
}
