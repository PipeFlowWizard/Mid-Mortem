using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    Room adjacent1, adjacent2;
    bool isOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    bool openDoor()
    {
        if (!isOpen)
        {
            //can change to sliding animation later
            gameObject.SetActive(false);
            //init room adjacent1
            

            //init room adjacent2
        }
        return isOpen;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
