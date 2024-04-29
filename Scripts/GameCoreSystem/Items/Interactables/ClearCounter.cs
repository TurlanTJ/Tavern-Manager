using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : Interactable
{
    private Item defaultValue = null;
    public Item storedItem = null;

    void Start()
    {
        defaultValue = storedItem;
    }

    public override void Interact()
    {
        if(storedItem.itemData == null)
            return;

        // there's stored item, then pick up stored item

        PlayerManager playerManager = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
        storedItem.SpawnItemPrefab(playerManager.gameObject.transform); // spawn visuals
        GameObject itemObj = storedItem.itemPref;
        if(playerManager.CarryItem(itemObj)) // try to carry
            storedItem = defaultValue; // if successful, remove stored item from counter
        else
            itemObj.GetComponent<ItemGameObject>().DestroyPrefab(); // else destroy spawned visual
    }
    public override void Interact(Item item, int itemIdx) // store given item, if there is no stored item
    {
        if(storedItem.itemData != null)
            return;

        storedItem = item;
        PlayerManager playerManager = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
        playerManager.RemoveCarryingItem(itemIdx);
    }
}
