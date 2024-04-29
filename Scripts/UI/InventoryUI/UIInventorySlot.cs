using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIInventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject slotIcon;
    [SerializeField] private GameObject slotName;
    [SerializeField] private GameObject slotCount;
    [SerializeField] private GameObject slotPrice;

    public Item slotItem = null;

    public void UpdateSlotItem(Item newItem)
    {
        slotItem = newItem;
        UpdateSlot();
    }

    public void UpdateSlot() // Update spawned invenroty slot info
    {
        if(slotItem != null)
        {
            slotIcon.GetComponent<Image>().sprite = slotItem.itemData.itemIcon;
            slotName.GetComponent<TextMeshProUGUI>().text = $"{slotItem.itemData.itemName}";
            slotCount.GetComponent<TextMeshProUGUI>().text = $"{slotItem.currentCount}";
            slotPrice.GetComponent<TextMeshProUGUI>().text = $"{slotItem.itemData.itemPrice}";
        }
        else
        {
            slotIcon.GetComponent<Image>().sprite = null;
            slotName.GetComponent<TextMeshProUGUI>().text = "";
            slotCount.GetComponent<TextMeshProUGUI>().text = "";
            slotPrice.GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData) // when clikced get item from storage to player carry list
    {
        TavernInventoryManager.instance.TransferItem(slotItem, out bool sts);
    }

    public void DestroySlot()
    {
        DestroyImmediate(this.gameObject);
    }
}
