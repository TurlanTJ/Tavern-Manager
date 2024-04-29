using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridObject
{

    private IBuilding[] storedBuildings = new IBuilding[4];

    public bool _isWalkable { get; private set; } = true;

    public void AddBuildingToStoredBuildingsList(IBuilding newBuilding)
    {
        int buildingIdx = newBuilding.buildingPlacementOrder;
        if(storedBuildings[buildingIdx] != null)
        {
            return;
        }
        
        storedBuildings[buildingIdx] = newBuilding;

        if(buildingIdx >= 2)
            SetTileWalkableStatus(false);
    }

    public IBuilding GetBuilding(int buildingIndex)
    {
        if(buildingIndex > storedBuildings.Length)
            return null;
        
        return storedBuildings[buildingIndex];
    }

    public int GetStoredBuildingsList(out IBuilding[] list)
    {
        
        int listCount = storedBuildings.Length;
        list = storedBuildings;
        
        if(listCount <= 0)
            list = null;

        return listCount;
    }

    public void SetTileWalkableStatus(bool sts)
    {
        _isWalkable = sts;
    }
}
