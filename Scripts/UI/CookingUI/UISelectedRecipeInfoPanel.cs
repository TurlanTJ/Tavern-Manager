using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISelectedRecipeInfoPanel : MonoBehaviour
{   // Selected recipe info panel
    [SerializeField] private GameObject recipeIcon;
    [SerializeField] private GameObject recipeName;
    [SerializeField] private GameObject recipePrice;
    [SerializeField] private GameObject ingridientSlot;
    [SerializeField] private GameObject ingridientSlotParent;
    [SerializeField] private List<GameObject> spawnedIngridientSlots = new List<GameObject>();

    public RecipeSO selectedRecipe = null;

    public void InitInfoPanel(RecipeSO recipe)
    {
        selectedRecipe = recipe;
        UpdateInfoPanel();
    }

    public void UpdateInfoPanel() //Update recipe infos
    {
        if(selectedRecipe != null)
        {
            recipeIcon.GetComponent<Image>().sprite = selectedRecipe.result.itemIcon; // icon
            recipeName.GetComponent<TextMeshProUGUI>().text = $"{selectedRecipe.result.itemName}"; // name
            recipePrice.GetComponent<TextMeshProUGUI>().text = $"{selectedRecipe.result.itemPrice}"; // price
            AddIngridients(selectedRecipe.ingredients, selectedRecipe.ingredientsCount); // required ingrideints list
        }
        else
        {   // Reset
            recipeIcon.GetComponent<Image>().sprite = null;
            recipeName.GetComponent<TextMeshProUGUI>().text = "";
            recipePrice.GetComponent<TextMeshProUGUI>().text = "";
            DeleteIngridients();
        }
    }

    public void AddIngridients(List<ItemSO> items, List<int> count)
    {
        ingridientSlotParent.GetComponent<GridLayoutGroup>().enabled = false;
        for(var i = 0; i < items.Count; i++) // loop through required items list
        {
            GameObject newSlot = Instantiate(ingridientSlot, ingridientSlotParent.transform); // Init new Item with relevant number
            newSlot.GetComponent<UIIngridientSlot>().UpdateSlot(items[i], count[i]);    // e.g., this recipe need carrot x2 and etc,
            spawnedIngridientSlots.Add(newSlot); // then add the initialized item it required ingrients list
        }
        ingridientSlotParent.GetComponent<GridLayoutGroup>().enabled = true;
    }

    public void DeleteIngridients() // loop through spawnedIngridients list and delete one-by-one
    {
        for(var i = spawnedIngridientSlots.Count - 1; i >=0; i--)
        {
            DestroyImmediate(spawnedIngridientSlots[i], true);
            spawnedIngridientSlots.Remove(spawnedIngridientSlots[i]);
        }
    }

    public void StartCooking() // select thsi recipe to start cooking
    {
        if(selectedRecipe != null)
            CookingManager.instance.StartCooking(selectedRecipe);
    }

    public void CloseInfoPanel()
    {
        InitInfoPanel(null);
        gameObject.SetActive(false);
    }
}
