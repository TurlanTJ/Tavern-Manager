using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Grid
{
    private float _cellSize;
    private Vector3 _gridOrigin;
    private int _width;
    private int _height;
    private GridObject[,] _gridArray;

    public Grid(int width, int height, float cellSize, Vector3 gridOrigin)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        _gridOrigin = gridOrigin;
        _gridArray = new GridObject[width, height];

        bool showGrids = false;

        if(showGrids)
        {
            for(int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for(int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x + 1, y), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPos(0, _height), GetWorldPos(_width, _height), Color.white, 100f);
            Debug.DrawLine(GetWorldPos(_width, 0), GetWorldPos(_width, _height), Color.white, 100f);
        }
    }

    private Vector3 GetWorldPos(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _gridOrigin;
    }

    private Vector2Int GetXYPos(Vector3 worldPos)
    {
        Vector2Int res = new Vector2Int
        {
            x = Mathf.FloorToInt((worldPos - _gridOrigin).x / _cellSize),
            y = Mathf.FloorToInt((worldPos - _gridOrigin).y / _cellSize)
        };

        return res;
    }

    private bool IsEmpty(Vector2Int XYPos)
    {
        GridObject gridObject = _gridArray[XYPos.x, XYPos.y];

        if(gridObject.GetStoredBuildingsList(out IBuilding[] list) <= 0)
            return true;

        return false;
    }

    public IBuilding[] GetGridObject(Vector3 mousePos)
    {
        Vector2Int xyPos = GetXYPos(mousePos);

        if(!IsEmpty(xyPos)) {
            Debug.Log($"{_gridArray[xyPos.x, xyPos.y]} is located at Grid[{xyPos.x}, {xyPos.y}]");
            IBuilding[] buildingsList;
            if(_gridArray[xyPos.x, xyPos.y].GetStoredBuildingsList(out buildingsList) <= 0)
                return null;
            return buildingsList;
        }
        return null;
    }

    public void SetGridObject(int x, int y, IBuilding building)
    {
        if(x >= 0 && x < _width && y >= 0 && y < _height){
            _gridArray[x, y].AddBuildingToStoredBuildingsList(building);
            Debug.Log($"{building} is set to Grid[{x}, {y}]");
        }      
    }

    public void SetGridObject(Vector3 mousePos, IBuilding building)
    {
        Vector2Int xyPos = GetXYPos(mousePos);

        SetGridObject(xyPos.x, xyPos.y, building);
    }
}


