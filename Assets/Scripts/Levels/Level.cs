using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    [SerializeField] List<GameObject> forestObstacles;
    public LevelData data;
    public List<Room> Rooms;
    public Level nextLevel;
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
                room.tp.setDestination(startRoom);
            }
        }
        
    }

    public void SpawnEnemy(Vector3 position, Room currentroom)
    {
        var rand = Random.Range(0, 2);
        var enemy = Spawn(position, data.Spawnables[rand]);
        enemy.GetComponent<Enemy>().CurrentRoom = currentroom;
    }
    public void SpawnObstacle(Vector3 position, Room currentroom)
    {
        var rand = Random.Range(0, data.ForestObstacles.Count);
        var obstacle = Spawn(position, data.ForestObstacles[rand]);
        obstacle.transform.SetParent(transform);
        
    }
    public IEnumerator SpawnBoss(Vector3 position, Room currentroom)
    {
        yield return new WaitForEndOfFrame();
        var enemy = Spawn(position, data.Spawnables[3]);
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
