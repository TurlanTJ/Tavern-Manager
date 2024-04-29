using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIOrder : MonoBehaviour
{
    [SerializeField] private GameObject orderedMeals;
    [SerializeField] private GameObject table;

    public Order storedOrder = null;

    public void UpdateSlot(Order newOrder)
    {
        storedOrder = newOrder;
        UpdateSlot();
    }

    public void UpdateSlot()  //Update order Infor
    {
        if(storedOrder != null)
        {
            string meals = $"{storedOrder.orderedRecipes[0].result.itemName}";// first ordered meal
            for(var i = 1; i < storedOrder.orderedRecipes.Count; i++)   // added the next ones
                meals += $"\n{storedOrder.orderedRecipes[i].result.itemName}";
            orderedMeals.GetComponent<TextMeshProUGUI>().text = meals;
            table.GetComponent<TextMeshProUGUI>().text = $"{storedOrder.tableId++}"; // added table id to the order
        }
        else
        {   //reset
            orderedMeals.GetComponent<TextMeshProUGUI>().text = "";
            table.GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
