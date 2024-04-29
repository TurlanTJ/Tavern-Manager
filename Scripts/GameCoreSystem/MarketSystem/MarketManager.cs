using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public static MarketManager instance;
    void Awake()
    {
        if(instance != null)
            return;
        instance = this;
    }

    [SerializeField] private List<ItemSO> allItems = new List<ItemSO>();
    [SerializeField] private List<ItemSO> spawnedItems = new List<ItemSO>();
    [SerializeField] private List<Item> cartItems = new List<Item>();

    private bool isCartOpen = false;
    private bool allItemsAdded = false;

    private TavernInventoryManager inventoryManager;

    public delegate void OnMarketOpened();
    public OnMarketOpened onMarketOpened;
    public delegate void OnMarketListFilled(ItemSO newItem);
    public OnMarketListFilled onMarketListFilled;

    public delegate void OnCartOpened();
    public OnCartOpened onCartOpened;
    public delegate void OnCartSlotUpdated(Item cartItem);
    public OnCartSlotUpdated onCartSlotUpdated;

    void Start()
    {
        inventoryManager = TavernInventoryManager.instance;
        allItems = inventoryManager.GetAllItems();
    }

    public void OpenMarketPanel()
    {
        if(!allItemsAdded)
            AddAllItemToMarket();
        onMarketOpened?.Invoke();
    }

    public void AddAllItemToMarket()
    {
        foreach(ItemSO i in allItems)
        {
            spawnedItems.Add(i);
            onMarketListFilled?.Invoke(i);
        }

        allItemsAdded = true;
    }

    public void AddItemToMarket(ItemSO newItem)
    {
        if(spawnedItems.Contains(newItem))
            return;

        spawnedItems.Add(newItem);
        onMarketListFilled?.Invoke(newItem);
    }

    public void OpenCart()
    {
        onCartOpened?.Invoke();
    }

    public void AddItemToCart(ItemSO selectedItem)
    {
        if(!isCartOpen)
        {
            OpenCart();
            isCartOpen = true;
        }

        if(cartItems.Count <= 0)
        {
            Item newItem = new Item(selectedItem);
            cartItems.Add(newItem);
            onCartSlotUpdated?.Invoke(newItem);
        }
        else
        {
            foreach(Item i in cartItems)
            {
                if(i.itemData != selectedItem)
                    continue;

                i.currentCount++;
                onCartSlotUpdated?.Invoke(i);
                return;
            }

            Item newItem = new Item(selectedItem);
            cartItems.Add(newItem);
            onCartSlotUpdated?.Invoke(newItem);
        }
    }

    public void RemoveItemFromCart(Item item)
    {
        int itemIdx = -1;

        foreach(Item cartItem in cartItems)
        {
            if(cartItem.itemData != item.itemData)
                continue;

            cartItem.currentCount--;
            onCartSlotUpdated?.Invoke(cartItem);
            itemIdx = cartItems.IndexOf(cartItem);
            break;
        }

        if(itemIdx == -1)
            return;

        if(cartItems[itemIdx].currentCount <= 0)
            RemoveWholeSelectedItemFromCart(cartItems[itemIdx]);
    }

    public void RemoveWholeSelectedItemFromCart(Item item)
    {
        if(!cartItems.Contains(item))
            return;

        cartItems.Remove(item);
    }

    public void Purchase()
    {
        if(cartItems.Count <= 0)
            return;

        int totalCost = 0;
        foreach(Item cartItem in cartItems)
            totalCost += cartItem.itemData.itemPrice;

        if(!inventoryManager.CanAfford(totalCost))
            Debug.Log("Not Enough Money");
        else
        {
            inventoryManager.UpdateMoney(-totalCost);

            foreach(Item cartItem in cartItems)
                inventoryManager.StoreItem(cartItem);

            for(var i = cartItems.Count - 1; i >= 0 ; i--)
                RemoveWholeSelectedItemFromCart(cartItems[i]);

            if(isCartOpen)
            {
                OpenCart();
                isCartOpen = false;
            }
        }
    }
}
