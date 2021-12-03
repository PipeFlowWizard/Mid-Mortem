using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Teleporter : MonoBehaviour
{

    private int roomNum;
    public GameEvent levelProgressEvent;
    [SerializeField]
    private Room destination;
    private void Start()
    {
        
    }
    public void teleportPlayer(GameObject g, Room room)
    {
        Debug.Log("Hit confirmed");

        room._level.SetMusic();
        room._level.levelCounter += 1;
        g.transform.position = room.transform.position + Vector3.up;
        levelProgressEvent.Raise();

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
