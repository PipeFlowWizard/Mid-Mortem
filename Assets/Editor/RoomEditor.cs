using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor((typeof(Room)))]
public class RoomEditor : Editor
{
  public override void OnInspectorGUI()
  {
   base.OnInspectorGUI();

   if (GUILayout.Button("Spawn Enemy"))
   {
       ((Room)target).SpawnEnemyInRoomRandom();
   }
  }
}
