using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCreation : MonoBehaviour
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
    

    // Start is called before the first frame update

    void Start()
    {
        
    }
    //can create fucntion and call it from levelcreation
    public GameObject createRoom(Vector3 pos, bool topAssured = false, bool botAssured = false, bool rightAssured = false, bool leftAssured = false, float ratio = 0.33f,  bool enemy = true, bool boss = false, bool start = false)

    {
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
        bot.SetActive(botDoor);
        top.SetActive(topDoor);
        right.SetActive(rightDoor);
        left.SetActive(leftDoor);
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

}
