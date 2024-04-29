using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public GameObject itemPref;
    public ItemSO itemData;
    public int currentCount = 1;

    public Item(){}
    public Item(ItemSO data)
    {
        itemData = data;
    }

    public Item(ItemSO data, bool max)
    {
        itemData = data;
        if(max)
            currentCount = 999;
    }

    public void SpawnItemPrefab(Transform parent) // spawn visual for this item
    {
        itemPref = GameObject.Instantiate(itemData.itemPrefab); // get prefab reference from itemData
        itemPref.AddComponent<ItemGameObject>().InitItem(this, parent); // add ItemGameObject class to spawned item
    }

    public void UpdateStack(int diff) // update current stack, destroy of below 0
    {
        currentCount += diff;

        if(currentCount <= 0)
            itemPref.GetComponent<ItemGameObject>().DestroyPrefab();
    }
}
