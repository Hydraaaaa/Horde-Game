using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class ItemInfo
{
    public string name;
    public Pickup pickup;
    public MonoScript script;
    public bool active;
    public int index;
}

[ExecuteInEditMode]
public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public List<ItemInfo> items;

	void Awake ()
    {
        if (items == null)
            items = new List<ItemInfo>();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].pickup != null)
                items[i].pickup.item = items[i];

            items[i].index = i;
        }

        instance = this;
	}

    public ItemInfo GetItem(int index)
    {
        return items[index];
    }
	
	void Update ()
    {
		
	}
}
