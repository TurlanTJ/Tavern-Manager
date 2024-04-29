using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IBuilding
{
    public string itemID { get; private set; }
    public string name;
    public float hp;
    public int costToBuild;
    public int costToAcquire;
    public int buildingPlacementOrder;

    public GameObject building;
    public GameObject blueprint;
    public Image icon;
}
