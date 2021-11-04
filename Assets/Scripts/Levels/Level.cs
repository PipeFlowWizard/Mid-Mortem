using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    
    public LevelData data;
    public List<Room> Rooms;
    
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

    public void SpawnEnemy(Vector3 position, Room currentroom)
    {
        var enemy = Spawn(position, data.Spawnables[0]);
        enemy.GetComponent<Enemy>()._currentRoom = currentroom;
    }

}
