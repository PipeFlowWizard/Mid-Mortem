using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
   
    public Room adjacent1, adjacent2;
    bool isOpen = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //isOpen = false;
    }

    public bool openDoor()
    {
        if (!isOpen)
        {
            //can change to sliding animation later
            gameObject.SetActive(false);
            //init room adjacent1

            if (!adjacent1.isCleared)
            {
                for(int i = 0; i < 2; i++)
                    adjacent1.SpawnEnemyInRoomRandom();
                if (adjacent1.bossRoomSelf)
                {
                    adjacent1.SpawnBossInRoomRandom();
                }
            }
            if (!adjacent2.isCleared)
            {
                for(int i = 0; i < 2; i++)
                    adjacent2.SpawnEnemyInRoomRandom();
                if (adjacent2.bossRoomSelf)
                {
                    adjacent2.SpawnBossInRoomRandom();
                }
            }

            //init room adjacent2
            isOpen = true;
        }

        return isOpen;
    }
    public void SetAdjacent1(Room adjacent)
    {
        this.adjacent1 = adjacent;
    }
    public void SetAdjacent2(Room adjacent)
    {
        this.adjacent2 = adjacent;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
