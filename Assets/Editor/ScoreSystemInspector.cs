using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoreSystem))]
public class ScoreSystemInspector : Editor
{
    bool tagsScoreFoldout = false;
    bool scriptsScoreFoldout = false;
    int scriptsScoreCount;
    ScoreSystem myScoreSystem;
    


    public override void OnInspectorGUI()
    {
        myScoreSystem = (ScoreSystem)target;

        tagsScoreFoldout = EditorGUILayout.Foldout(tagsScoreFoldout, "Score On Death By Tag");

        EditorGUI.indentLevel++;
        if (tagsScoreFoldout)
        {
            for (int i = 0; i < myScoreSystem.tagScores.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(myScoreSystem.tagScores[i].tag, GUILayout.Width(185));
                myScoreSystem.tagScores[i].score = EditorGUILayout.IntField(myScoreSystem.tagScores[i].score);
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();

        scriptsScoreFoldout = EditorGUILayout.Foldout(scriptsScoreFoldout, "Score On Death By Script");

        EditorGUI.indentLevel++;

        scriptsScoreCount = EditorGUILayout.IntField(myScoreSystem.scriptScores.Count);

        while (scriptsScoreCount > myScoreSystem.scriptScores.Count)
            myScoreSystem.scriptScores.Add(new ScriptScore());

        while (scriptsScoreCount < myScoreSystem.scriptScores.Count)
            myScoreSystem.scriptScores.RemoveAt(myScoreSystem.scriptScores.Count - 1);

        if (scriptsScoreFoldout)
        {
            for (int i = 0; i < myScoreSystem.scriptScores.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                myScoreSystem.scriptScores[i].script = EditorGUILayout.ObjectField(myScoreSystem.scriptScores[i].script, typeof(MonoBehaviour), false) as MonoBehaviour;
                myScoreSystem.scriptScores[i].score = EditorGUILayout.IntField(myScoreSystem.scriptScores[i].score);
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
