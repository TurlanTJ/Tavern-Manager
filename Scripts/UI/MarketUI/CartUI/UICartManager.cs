using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICartManager : MonoBehaviour
{
    [SerializeField] private GameObject cartPanel;
    [SerializeField] private GameObject cartSlotsParent;
    [SerializeField] private GameObject cartSlotPrefab;

    [SerializeField] private List<GameObject> spawnedSlots = new List<GameObject>();

    private MarketManager marketManager;

    void Start()
    {
        marketManager = MarketManager.instance;

        marketManager.onCartOpened += OpenCartPanel;
        marketManager.onCartSlotUpdated += UpdateCartSlot;
    }

    public void OpenCartPanel()
    {
        cartPanel.SetActive(!cartPanel.activeSelf);

        if(!cartPanel.activeInHierarchy)
            ClearSlots();
    }

    public List<GameObject> GetSpawnedSlots()
    {
        return spawnedSlots;
    }

    public void InstantiateSlots(Item item) // iinitialize cart slot with selected item
    {
        cartSlotsParent.GetComponent<GridLayoutGroup>().enabled = false;
        GameObject newSlot = Instantiate(cartSlotPrefab, cartSlotsParent.transform);
        newSlot.GetComponent<UICartSlot>().UpdateSlot(item, this);
        spawnedSlots.Add(newSlot);
        cartSlotsParent.GetComponent<GridLayoutGroup>().enabled = true;
    }

    public void UpdateCartSlot(Item addedItem) // update the given item stored in cart slot;
    {
        if(spawnedSlots.Count <= 0) // if no cart items, add this one to the list;
            InstantiateSlots(addedItem);
        else
        {
            bool foundSlot = true;

            foreach(GameObject obj in spawnedSlots) // else loop through the cart items
            {
                UICartSlot slot = obj.GetComponent<UICartSlot>();
                if(slot.slotItem.itemData == addedItem.itemData) // this is same as the given item, update cart item
                {
                    slot.UpdateSlot(addedItem, this);
                    return;
                }
                else
                    foundSlot = false;
            }

            if(!foundSlot) // if not found while looping, add given item to the cart items list
                InstantiateSlots(addedItem);
        }
    }

    public void ClearSlots()
    {
        for(var i = spawnedSlots.Count - 1; i >= 0; i--) // had to loop through backwards to not have "index out of range/cant modify while iterating" problem
            spawnedSlots[i].GetComponent<UICartSlot>().RemoveItemCompletely();
    }
}
