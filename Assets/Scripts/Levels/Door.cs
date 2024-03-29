using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
   
    public Room adjacent1, adjacent2;
    public bool isOpen = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //isOpen = false;
    }

    public bool openDoor()
    {
        Debug.Log("Door open?");
        if (!isOpen)
        {
            Debug.Log("Door was closed");
            //can change to sliding animation later
            if ((adjacent1.bossRoomSelf || adjacent2.bossRoomSelf) && !GetComponentInParent<Level>().getKeyState())
            {
                GetComponentInParent<Level>().doorsAccessibleToBoss.Add(this);
               
            }
            else
            {

                StartCoroutine(GetComponentInParent<Level>().DestroyDoor(this.gameObject));

                //init room adjacent1


                if (!adjacent1.isCleared)
                {
                    int temp = Random.Range(2, 5);
                    for (int i = 0; i < temp; i++)
                    {
                        adjacent1.SpawnEnemyInRoomRandom();
                        
                    }


                    if (adjacent1.bossRoomSelf)
                    {
                        adjacent1.SpawnBossInRoomRandom();
                    }
                }
                //init room adjacent2
                else if (!adjacent2.isCleared)
                {
                    int temp = Random.Range(2, 5);
                    for (int i = 0; i < temp; i++) { 
                        adjacent2.SpawnEnemyInRoomRandom();
                        
                    }
                    if (adjacent2.bossRoomSelf)
                    {
                        adjacent2.SpawnBossInRoomRandom();

                    }
                }

                isOpen = true;
            }
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
