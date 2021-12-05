using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private List<Level> _levels;
    public enum biomes { forest, desert, snow };
    public float SceneResetTime = 5;

    public GameEvent doorsUnlockedEvent;
    public GameEvent levelProgressionEvent;
    // Start is called before the first frame update
    void Start()
    {
        //Load screen
        // Main menu
        // Create level
        initializeGame();
            
    }

    void initializeGame()
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

        for (int i = 0; i < temp.Count; i++)
        {
            int random = Random.Range(0, temp.Count);
            _levels.Add(temp[random]);
            temp.RemoveAt(random);
        }
        _levels.Add(temp[0]);
        temp.RemoveAt(0);

        _levels[0].setNextLevel(_levels[1]);
        
        _levels[0].transform.position = _levels[0].transform.position + new Vector3(0, 0, 500);
        _levels[1].setNextLevel(_levels[2]);
        _levels[1].transform.position = _levels[1].transform.position + new Vector3(500, 0, 0);

        foreach(Level lvl in _levels)
        {
            foreach(Room rm in lvl.Rooms)
            {
                for (int i = 0; i < 10; i++)
                {
                    rm.SpawnGrassInRoomRandom();
                    
                }

                if (!rm.bossRoomSelf)
                {
                    var ran = Random.Range(1, 5);
                    for (int i = 0; i < ran; i++)
                    {
                            rm.SpawnRocks();
                    }    
                }
                
                
            }
        }
        foreach (var room in _levels[0].Rooms)
        {
            if (room.startSelf)
            {
                player.transform.position = room.transform.position + Vector3.up;
                _levels[0].SetMusic();
                //first room is pAcifist    
                //for (int i = 0; i < 4; i++ )
                
                room.SpawnEnemyInRoomRandom();

                //try to access this enemy and make it weak
                //introduce reaping mechanic
                
            }
            
        }
      
        levelProgressionEvent.Raise();

    }
    // Update is called once per frame
    void Update()
    {
        
    }private IEnumerator WaitToReload()
    {
        yield return new WaitForSeconds(SceneResetTime);
        SceneManager.LoadScene(0);
    }

    public void OnGameOver()
    {
        StartCoroutine(WaitToReload());
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
