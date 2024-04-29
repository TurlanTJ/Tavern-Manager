using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UICookingRecipeSlot : MonoBehaviour, IPointerClickHandler
{   // Avaulabel recipes slot scrip to update slot inf about slected recipe
    [SerializeField] private GameObject slotIcon;
    [SerializeField] private GameObject slotName;

    public RecipeSO storedRecipe = null;

    public void UpdateSlot(RecipeSO recipe)
    {
        storedRecipe = recipe;
        UpdateSlot();
    }

    public void UpdateSlot() // update slot info
    {
        if(storedRecipe != null)
        {
            slotIcon.GetComponent<Image>().sprite = storedRecipe.result.itemIcon;
            slotName.GetComponent<TextMeshProUGUI>().text = storedRecipe.result.itemName;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CookingManager.instance.SelectRecipe(storedRecipe); // select current recipe when cliked
    }
}
