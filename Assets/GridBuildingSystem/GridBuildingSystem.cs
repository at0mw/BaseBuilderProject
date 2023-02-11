using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private Transform testTransform;
    // I think for now I might do a top and side wall to save confusion
    [SerializeField] private Transform testTopWallTransform;
    [SerializeField] private Transform testSideWallTransform;
    private GridXZ<GridObject> _grid;

    private void Awake()
    {
        var gridWidth = 10;
        var gridHeight = 10;
        var cellSize = 10f;
        Debug.Log("Creating Grid");
        _grid = new GridXZ<GridObject>(gridWidth, gridWidth, cellSize, Vector3.zero,
            (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
    }



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Mouse3D.GetMouseWorldPosition();
            _grid.GetXZ(mousePosition, out int x, out int z);
            Debug.Log($"{x},{z}");
            var gridObject = _grid.GetGridObject(x, z);
            if (gridObject.CanBuildBuilding)
            {
                var buildingTransform = Instantiate(testTransform, _grid.GetWorldPosition(x, z), Quaternion.identity);
                gridObject.SetBuildingTransform(buildingTransform);
            }
            else
            {
                UtilsClass.CreateWorldTextPopup("Cannot Place a Building Here!!", mousePosition);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            var mousePosition = Mouse3D.GetMouseWorldPosition();
            _grid.GetPreciseXZ(mousePosition, out var preciseX, out var preciseZ);
            _grid.GetXZ(mousePosition, out var x, out var z);
            Debug.Log($"{x},{z}");
            var gridObject = _grid.GetGridObject(x, z);
            var closestEdge = gridObject.GetClosestEdgeDirection(mousePosition, _grid.GetWorldPosition(x, z), _grid.GetCellSize());
            // Use precise to see if mouse click is close to n/s/e/w edge
            
            Debug.Log(closestEdge);
            // TODO - Use Enums
            switch(closestEdge)
            {
                case "north":
                {
                    // Check for gridObject at x, z+1. If no wall object, place in both
                    var gridObjectAdjacent = _grid.GetGridObject(x, z+1);
                    if (gridObject.CanBuildNorthWall && gridObjectAdjacent.CanBuildSouthWall)
                    {
                        var wallTransform = Instantiate(testTopWallTransform, _grid.GetWorldPosition(x, z+1), Quaternion.identity);
                        gridObject.SetNorthTransform(wallTransform);
                        gridObjectAdjacent.SetSouthTransform(wallTransform);
                    }
                    else
                        UtilsClass.CreateWorldTextPopup("Cannot Place a Wall Here!!", mousePosition);
                    break;
                }
                case "east":
                {
                    // Check for gridObject at x+1, z. If out of grid bounds, place wall. If no wall object, place in both
                    var gridObjectAdjacent = _grid.GetGridObject(x+1, z);
                    if (gridObject.CanBuildEastWall && gridObjectAdjacent.CanBuildWestWall)
                    {
                        var wallTransform = Instantiate(testSideWallTransform, _grid.GetWorldPosition(x+1, z), Quaternion.identity);
                        gridObject.SetEastTransform(wallTransform);
                        gridObjectAdjacent.SetWestTransform(wallTransform);
                    }
                    else
                        UtilsClass.CreateWorldTextPopup("Cannot Place a Wall Here!!", mousePosition);
                    break;
                }
                case "south":
                {
                    // Check for gridObject at x, z-1. If no wall object, place in both
                    var gridObjectAdjacent = _grid.GetGridObject(x, z-1);
                    if (gridObject.CanBuildSouthWall && gridObjectAdjacent.CanBuildNorthWall)
                    {
                        var wallTransform = Instantiate(testTopWallTransform, _grid.GetWorldPosition(x, z), Quaternion.identity);
                        gridObject.SetSouthTransform(wallTransform);
                        gridObjectAdjacent.SetNorthTransform(wallTransform);
                    }
                    else
                        UtilsClass.CreateWorldTextPopup("Cannot Place a Wall Here!!", mousePosition);
                    break;
                }
                case "west":
                {
                    // Check for gridObject at x-1, z. If no wall object, place in both
                    var gridObjectAdjacent = _grid.GetGridObject(x-1, z);
                    if (gridObject.CanBuildWestWall && gridObjectAdjacent.CanBuildEastWall)
                    {
                        var wallTransform = Instantiate(testSideWallTransform, _grid.GetWorldPosition(x, z), Quaternion.identity);
                        gridObject.SetWestTransform(wallTransform);
                        gridObjectAdjacent.SetEastTransform(wallTransform);
                    }
                    else
                        UtilsClass.CreateWorldTextPopup("Cannot Place a Wall Here!!", mousePosition);
                    break;
                }
            }
        }
    }
}
