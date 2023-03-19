using System.Diagnostics;
using BuildingSystem.Enums;
using BuildingSystem.Grid;
using BuildingSystem.Scripts;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace BuildingSystem {
    public class GridBuildingSystem : MonoBehaviour {
        [SerializeField] private PlacedBuildingType placedBuildingType;
        private GameObject _ghostObject;
        [SerializeField] private Transform buildingParent;
        [SerializeField] private Transform gridParent;
        [SerializeField] private Transform player;

        // I think for now I might do a top and side wall to save confusion
        // TODO - Make placedEdgeBuildingType Scriptable Object
        [SerializeField] private Transform edgeBuilding;

        [SerializeField] private int gridWidth;
        [SerializeField] private int gridHeight;
        [SerializeField] private float cellSize;
        private GridXZ<GridObject> _grid;

        private bool _buildMode;
        private bool _destroyMode;

        private void Awake() {
            _grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero,
                (GridXZ<GridObject> g_, int x_, int z_) => new GridObject(g_, x_, z_), gridParent);
        }


        private void Update() {
            // TODO - If Build Tool is active
            // Observer Pattern
            if(_buildMode)
                Construct();
            else if(_destroyMode)
                Deconstruct();
            // TODO - If Destroy Tool is active
            //Destroy();
        }

        private void Construct() {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (Input.GetMouseButtonDown(0)) {
                InsideBuild();
            }
            else if (Input.GetMouseButtonDown(1)) {
                EdgeBuild();
            }
            else {
                GhostBuild();
            }
        }
        
        private void Deconstruct() {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (Input.GetMouseButtonDown(0)) {
                DeleteBuild();
            }            
            else if (Input.GetMouseButtonDown(1)) {
                DeleteEdgeBuild();
            }
            else {
                GhostBuild();
            }
        }

        private void DeleteBuild() {
            var mousePosition = Mouse3D.GetMouseWorldPosition();
            _grid.GetXZ(mousePosition, out int x, out int z);
            if (!IsPlayerWithinRange(_grid.GetWorldPosition(x, z))) return;

            var gridObject = _grid.GetGridObject(x, z);
            if (gridObject.CanBuildBuilding) return;
            gridObject.ClearBuildingTransform();
        }
        
        private void DeleteEdgeBuild() {
            var mousePosition = Mouse3D.GetMouseWorldPosition();
            _grid.GetPreciseXZ(mousePosition, out var preciseX, out var preciseZ);
            _grid.GetXZ(mousePosition, out var x, out var z);
            if (!IsPlayerWithinRange(_grid.GetWorldPosition(x, z))) {
                UtilsClass.CreateWorldTextPopup("Player out of range!!", mousePosition);
                return;
            }
            var gridObject = _grid.GetGridObject(x, z);
            var closestEdge =
                GridObject.GetClosestEdgeDirection(mousePosition, _grid.GetWorldPosition(x, z), _grid.GetCellSize());
            DeleteWalls(closestEdge, x, z, gridObject);
        }

        private void DeleteWalls(GridDirection closestEdge_, int x_, int z_, GridObject gridObject_) {
            var gridObjectAdjacent = AdjacentDirection(closestEdge_, x_, z_, out var adjacentDirection);
            
            if (gridObjectAdjacent is null) {
                ClearWall(closestEdge_, gridObject_);
            }
            else {
                ClearWall(closestEdge_, gridObject_);
                ClearWall(adjacentDirection, gridObjectAdjacent);
            }
        }

        private static void ClearWall(GridDirection closestEdge_, GridObject gridObject_) {
            if (!gridObject_.CanBuildWall(closestEdge_))
                gridObject_.DeleteWall(closestEdge_);
        }

        private bool IsPlayerInLocation(Vector3 worldPosition_) {
            return _grid.IsInGridSpace(player.transform.position, worldPosition_);
        }
        
        private bool IsPlayerWithinRange(Vector3 worldPosition_) {
            return _grid.IsWithinPlayerRange(player.transform.position, worldPosition_);
        }

        private void InsideBuild() {
            var mousePosition = Mouse3D.GetMouseWorldPosition();
            _grid.GetXZ(mousePosition, out int x, out int z);
            if (IsPlayerInLocation(_grid.GetWorldPosition(x, z))) {
                UtilsClass.CreateWorldTextPopup("Cannot Build Here, Player is Present!!", mousePosition);
                return;
            }
            
            if (!IsPlayerWithinRange(_grid.GetWorldPosition(x, z))) {
                UtilsClass.CreateWorldTextPopup("Player out of range!!", mousePosition);
                return;
            }

            var gridPositionList = placedBuildingType.GetGridPositionList(new Vector2Int(x, z));
            
            var gridObject = _grid.GetGridObject(x, z);
            if (gridObject.CanBuildBuilding) {
                var buildingTransform = Instantiate(placedBuildingType.prefab, _grid.GetWorldPosition(x, z),
                    Quaternion.identity);
                buildingTransform.parent = buildingParent;
                buildingTransform.GetComponent<GhostBuilding>().ActivateMeshRenderers();
                foreach (var gridPosition in gridPositionList) {
                    _grid.GetGridObject(gridPosition.x, gridPosition.y).SetBuildingTransform(buildingTransform);
                }
                //gridObject.SetBuildingTransform(buildingTransform);
            }
            else {
                UtilsClass.CreateWorldTextPopup("Cannot Place a Building Here!!", mousePosition);
            }
        }

        private void GhostBuild() {
            var mousePosition = Mouse3D.GetMouseWorldPosition();
            _grid.GetXZ(mousePosition, out int x, out int z);

            if (_ghostObject is null) {
                _ghostObject = Instantiate(placedBuildingType.prefab, _grid.GetWorldPosition(x, z),
                    Quaternion.identity).gameObject;
                _ghostObject.GetComponent<GhostBuilding>()?.DeactivateMeshRenderers();
            }

            if (IsPlayerInLocation(_grid.GetWorldPosition(x, z))) {
                _ghostObject.GetComponent<GhostBuilding>().ChangeToUnsafe();
            }
            else if (!IsPlayerWithinRange(_grid.GetWorldPosition(x, z))){
                // Set Material to can Place
                _ghostObject.GetComponent<GhostBuilding>().ChangeToTooFar();
            }
            else {
                _ghostObject.GetComponent<GhostBuilding>().ChangeToSafe();
            }
            
            var gridObject = _grid.GetGridObject(x, z);
            if (gridObject is not null && gridObject.CanBuildBuilding) {
                _ghostObject.transform.SetPositionAndRotation(_grid.GetWorldPosition(x, z), Quaternion.identity);
            }
        }

        private void EdgeBuild() {
            var mousePosition = Mouse3D.GetMouseWorldPosition();
            _grid.GetPreciseXZ(mousePosition, out var preciseX, out var preciseZ);
            _grid.GetXZ(mousePosition, out var x, out var z);
            if (!IsPlayerWithinRange(_grid.GetWorldPosition(x, z))) {
                UtilsClass.CreateWorldTextPopup("Player out of range!!", mousePosition);
                return;
            }
            var gridObject = _grid.GetGridObject(x, z);
            var closestEdge =
                GridObject.GetClosestEdgeDirection(mousePosition, _grid.GetWorldPosition(x, z), _grid.GetCellSize());
            PlaceWalls(closestEdge, x, z, gridObject);
        }

        private void PlaceWalls(GridDirection closestEdge_, int x_, int z_, GridObject gridObject_) {
            var gridObjectAdjacent = AdjacentDirection(closestEdge_, x_, z_, out var adjacentDirection);
            if (gridObjectAdjacent is null) {
                if (!gridObject_.CanBuildWall(closestEdge_)) return;
                var wallTransform = Instantiate(edgeBuilding,
                    _grid.GetWorldPosition(x_ + closestEdge_.GetPlacementX(), z_ + closestEdge_.GetPlacementZ()),
                    closestEdge_.IsHorizontal() ? Quaternion.Euler(0, -90, 0) : Quaternion.identity);
                gridObject_.SetWallTransform(closestEdge_, wallTransform);
            }
            else {
                if (!gridObject_.CanBuildWall(closestEdge_) || !gridObjectAdjacent.CanBuildWall(adjacentDirection)) return;
                var wallTransform = Instantiate(edgeBuilding,
                    _grid.GetWorldPosition(x_ + closestEdge_.GetPlacementX(), z_ + closestEdge_.GetPlacementZ()),
                    closestEdge_.IsHorizontal() ? Quaternion.Euler(0,-90, 0) : Quaternion.identity);
                gridObject_.SetWallTransform(closestEdge_, wallTransform);
                gridObjectAdjacent.SetWallTransform(adjacentDirection, wallTransform);
            }
        }

        private GridObject AdjacentDirection(GridDirection closestEdge_, int x_, int z_, out GridDirection adjacentDirection_) {
            adjacentDirection_ = closestEdge_.GetOpposite();
            var gridObjectAdjacent =
                _grid.GetGridObject(x_ + closestEdge_.GetAdjacentX(), z_ + closestEdge_.GetAdjacentZ());
            return gridObjectAdjacent;
        }

        public void SetBuildMode() {
            Debug.Log($"Toggling Build Mode");
            _destroyMode = false;
            _buildMode = !_buildMode;
            if (_buildMode) return;
            
            Debug.Log($"Hiding Ghost Render");
            // Deactivate Ghost Renderer
            _ghostObject.GetComponent<DeleteObject>().Delete();
            _ghostObject = null;
        }
        
        public void SetDestroyMode() {
            Debug.Log($"Toggling Destroy Mode");
            _destroyMode = !_destroyMode;
            _buildMode = false;
            if (_destroyMode) return;
            
            Debug.Log($"Hiding Ghost Render");
            // Deactivate Ghost Renderer
            _ghostObject.GetComponent<DeleteObject>().Delete();
            _ghostObject = null;
        }
    }
}