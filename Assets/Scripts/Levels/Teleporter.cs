using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    private int roomNum;
    private Room destination;
    private void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        transform.parent.GetComponent<RoomTransporter>().teleportPlayer(other.gameObject , destination);
        
    }

    internal void setDestination(Room startRoom)
    {
        destination = startRoom;
    }
}
