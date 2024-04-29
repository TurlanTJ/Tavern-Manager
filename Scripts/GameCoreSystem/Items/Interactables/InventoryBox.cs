using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBox : Interactable
{
    private TavernInventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = TavernInventoryManager.instance;
    }

    public override void Interact()
    {
        inventoryManager.OpenInventory();
    }

    public override void Interact(Item item, int itemIdx)
    {
        bool success;
        inventoryManager.StoreItem(item, out success); // try to store given item

        if(!success)
            inventoryManager.RemoveItem(item); // if not successful remove it from storage list,
        else                                   // just in case it somehow registered the item
        {
            PlayerManager playerManager = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
            playerManager.RemoveCarryingItem(itemIdx); // if success, then remove given item from carrying list
        }
    }
}
