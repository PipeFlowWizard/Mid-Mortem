using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    private int roomNum;
    [SerializeField]
    private Room destination;
    private void Start()
    {
        
    }
    public void teleportPlayer(GameObject g, Room room)
    {
        Debug.Log("Hit confirmed");

        g.transform.position = room.transform.position + Vector3.up;

        room.SpawnEnemyInRoomRandom();
    }
        void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            Debug.Log(other.gameObject.name);
            teleportPlayer(other.gameObject, destination);
        }
        
    }

    internal void setDestination(Room startRoom)
    {
        destination = startRoom;
    }
}
