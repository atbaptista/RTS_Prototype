using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CanEditMultipleObjects]
[CustomEditor(typeof(Enemy1))]
public class Enemy1Editor : Editor
{
    //SerializedProperty patrolEnd;
    Enemy1 enemy1;
    GameObject patrolEnd;

    public void OnEnable() {
        enemy1 = (Enemy1)target;
        patrolEnd = enemy1.patrolEnd;
        //patrolEnd = serializedObject.FindProperty("patrolEnd");
    }

    public override void OnInspectorGUI() {
        //serializedObject.Update();
        //EditorGUILayout.PropertyField(patrolEnd);
        //serializedObject.ApplyModifiedProperties();
    }
}