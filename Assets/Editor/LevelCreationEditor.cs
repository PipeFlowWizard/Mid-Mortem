using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor((typeof(LevelCreation)))]
public class LevelCreationEditor : Editor
{
  public override void OnInspectorGUI()
  {
   base.OnInspectorGUI();

   if (GUILayout.Button("Generate Level"))
   {
       ((LevelCreation)target).GenerateLevel();
   }
  }
}
