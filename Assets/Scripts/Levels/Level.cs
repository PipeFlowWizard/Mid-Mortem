using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    [SerializeField] List<GameObject> forestObstacles;
    public LevelData data;
    public List<Room> Rooms;
    public Level nextLevel;
    public int levelCounter = 1;
    public GameManager.biomes biome;
    public bool keyUnlocked = false;
   
    public List<Door> doorsAccessibleToBoss;
    internal void keyAcquired()
    {
        keyUnlocked = true;
        if (doorsAccessibleToBoss.Count > 0)
        {
            doorsAccessibleToBoss[0].openDoor();
            //disable them all
            foreach(Door door in doorsAccessibleToBoss)
            {
                StartCoroutine(DestroyDoor(door.gameObject));
                
            }
        }
      
    }

    public void SetMusic()
    {
        switch (biome)
        {
            case GameManager.biomes.desert:
                data.desertBiomeEvent.Raise();
                break;
            case GameManager.biomes.snow:
                data.snowBiomeEvent.Raise();
                break;
            case GameManager.biomes.forest:
                data.forestBiomeEvent.Raise();
                break;
            default:
                data.forestBiomeEvent.Raise();
                break;
        }
        
    }

    public bool getKeyState()
    {
        return keyUnlocked;
    }

    private void Start()
    {
        doorsAccessibleToBoss = new List<Door>();
    }

    /// <summary>
    /// Samples a random point within a rectangular area surrounding a point
    /// </summary>
    /// <param name="position">The center point of the sampling area</param>
    /// <param name="size">height and width of the area to sample</param>
    /// <returns>Returns a random point from within the sampling area</returns>
    public static Vector3 SamplePoint(Vector3 position, Vector2 size)
    {
        Vector2 rand = Random.insideUnitCircle * size / 2;
        Vector3 point = position + new Vector3(rand.x,1, rand.y);
        return point;
    }

    public GameObject Spawn(Vector3 position, GameObject gameObject)
    {
        var spawned = Instantiate(gameObject, position, quaternion.identity);
        return spawned;
    }

    internal void setNextLevel(Level level)
    {
        nextLevel = level;
        //set teleporter location of level
        Room startRoom = null;
        foreach (var room in nextLevel.Rooms)
        {
            if (room.startSelf)
            {
                startRoom = room;
            }
        }
        foreach (var room in this.Rooms)
        {
            if (room.bossRoomSelf)
            {
                room.tp.SetDestination(startRoom);
            }
        }
        
    }

    public Enemy SpawnEnemy(Vector3 position, Room currentroom)
    {
        var rand = Random.Range(0, 3);
        Enemy enemy = Spawn(position, data.Spawnables[rand]).GetComponent<Enemy>();
        enemy.CurrentRoom = currentroom;
        enemy.currentLevel = levelCounter;
        return enemy;
    }
    public void SpawnObstacle(Vector3 position, Room currentroom)
    {
        var rand = 0;
        GameObject obstacle = null;
        switch (biome)
        {
            case GameManager.biomes.forest:
                rand = Random.Range(0, data.ForestObstacles.Count);

                GameObject theObstacle = data.ForestObstacles[rand];
                if (theObstacle.name.Contains("Tree (5)"))
                {
                    position += new Vector3(0, 0.5f, 0);
                }
                obstacle = Spawn(position, theObstacle);
                break;
            case GameManager.biomes.desert:
                rand = Random.Range(0, data.SandObstacles.Count);
                obstacle = Spawn(position, data.SandObstacles[rand]);
                break;
            case GameManager.biomes.snow:
                rand = Random.Range(0, data.SnowObstacles.Count);
                obstacle = Spawn(position, data.SnowObstacles[rand]);
                break;
        }
        
        
        obstacle.transform.SetParent(transform);
        obstacle.transform.rotation = Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f);

    }
    //dont ask

    public void SpawnGrass(Vector3 position, Room currentroom)
    {
        var rand = 0;
        GameObject obstacle = null;
        switch (biome)
        {
            case GameManager.biomes.forest:
                rand = Random.Range(0, data.GrassForest.Count);
                position = new Vector3(position.x + 10, position.y, position.z);
                obstacle = Spawn(position, data.GrassForest[rand]);
                break;
            case GameManager.biomes.desert:
                rand = Random.Range(0, data.GrassSand.Count);
                position = new Vector3(position.x +15, position.y, position.z+10);
                obstacle = Spawn(position, data.GrassSand[rand]);
                break;
            
        }


        obstacle.transform.SetParent(currentroom.transform);
        if (biome == GameManager.biomes.desert)
        {
            obstacle.transform.position = new Vector3(position.x + 25, position.y, position.z);
        }

    }
    public void SpawnRocks(Vector3 position, Room currentroom)
    {
        var rand = Random.Range(0.5f, 3f);
        GameObject obstacle = null;
        switch (biome)
        {
            case GameManager.biomes.forest:
                
                position = new Vector3(position.x, position.y-0.25f, position.z);
                obstacle = Spawn(position, data.forestRock);
                break;
            case GameManager.biomes.desert:
                position = new Vector3(position.x, position.y - 0.25f, position.z);
                obstacle = Spawn(position, data.sandRock);
                break;
            case GameManager.biomes.snow:
                position = new Vector3(position.x, position.y - 0.25f, position.z);
                obstacle = Spawn(position, data.snowRock);
                rand = Random.Range(0.5f, 1f);
                break;


        }

        
        obstacle.transform.SetParent(currentroom.transform);
        obstacle.transform.localScale = obstacle.transform.localScale * rand;
        if (biome != GameManager.biomes.snow)
        {
            obstacle.transform.rotation = Random.rotation;
        }
        else
        {
            obstacle.transform.rotation = Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
        }


    }
    public IEnumerator SpawnBoss(Vector3 position, Room currentroom)
    {
        yield return new WaitForEndOfFrame();
        GameObject boss;
        switch (biome)
        {
            case GameManager.biomes.desert:
                boss = data.Spawnables[5];
                break;
            case GameManager.biomes.forest:
                boss = data.Spawnables[3];
                break;
            case GameManager.biomes.snow:
                boss = data.Spawnables[4];
                break;
            default:
                boss = data.Spawnables[3];
                break;
        }

        Enemy enemy = Spawn(position, boss).GetComponent<Enemy>();
        enemy.currentLevel = levelCounter;
        enemy.CurrentRoom = currentroom;




        // enemy.GetComponent<Enemy>()._currentRoom = currentroom;
    }

    internal void setBiome(GameManager.biomes theBiome)
    {
        biome = theBiome;
    }

    public IEnumerator DestroyDoor(GameObject door)
    {
        yield return new WaitForEndOfFrame();

        Destroy(door);


    }

}
