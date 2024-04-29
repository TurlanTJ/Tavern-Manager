using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIIngridientSlot : MonoBehaviour
{   // Selected Recipe's required ingridents info slot;
    [SerializeField] private GameObject slotIcon;
    [SerializeField] private GameObject slotName;
    [SerializeField] private GameObject slotItemCount;

    public ItemSO storedItem = null;

    public int storedCount = 0;

    public void UpdateSlot(ItemSO item, int count)
    {
        storedItem = item;
        storedCount = count;
        UpdateSlot();
    }

    public void UpdateSlot() // update current recipe ingrident info
    {
        if(storedItem != null)
        {
            slotIcon.GetComponent<Image>().sprite = storedItem.itemIcon;
            slotName.GetComponent<TextMeshProUGUI>().text = $"{storedItem.itemName}";
            slotItemCount.GetComponent<TextMeshProUGUI>().text = $"{storedCount}";
        }
        else
        {
            slotIcon.GetComponent<Image>().sprite = null;
            slotName.GetComponent<TextMeshProUGUI>().text = "";
            slotItemCount.GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
