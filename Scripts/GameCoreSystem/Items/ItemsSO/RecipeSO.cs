using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "ScriptableObjects/NewRecipe")]
public class RecipeSO : ScriptableObject
{
    public List<ItemSO> ingredients = new List<ItemSO>();
    public List<int> ingredientsCount = new List<int>();

    public ItemSO result;
}
