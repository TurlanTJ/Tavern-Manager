using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICookingManager : MonoBehaviour
{
    [SerializeField] private GameObject cookingPanel;
    [SerializeField] private GameObject cookingSlotsParent;
    [SerializeField] private GameObject cookingSlotPrefab;
    [SerializeField] private GameObject recipeInfoPanel;

    [SerializeField] private List<GameObject> spawnedSlots = new List<GameObject>();

    [SerializeField] private List<RecipeSO> availableRecipes = new List<RecipeSO>();

    private CookingManager cookingManager;

    void Start()
    {
        cookingManager = CookingManager.instance;
        availableRecipes = cookingManager.GetAvailableRecipes();

        cookingManager.onCookingPanelOpened += OpenCookingPanel;
        cookingManager.onNewRecipeUnlocked += PopulateAvailableRecipes;
        cookingManager.onRecipeSelected += OpenSelectedRecipePanel;
    }

    public void OpenCookingPanel() // open cooking panel
    {
        cookingPanel.SetActive(!cookingPanel.activeSelf);
        if(!cookingPanel.activeInHierarchy)
            recipeInfoPanel.GetComponent<UISelectedRecipeInfoPanel>().CloseInfoPanel(); // if panel not active close recipe info panel
    }

    public void PopulateAvailableRecipes(RecipeSO newRecipe) // add new recipe to available recipes list
    {
        cookingSlotsParent.GetComponent<GridLayoutGroup>().enabled = false; // disable layout group, so that it doesnt mess up the spanwed slots
        GameObject newSlot = Instantiate(cookingSlotPrefab, cookingSlotsParent.transform); // init slot panel
        newSlot.GetComponent<UICookingRecipeSlot>().UpdateSlot(newRecipe); // update slot info
        spawnedSlots.Add(newSlot);
        cookingSlotsParent.GetComponent<GridLayoutGroup>().enabled = true;
    }

    public void OpenSelectedRecipePanel(RecipeSO selectedRecipe) // open selected recipe info panel
    {
        recipeInfoPanel.GetComponent<UISelectedRecipeInfoPanel>().InitInfoPanel(null); // Reset info pane;
        recipeInfoPanel.SetActive(true);
        recipeInfoPanel.GetComponent<UISelectedRecipeInfoPanel>().InitInfoPanel(selectedRecipe); // give the selected recipe
    }
}
