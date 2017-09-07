using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemManager))]
public class ItemManagerInspector : Editor
{
    bool itemListFoldout;
    int itemCount;

    public override void OnInspectorGUI()
    {
        ItemManager itemManager = (ItemManager)target;
        
        itemListFoldout = EditorGUILayout.Foldout(itemListFoldout, "Items");

        EditorGUI.indentLevel++;
        if (itemListFoldout)
        {
            itemCount = EditorGUILayout.IntField("Size", itemManager.items.Count);

            while (itemCount > itemManager.items.Count)
                itemManager.items.Add(new ItemInfo());

            while (itemCount < itemManager.items.Count)
                itemManager.items.RemoveAt(itemManager.items.Count - 1);

            for (int i = 0; i < itemManager.items.Count; i++)
            {
                EditorGUILayout.Space();
                itemManager.items[i].pickup = EditorGUILayout.ObjectField("Pickup", itemManager.items[i].pickup, typeof(Pickup), false) as Pickup;
                itemManager.items[i].script = EditorGUILayout.ObjectField("Script", itemManager.items[i].script, typeof(MonoScript), false) as MonoScript;
                itemManager.items[i].active = EditorGUILayout.Toggle("Active", itemManager.items[i].active);
            }
        }
        EditorGUI.indentLevel--;

        EditorUtility.SetDirty(target);
    }
}
