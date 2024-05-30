using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOrderManager : MonoBehaviour
{
    [SerializeField] private GameObject ordersListPanel;
    [SerializeField] private GameObject orderSlotsParent;
    [SerializeField] private GameObject orderSlotPrefab;

    private List<GameObject> spawnedSlots = new List<GameObject>();

    private OrderManager orderManager;

    // Start is called before the first frame update
    void Start()
    {
        orderManager = OrderManager.instance;

        orderManager.onOrderPlaced += InstantiateOrderSlot;
        orderManager.onOrderCompleted += ClearSlot;
    }

    public void InstantiateOrderSlot(Order newOrder)  // Update active orders list
    {
        orderSlotsParent.GetComponent<GridLayoutGroup>().enabled = false;

        GameObject newSlot = Instantiate(orderSlotPrefab, orderSlotsParent.transform); // creating new slot
        newSlot.GetComponent<UIOrder>().UpdateSlot(newOrder); // updating the created order slot information
        spawnedSlots.Add(newSlot); // adding the created slot to the list to store

        orderSlotsParent.GetComponent<GridLayoutGroup>().enabled = true;

    }

    public void ClearSlot(Order order) // clear slots
    {
        int slotIdx = -1;
        for(var i = 0; i < spawnedSlots.Count; i++) // iterate through the spawned slots
        {
            if(spawnedSlots[i].GetComponent<UIOrder>().storedOrder == order) // if given order is found
                slotIdx = i; // assign its index in the spawned slots list
        }

        if(slotIdx != -1)  // check if the slot index is NOT default value
        {
            orderSlotsParent.GetComponent<GridLayoutGroup>().enabled = false;
            spawnedSlots.Remove(spawnedSlots[slotIdx]); // remove the slot from the list
            DestroyImmediate(orderSlotsParent.transform.GetChild(slotIdx).gameObject, true); // destroy the order slot
            orderSlotsParent.GetComponent<GridLayoutGroup>().enabled = true;
        }
    }
}
