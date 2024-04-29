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

        orderManager.onOrderListUpdated += InstantiateOrderSlot;
    }

    public void InstantiateOrderSlot(List<Order> activeOrders)  // Update active orders list
    {
        ClearSlots(); // reset
        orderSlotsParent.GetComponent<GridLayoutGroup>().enabled = false;

        foreach(Order o in activeOrders) // loop through the active orders list and spawn order slot for each of them
        {
            GameObject newSlot = Instantiate(orderSlotPrefab, orderSlotsParent.transform);
            newSlot.GetComponent<UIOrder>().UpdateSlot(o);
            spawnedSlots.Add(newSlot);
        }

        orderSlotsParent.GetComponent<GridLayoutGroup>().enabled = true;

    }

    public void ClearSlots() // clear slots
    {
        spawnedSlots.Clear();
        while(orderSlotsParent.transform.childCount != 0)
        {
            DestroyImmediate(orderSlotsParent.transform.GetChild(0).gameObject, true);
        }
    }

}
