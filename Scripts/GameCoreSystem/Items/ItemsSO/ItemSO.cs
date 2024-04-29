using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/NewItem")]
public class ItemSO : ScriptableObject
{
    public int itemId;
    public string itemName;
    public int itemPrice;
    public bool isEdible;

    public ItemType itemType;

    public Sprite itemIcon;
    public GameObject itemPrefab;
}

public enum ItemType
{
    Ingridient,
    Meal,
    Drink,
    Furniture,
}
