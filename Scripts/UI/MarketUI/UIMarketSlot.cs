using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIMarketSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject slotIcon;
    [SerializeField] private GameObject slotName;
    [SerializeField] private GameObject slotPrice;
    [SerializeField] private GameObject slotType;

    public ItemSO itemData = null;

    public void UpdateSlotItem(ItemSO newItem)
    {
        itemData = newItem;
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if(itemData != null)
        {
            slotIcon.GetComponent<Image>().sprite = itemData.itemIcon;
            slotName.GetComponent<TextMeshProUGUI>().text = $"{itemData.itemName}";
            slotPrice.GetComponent<TextMeshProUGUI>().text = $"{itemData.itemPrice}";
            slotType.GetComponent<TextMeshProUGUI>().text = $"{itemData.itemType}";
        }
        else
        {
            slotIcon.GetComponent<Image>().sprite = null;
            slotName.GetComponent<TextMeshProUGUI>().text = "";
            slotType.GetComponent<TextMeshProUGUI>().text = "";
            slotPrice.GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MarketManager.instance.AddItemToCart(itemData);
    }
}
