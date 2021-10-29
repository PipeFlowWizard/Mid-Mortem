using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCreation : MonoBehaviour
{
    public bool botDoor, topDoor, rightDoor, leftDoor;
    private GameObject bot, top, right, left;
    private GameObject botRoom, topRoom, rightRoom, leftRoom;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        bot = transform.FindChild("BotDoor").gameObject;
        top = transform.FindChild("TopDoor").gameObject;
        right = transform.FindChild("RightDoor").gameObject;
        left = transform.FindChild("LeftDoor").gameObject;

        bot.SetActive(botDoor);
        top.SetActive(topDoor);
        right.SetActive(rightDoor);
        left.SetActive(leftDoor);
    }
    //can create fucntion and call it from levelcreation


    // Update is called once per frame
    void Update()
    {
        
    }
    void ObjectPlacement()
    {

    }
}
