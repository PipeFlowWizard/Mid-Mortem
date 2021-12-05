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

    public void TeleportPlayer(GameObject g, Room room)
    {
        Debug.Log("Hit confirmed");

        room._level.SetMusic();
        room._level.levelCounter += 1;
        g.transform.position = room.transform.position + Vector3.up;
        levelProgressEvent.Raise();

        room.SpawnEnemyInRoomRandom();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponentInParent<Player>();
            TeleportPlayer(player.gameObject, destination);
        }
        
    }

    internal void SetDestination(Room startRoom)
    {
        destination = startRoom;
    }
}
