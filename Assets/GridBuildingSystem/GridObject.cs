using UnityEngine;

public class GridObject
{
    private readonly GridXZ<GridObject> _gridXZ;
    private readonly int _x;
    private readonly int _z;
    private Transform _buildingTransform;
    private Transform _northWallTransform;
    private Transform _southWallTransform;
    private Transform _eastWallTransform;
    private Transform _westWallTransform;

    public bool CanBuildBuilding => _buildingTransform == null;
    public bool CanBuildNorthWall => _northWallTransform == null;
    public bool CanBuildSouthWall => _southWallTransform == null;
    public bool CanBuildEastWall => _eastWallTransform == null;
    public bool CanBuildWestWall => _westWallTransform == null;

    public GridObject(GridXZ<GridObject> grid, int x, int z)
    {
        _gridXZ = grid;
        _x = x;
        _z = z;
    }

    public void SetBuildingTransform(Transform transform)
    {
        _buildingTransform = transform;
        _gridXZ.TriggerGridObjectChanged(_x, _z);
    }
    // TODO - Less Methods, more switches
    public void SetNorthTransform(Transform transform)
    {
        _northWallTransform = transform;
        _gridXZ.TriggerGridObjectChanged(_x, _z);
    }
    public void SetSouthTransform(Transform transform)
    {
        _southWallTransform = transform;
        _gridXZ.TriggerGridObjectChanged(_x, _z);
    }
    public void SetEastTransform(Transform transform)
    {
        _eastWallTransform = transform;
        _gridXZ.TriggerGridObjectChanged(_x, _z);
    }
    public void SetWestTransform(Transform transform)
    {
        _westWallTransform = transform;
        _gridXZ.TriggerGridObjectChanged(_x, _z);
    }
    
    public void ClearBuildingTransform()
    {
        _buildingTransform = null;
    }
    
    public string GetClosestEdgeDirection(Vector3 worldPosition, Vector3 squareCorner, float squareSize) 
    {
        Vector3 squareCenter = squareCorner + new Vector3(squareSize * 0.5f, 0, squareSize * 0.5f);
        Vector3 relativePosition = worldPosition - squareCenter;
        float halfSquareSize = squareSize * 0.5f;

        if (Mathf.Abs(relativePosition.x) > Mathf.Abs(relativePosition.z))
        {
            // Closest to left/right edges
            return relativePosition.x > 0 ? "east" : "west";
        }


        // Closest to top/bottom edges
        return relativePosition.z > 0 ? "north" : "south";
    }
    
    public override string ToString()
    {
        return $"{_x},{_z}\n {_buildingTransform}";
    }
}
