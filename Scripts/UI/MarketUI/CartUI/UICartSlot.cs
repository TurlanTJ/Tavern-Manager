using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICartSlot : MonoBehaviour
{
    [SerializeField] private GameObject slotIcon;
    [SerializeField] private GameObject slotCount;
    [SerializeField] private GameObject slotRemoveBtn;

    public Item slotItem = null;
    private UICartManager cartManager = null;

    public void UpdateSlot(Item item, UICartManager manager)
    {
        slotItem = item;
        cartManager = manager;
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if(slotItem != null) // Update cart item Information
        {
            slotIcon.GetComponent<Image>().sprite = slotItem.itemData.itemIcon;
            slotCount.SetActive(true);
            slotCount.GetComponentInChildren<TextMeshProUGUI>().text = $"{slotItem.currentCount}";
            slotRemoveBtn.SetActive(true);
        }
        else
        {
            slotIcon.GetComponent<Image>().sprite = null;
            slotCount.SetActive(false);
            slotCount.GetComponent<TextMeshProUGUI>().text = "";
            slotRemoveBtn.SetActive(false);
        }
    }

    public void ReduceNumbers() // reduce the current number stored cart item, when clikced on it
    {
        MarketManager.instance.RemoveItemFromCart(slotItem);
        if(slotItem.currentCount <= 0)
            RemoveItemCompletely();
    }

    public void RemoveItemCompletely() // remove cart item completely when X btn clicked
    {
        MarketManager.instance.RemoveWholeSelectedItemFromCart(slotItem);
        cartManager.GetSpawnedSlots().Remove(this.gameObject);
        DestroyImmediate(this.gameObject, true);
    }
}
