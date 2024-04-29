using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingPod : Interactable
{
    public Item result;

    public override void Interact()
    {
        CookingManager.instance.OpenCookingPanel(); // opens cooking panel
    }

    public override void Interact(Item item, int itemIdx) // add ingridient to the cooking pod
    {
        bool addedItem;
        CookingManager.instance.AddIngridient(item, out addedItem); // if successfully added
        if(addedItem)
        {
            PlayerManager playerManager = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
            playerManager.RemoveCarryingItem(itemIdx); // remove item from player carrying list

            if(CookingManager.instance.CheckIfCooked(out result)) // check if meal is ready
            {
                result.SpawnItemPrefab(gameObject.transform); // if ready spawn visual object and
                GameObject resItem = result.itemPref;
                if(playerManager.CarryItem(resItem)) // try to add it player carrying list
                {
                    resItem.transform.parent = null;
                    resItem.transform.position = Vector3.zero;
                    resItem.transform.parent = playerManager.gameObject.transform;
                    resItem.transform.localPosition = Vector3.zero;
                    result = null;
                }
                else // else destroy spawned visual object
                    resItem.GetComponent<ItemGameObject>().DestroyPrefab();
            }
        }
    }

    public void PickUpMeal() // function to pick up ready meal
    {
        PlayerManager playerManager = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
        result.SpawnItemPrefab(playerManager.gameObject.transform); // spawn visual object
        GameObject resItem = result.itemPref;
        if(playerManager.CarryItem(resItem)) // if can add it carrying list, add; else destroy visual object
            result = null;
        else
            resItem.GetComponent<ItemGameObject>().DestroyPrefab();
    }
}
