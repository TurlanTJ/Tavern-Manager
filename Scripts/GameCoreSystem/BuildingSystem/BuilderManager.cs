using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

    //  NOTE: BUILDING SYSTEM IS NOT BEING USED
    //  AS I COULD NOT MAKE IT WORK PROPERLY

public class BuilderManager : MonoBehaviour
{
    public static BuilderManager instance;
    void Awake()
    {
        if(instance != null)
            return;
        instance = this;
    }

    private MapManager _mapManager = MapManager.instance;

    [SerializeField] private List<IBuilding> _acquiredBuildings = new List<IBuilding>();
    [SerializeField] private List<IBuilding> _availableBuildings = new List<IBuilding>();

    public GameObject selectedBuilding { get; private set; }

    public void UnlockBuilding(int itemOrder) // unlock the selected building
    {
        IBuilding selectedBuilding = _availableBuildings[itemOrder];

        if(_acquiredBuildings.Contains(selectedBuilding))
            return;
        
        _acquiredBuildings.Add(selectedBuilding); // the building is NOT acquired then add it to acquired list
    }

    public void SetBuildingSts()
    {
        
    }

    public bool IsAvailable(IBuilding building) // building can be selected
    {
        if(_acquiredBuildings.Contains(building))
            return true;

        return false;
    }

    public void PrepareToBuild(IBuilding building) // enter builiding mode 
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // select place a to build
        mousePos.z = 0f;

        if(CanBuild(building)) // if can build then select a blueprint of building
            selectedBuilding = building.blueprint;        
    }

    public void CancelBuildingBlueprint() // cancel blueprint placement
    {
        selectedBuilding = null;
    }

    public void Build(IBuilding building, Vector3 mousePos) // build builing at given pos
    {
        if(!CanBuild(building, mousePos))
            return;

        _mapManager._grid.SetGridObject(mousePos, building); // set grid to
        selectedBuilding = null;
    }

    public bool CanBuild(IBuilding building)
    {
        if(IsAvailable(building))
        {
            //if(_playerManager._cash >= building.costToBuild)
                //return true;
        }


        return false;
    }

    public bool CanBuild(IBuilding building, Vector3 mousePos)
    {
        if(IsAvailable(building))
        {
            //if(_playerManager._cash >= building.costToBuild)
                //return true;
        }

        return false;
    }

    public bool CanBuild(IBuilding building, int posX, int posY)
    {
        return true;
    }
}
