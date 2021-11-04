using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        //TODO: Generate Level, spawn entities and set defaults
            // Step 1
            // Create level
            //  Initialize fields to defaults
            // Step 2
            // Spawn Player in start room
            // Step 3
            // Spawn obstacles in all rooms
            // Step 4
            // Spawn enemies in next rooms

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    // Step 3
        // Spawn room enemies on room unlock

        /// <summary>
        /// Resets the player's position and state to their respective default starting values.
        /// To be used at the start of a new level/death
        /// </summary>
        public void ResetPlayer()
        {
            
        }
        
        /// <summary>
        /// Resets the level to default starting values
        /// To be used for debugging purposes
        /// </summary>
        public void ResetLevel()
        {
            
        }

        /// <summary>
        /// Generates a new level.
        /// To be used when the player dies.
        /// </summary>
        public void RegenerateLevel()
        {
            
        }
}
