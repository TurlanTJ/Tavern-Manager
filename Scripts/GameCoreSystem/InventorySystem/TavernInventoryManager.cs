using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernInventoryManager : MonoBehaviour
{
    public static TavernInventoryManager instance;
    void Awake()
    {
        if(instance != null)
            return;
        instance = this;
    }

    [SerializeField] private List<ItemSO> allItems = new List<ItemSO>();
    [SerializeField] private List<Item> storedItems = new List<Item>();

    private bool isInventoryActive = false;

    public delegate void OnInventoryFilled(Item item);
    public OnInventoryFilled onInventoryFilled;

    public delegate void OnInventoryOpened();
    public OnInventoryOpened onInventoryOpened;

    public delegate void OnMoneyChanged(int currentCash);
    public OnMoneyChanged onMoneyChanged;

    public int money = 0;
    public int maxMoney = 999999;

    void Start()
    {
        UpdateMoney(5000); // set money
    }

    public List<ItemSO> GetAllItems()
    {
        return allItems;
    }

    public void OpenInventory()
    {
        PlayerManager playerManager = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
        isInventoryActive = !isInventoryActive;
        if(isInventoryActive) // if the inventory panel is open set play status to can't move
            playerManager.canMove = false;
        else
            playerManager.canMove = true;
        onInventoryOpened?.Invoke(); // fire trigger to all subscribed classes
    }

    public void AddAllItems()
    {
        foreach(ItemSO itemData in allItems)
        {   // check if item exists in the stored items list
            foreach(Item item in storedItems)
            {
                if(item.itemData == itemData)
                    continue;
            }

            Item addedItem = new Item(itemData, true); 
            storedItems.Add(addedItem); // add all items in the game to stored items list
            onInventoryFilled?.Invoke(addedItem);
        }
    }

    public void StoreItem(Item newItem) // function to store item to inventory
    {
        if(ContainsItem(newItem, out int idx)) // check if the given item exists in inventory
        {
            storedItems[idx].currentCount += newItem.currentCount; // if yes, then simply increase its count
            onInventoryFilled?.Invoke(storedItems[idx]);
            return;
        }

        storedItems.Add(newItem); // add given item
        onInventoryFilled?.Invoke(newItem);
        return;
    }

    public void StoreItem(Item newItem, out bool success)   // basically the same thing as the function above
    {                                                       // but, used only store items carried by player
        success = false;                                    // in case of success, player destroys the item's visual object
        if(ContainsItem(newItem, out int idx))
        {
            success = true;
            storedItems[idx].currentCount += newItem.currentCount;
            onInventoryFilled?.Invoke(storedItems[idx]);
            return;
        }

        success = true;
        storedItems.Add(newItem);
        onInventoryFilled?.Invoke(newItem);
        return;
    }

    public void UpdateItem(Item item) // function to update the stored item
    {
        List<Item> oItems = new List<Item>(); // list of items to be deleted

        if(ContainsItem(item, out int idx))
        {
            storedItems[idx].currentCount = item.currentCount; // set the new stored item amount 
            onInventoryFilled?.Invoke(storedItems[idx]);
            if(storedItems[idx].currentCount <= 0) // if item amount is less or equal to 0, add it to deleted items list
                oItems.Add(storedItems[idx]);
        }

        foreach(Item i in oItems) // delete items in deleted items list
            RemoveItem(i);

        oItems = null;
    }

    public bool ContainsItem(Item item, out int itemIndex) // function to checj if the item exists in the storage
    {
        itemIndex = -1; // set defaul item index

        for(var i = 0; i < storedItems.Count; i++)
        {
            if(storedItems[i].itemData != item.itemData)    // if current iterated item is NOT the same as the given item,
                continue;                                   // move to the next item in the list

            itemIndex = i; // if item exists, default index changed to relevant one
        }

        if(itemIndex != -1) // if default index changed, return true, else false
            return true;
        else
            return false;
    }

    public void TransferItem(Item item, out bool success) // pick up item from inventory storage
    {
        success = true;
        PlayerManager playerManager = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
        Item newItem = new Item(item.itemData); // initialize new item, and spawn visual object of it
        newItem.SpawnItemPrefab(playerManager.carryPos.transform);
        if(playerManager.CarryItem(newItem.itemPref))   // try to add it to player carrying list
        {                                               // in case of success, reduce the stored item count
            item.currentCount--;                        // if the number becomes less than or equal to 0, remove it from storage
            if(item.currentCount <= 0)
                RemoveItem(item);
            else                                        // else update the current count
                UpdateItem(item);
        }
        else
        {
            success = false;                        // if could not add to carrying list, then simply destroy the spawned object
            newItem.itemPref.GetComponent<ItemGameObject>().DestroyPrefab();
        }
    }

    public void RemoveItem(Item item) // remove item, if storage contains it
    {
        if(storedItems.Contains(item))
        {
            storedItems.Remove(item);
            onInventoryFilled?.Invoke(item);
        }
    }

    public bool CanAfford(int totalCost) // if the given number of sum exists, can afford, else not
    {
        if(totalCost <= money)
            return true;
        return false;
    }

    public void UpdateMoney(int difference) // update the current cash
    {
        money += difference;
        if(money >= maxMoney)
            money = maxMoney;

        onMoneyChanged?.Invoke(money);
    }
}
