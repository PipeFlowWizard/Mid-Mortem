using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.AI;

// Modified version of https://learn.unity.com/tutorial/runtime-navmesh-generation#5c7f8528edbc2a002053b491
public class NavigationBaker : MonoBehaviour {

    private List<NavMeshSurface> _surfaces = new List<NavMeshSurface>();
    // private List<Transform> objectsToRotate = new List<Transform>();

    // Use this for initialization
    void Start () 
    {
        BakeCookies();
    }

    // Will rebake and rotate the navmesh surfaces. Useful for level generation since we need a few frames to generate the level first.
    public void BakeCookies()
    {
        // for (int j = 0; j < objectsToRotate.Count; j++) 
        // {
        //     objectsToRotate [j].localRotation = Quaternion.Euler (new Vector3 (0, Random.Range (0, 360), 0));
        // }

        for (int i = 0; i < _surfaces.Count; i++) 
        {
            _surfaces [i].BuildNavMesh ();    
        }    
    }

    public void AddGameObjectToSurface(GameObject obj)
    {
        // obj.AddComponent<NavMeshSurface>();
        _surfaces.Add(obj.GetComponent<NavMeshSurface>());
    }

    public void ClearSurfaces()
    {
        _surfaces.Clear();
    }

}
