using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public int roomNum;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        transform.parent.GetComponent<RoomTransporter>().teleportPlayer(other.gameObject , roomNum);
    }
}
