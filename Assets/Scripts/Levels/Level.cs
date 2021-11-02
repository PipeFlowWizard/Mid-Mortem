using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{

    public List<Room> Rooms;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public static Vector3 SamplePoint(Vector3 position, Vector2 size)
    {
        Vector2 rand = Random.insideUnitCircle * size / 2;
        Vector3 point = position + new Vector3(rand.x,rand.y, 0);
        
        //Debug.DrawLine(position, point);
        return point;
    }

    public GameObject Spawn(Vector3 position, GameObject gameObject)
    {
        var spawned = Instantiate(gameObject, position, quaternion.identity);
        return spawned;
    }

}
