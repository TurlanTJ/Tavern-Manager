using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingManager : MonoBehaviour
{
    public static CookingManager instance;
    void Awake()
    {
        if(instance != null)
            return;
        instance = this;
    }

    [SerializeField] private List<RecipeSO> allRecipes = new List<RecipeSO>();
    [SerializeField] private List<RecipeSO> availableRecipes = new List<RecipeSO>();

    private bool isCookingPanelActive = false;
    private bool allRecipesAdded = false;

    public RecipeSO cookingRightNow = null;
    [SerializeField] private List<Item> requiredItems = new List<Item>();
    [SerializeField] private List<Item> addedItems = new List<Item>();

    public delegate void OnCookingPanelOpened();
    public OnCookingPanelOpened onCookingPanelOpened;
    public delegate void OnNewRecipeUnlocked(RecipeSO recipe);
    public OnNewRecipeUnlocked onNewRecipeUnlocked;

    public delegate void OnRecipeSelected(RecipeSO recipe);
    public OnRecipeSelected onRecipeSelected;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!allRecipesAdded)
                UnlockAllRecipe();
        }
    }

    public void OpenCookingPanel() // open cooking panel
    {
        PlayerManager playerManager = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
        isCookingPanelActive = !isCookingPanelActive;
        if(isCookingPanelActive) // if cooking panel is active, player cant move
            playerManager.canMove = false;
        else
            playerManager.canMove = true;

        if(!allRecipesAdded) // add all recipes in game, for the sake of testing
            UnlockAllRecipe();
        onCookingPanelOpened?.Invoke();
    }

    public void UnlockAllRecipe() // unlock all recipes
    {
        availableRecipes.Clear();
        foreach(RecipeSO r in allRecipes)
        {
            availableRecipes.Add(r);
            onNewRecipeUnlocked?.Invoke(r);
        }
        allRecipesAdded = true;
    }

    public void UnlockRecipe(RecipeSO newRecipe) // unlock specific recipe
    {
        if(availableRecipes.Contains(newRecipe))
            return;
        availableRecipes.Add(newRecipe);
        onNewRecipeUnlocked?.Invoke(newRecipe);
    }

    public List<RecipeSO> GetAvailableRecipes()
    {
        return availableRecipes;
    }

    public void SelectRecipe(RecipeSO recipe) // select recipe from available ones
    {
        onRecipeSelected?.Invoke(recipe);
    }

    public void StartCooking(RecipeSO recipe) // start cooking the selected recipe
    {
        if(cookingRightNow != recipe) // first clear the previous recipe and ingridents
            ClearAddedIngridients();

        cookingRightNow = recipe;
        AddRequiredItems(cookingRightNow); // specify required items to cook
    }

    public void AddRequiredItems(RecipeSO recipe) // add required items
    {
        for(var i = 0; i < recipe.ingredients.Count; i++) // loop through req ingridents and add them
        {
            Item newItem = new Item(recipe.ingredients[i]);
            newItem.currentCount = recipe.ingredientsCount[i];
            requiredItems.Add(newItem);
        }
    }

    // Add Ingridient to cooking pod, return success if added
    public void AddIngridient(Item item, out bool success)
    {
        success = false;
        if(requiredItems.Count <= 0) // if not req items, leave function
            return;

        foreach(Item i in requiredItems) // loop through req items
        {
            if(i.itemData != item.itemData) // if current item not same the added one, proceed to next req irem
                continue;

            success = true;
            int itemIdx = -1;
            for(var j = 0; j < addedItems.Count; j++) // else, loop through added items
            {
                if(addedItems[j].itemData == item.itemData) // check if newly added item same as current added item
                    itemIdx = j;    // get its index
            }

            if(itemIdx == -1) // if default index isnt changed, meaning theres no this kind of item added previously
                addedItems.Add(item);   // add the new item to added items list
            else // else increase the current number of previously added item
                addedItems[itemIdx].currentCount++;
            break;
        }
    }

    // Check if the selected meal is finished, return result if finished
    public bool CheckIfCooked(out Item result)
    {
        result = null;
        int numberOfItemsMatched = 0;           // check if all required item, with their count,
        foreach(Item reqItem in requiredItems)  // are the same as the added items
        {
            foreach(Item addedItem in addedItems)
            {
                if(reqItem.itemData != addedItem.itemData)
                    continue;

                if(addedItem.currentCount == reqItem.currentCount)
                    numberOfItemsMatched++;
                Debug.Log(numberOfItemsMatched);
            }
        }

        if(numberOfItemsMatched != requiredItems.Count) // if doesnt match, then meal isnot cooked
            return false;
        else
        {
            result = new Item(cookingRightNow.result); // else meal is cooked
            cookingRightNow = null; // reset the selected recipe and required items;
            requiredItems.Clear();
            return true;
        }
    }

    public void ClearAddedIngridients()
    {
        requiredItems.Clear();
        addedItems.Clear();
    }
}
