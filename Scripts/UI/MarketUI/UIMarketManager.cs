using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMarketManager : MonoBehaviour
{
    [SerializeField] private GameObject marketPanel;
    [SerializeField] private GameObject marketSlotsParent;
    [SerializeField] private GameObject marketSlotPrefab;

    [SerializeField] private List<GameObject> spawnedSlots = new List<GameObject>();

    private MarketManager marketManager;

    void Start()
    {
        marketManager = MarketManager.instance;

        marketManager.onMarketOpened += OpenMarketPanel;
        marketManager.onMarketListFilled += UpdateMarketSlots;
    }

    private void OpenMarketPanel()
    {
        marketPanel.SetActive(!marketPanel.activeSelf);
    }

    private void UpdateMarketSlots(ItemSO newItem) // Initailize new market item slot
    {
        marketSlotsParent.GetComponent<GridLayoutGroup>().enabled = false;
        GameObject newSlot = null;
        if(spawnedSlots.Count > 0)
        {
            foreach(GameObject slot in spawnedSlots)
            {
                if(slot.GetComponent<UIMarketSlot>().itemData == newItem) // this item is already spawned, dont do anyhting
                    continue;

                newSlot = Instantiate(marketSlotPrefab, marketSlotsParent.transform); // else add this item to market slots
                newSlot.GetComponent<UIMarketSlot>().UpdateSlotItem(newItem);
                break;
            }
        }
        else
        {
            newSlot = Instantiate(marketSlotPrefab, marketSlotsParent.transform); // market items list is 0, added new item list
            newSlot.GetComponent<UIMarketSlot>().UpdateSlotItem(newItem);
        }
        spawnedSlots.Add(newSlot);
        marketSlotsParent.GetComponent<GridLayoutGroup>().enabled = true;
    }
}
