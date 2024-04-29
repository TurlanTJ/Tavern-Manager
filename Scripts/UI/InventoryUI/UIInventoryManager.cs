using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventorySlotsParent;
    [SerializeField] private GameObject inventorySlotPrefab;

    [SerializeField] private List<GameObject> spawnedSlots = new List<GameObject>();

    private TavernInventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = TavernInventoryManager.instance;
        inventoryManager.onInventoryFilled += PopulateInventorySlot;
        inventoryManager.onInventoryOpened += OpenInventoryPanel;
    }

    public void OpenInventoryPanel()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void PopulateInventorySlot(Item item)
    {
        inventorySlotsParent.GetComponent<GridLayoutGroup>().enabled = false;
        int slotIdx = -1;
        if(spawnedSlots.Count > 0)
        {
            foreach(GameObject s in spawnedSlots)
            {
                if(s.GetComponent<UIInventorySlot>().slotItem.itemData != item.itemData)
                    continue;

                slotIdx = spawnedSlots.IndexOf(s);
            }

            if(slotIdx != -1)
            {
                spawnedSlots[slotIdx].GetComponent<UIInventorySlot>().UpdateSlotItem(item);

                if(spawnedSlots[slotIdx].GetComponent<UIInventorySlot>().slotItem.currentCount <= 0)
                {
                    spawnedSlots[slotIdx].GetComponent<UIInventorySlot>().DestroySlot();
                    spawnedSlots.Remove(spawnedSlots[slotIdx]);
                    return;
                }
            }
            else
            {
                GameObject newSlot = Instantiate(inventorySlotPrefab, inventorySlotsParent.transform);
                newSlot.GetComponent<UIInventorySlot>().UpdateSlotItem(item);
                spawnedSlots.Add(newSlot);
            }
        }
        else
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab, inventorySlotsParent.transform);
            newSlot.GetComponent<UIInventorySlot>().UpdateSlotItem(item);
            spawnedSlots.Add(newSlot);
        }
        
        inventorySlotsParent.GetComponent<GridLayoutGroup>().enabled = true;
    }
}
