using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Order
{
    public int orderId;
    public int tableId;
    public bool orderComplete = false;
    public List<RecipeSO> orderedRecipes = new List<RecipeSO>();
    public List<Item> expectedResuls = new List<Item>();

    public Order(){}

    public Order(int id, int table, List<RecipeSO> recipes)
    {
        orderId = id;
        tableId = table;
        orderedRecipes = recipes;

        foreach(RecipeSO r in orderedRecipes)
        {
            int itemIdx = -1;
            for(var i = 0; i < expectedResuls.Count; i++) // initialize new items that corresponds ordered recipes reault/meals
            {
                if(r.result != expectedResuls[i].itemData)
                    continue;
                itemIdx = i;
                break;
            }

            if(itemIdx == -1)
                expectedResuls.Add(new Item(r.result)); // if expected results list doesnt have the item, add it
            else // else simply increase the required count of the expected meal
                expectedResuls[itemIdx].currentCount++;
        }
    }
}
