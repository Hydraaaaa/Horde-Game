using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoreSystem))]
public class ScoreSystemInspector : Editor
{
    bool tagsScoreFoldout = false;
    ScoreSystem myScoreSystem;
    
    public override void OnInspectorGUI()
    {
        myScoreSystem = (ScoreSystem)target;

        tagsScoreFoldout = EditorGUILayout.Foldout(tagsScoreFoldout, "Score On Death");

        EditorGUI.indentLevel++;
        if (tagsScoreFoldout)
        {
            for (int i = 0; i < myScoreSystem.tagsScore.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(myScoreSystem.tagsScore[i].tag, GUILayout.Width(185));
                myScoreSystem.tagsScore[i].score = EditorGUILayout.IntField(myScoreSystem.tagsScore[i].score);
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Score On Civilian Rescue", GUILayout.Width(200));
        myScoreSystem.scoreOnCivilianRescue = EditorGUILayout.IntField(myScoreSystem.scoreOnCivilianRescue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Score On Player Escape", GUILayout.Width(200));
        myScoreSystem.scoreOnPlayerEscape = EditorGUILayout.IntField(myScoreSystem.scoreOnPlayerEscape);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Score On Scrap Pickup", GUILayout.Width(200));
        myScoreSystem.scoreOnScrapPickup = EditorGUILayout.IntField(myScoreSystem.scoreOnScrapPickup);
        EditorGUILayout.EndHorizontal();
    }
}
