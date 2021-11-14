using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private List<Level> _levels;
    public enum biomes { forest, desert, snow };

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        //TODO: Generate Level, spawn entities and set defaults
        // Step 1
        // Create level
        List<Level> temp = new List<Level>();
        _levels = new List<Level>();
            temp.Add(GetComponent<LevelCreation>().createLevel(biomes.desert));
            temp.Add(GetComponent<LevelCreation>().createLevel(biomes.forest));
            temp.Add(GetComponent<LevelCreation>().createLevel(biomes.snow));

        for(int i = 0; i < temp.Count; i++)
        {
            int random = Random.Range(0, temp.Count);
            _levels.Add(temp[random]);
            temp.RemoveAt(random);
        }

        _levels[0].setNextLevel(_levels[1]);
        _levels[1].setNextLevel(_levels[2]);


            foreach (var room in  _levels[0].Rooms)
            {
                if (room.startSelf)
                {
                    player.transform.position = room.transform.position + Vector3.up;
                //first room is pAcifist    
                //for (int i = 0; i < 4; i++ )
                        room.SpawnEnemyInRoomRandom();
                }
            }
            
            
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
