using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransporter : MonoBehaviour
{
    public RoomTransporter[] AdjecentRooms= new RoomTransporter[4];
    public Transform[] AdjacentDoors = new Transform[4];
    
    
    public void teleportPlayer(GameObject g, Room room)
    {
        Debug.Log("Hit confirmed");

        g.transform.position = room.transform.position + Vector3.up;
        //int goTo = (i + 2) % 4;
        //switch (i)
        //{
        //    case 0:
        //        Debug.Log("Move North");
        //        g.transform.position = (AdjecentRooms[i].AdjacentDoors[goTo].position + new Vector3(0, 0, 3));
        //        break;
        //    case 1:
        //        Debug.Log("Move East");
        //        g.transform.position = (AdjecentRooms[i].AdjacentDoors[goTo].position + new Vector3(-3, 0, 0));
        //        break;
        //    case 2:
        //        Debug.Log("Move South");
        //        g.transform.position = (AdjecentRooms[i].AdjacentDoors[goTo].position + new Vector3(0, 0, -3));
        //        break;
        //    case 3:
        //        Debug.Log("Move West");
        //        g.transform.position = (AdjecentRooms[i].AdjacentDoors[goTo].position + new Vector3(3, 0, 0));
        //        break;
        //}
    }
}
