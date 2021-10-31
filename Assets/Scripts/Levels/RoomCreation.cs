using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCreation : MonoBehaviour
{
    public bool botDoor, topDoor, rightDoor, leftDoor, bossRoom,KeyRooom;
    [SerializeField]
    private GameObject bot, top, right, left;
    [SerializeField]
    private List<GameObject> xWalls, zWalls;
    private GameObject botRoom, topRoom, rightRoom, leftRoom;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        createRoom();
    }
    //can create fucntion and call it from levelcreation
    void createRoom(int roomSize = 10,bool boss = false,bool key= false)
    {


        if (roomSize != 10)
        {

        }

        bot.SetActive(botDoor);
        top.SetActive(topDoor);
        right.SetActive(rightDoor);
        left.SetActive(leftDoor);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ObjectPlacement()
    {

    }

}
