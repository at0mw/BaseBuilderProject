using System;
using BuildingSystem.Enums;
using BuildingSystem.Grid;
using BuildingSystem.Scripts;
using JetBrains.Annotations;
using UnityEngine;

namespace BuildingSystem {
    public class GridObject {
        private readonly GridXZ<GridObject> _gridXZ;
        private readonly int _x;
        private readonly int _z;
        private Transform _buildingTransform;
        private Transform _eastWallTransform;
        private Transform _northWallTransform;
        private Transform _southWallTransform;
        private Transform _westWallTransform;

        public GridObject([NotNull] GridXZ<GridObject> grid_, int x_, int z_) {
            _gridXZ = grid_ ?? throw new ArgumentNullException(nameof(grid_));
            _x = x_;
            _z = z_;
        }

        public bool CanBuildBuilding => _buildingTransform == null;
        public bool CanBuildNorthWall => _northWallTransform == null;
        public bool CanBuildSouthWall => _southWallTransform == null;
        public bool CanBuildEastWall => _eastWallTransform == null;
        public bool CanBuildWestWall => _westWallTransform == null;

        public bool CanBuildWall(GridDirection gridDirection_) {
            return gridDirection_ switch {
                GridDirection.North => _northWallTransform is null,
                GridDirection.South => _southWallTransform is null,
                GridDirection.East => _eastWallTransform is null,
                GridDirection.West => _westWallTransform is null,
                _ => false
            };
        }

        public void SetBuildingTransform(Transform transform_) {
            _buildingTransform = transform_;
            _gridXZ.TriggerGridObjectChanged(_x, _z);
        }

        public void SetWallTransform(GridDirection direction_, Transform transform_) {
            switch (direction_) {
                case GridDirection.North: {
                    _northWallTransform = transform_;
                    break;
                }
                case GridDirection.South: {
                    _southWallTransform = transform_;
                    break;
                }
                case GridDirection.East: {
                    _eastWallTransform = transform_;
                    break;
                }
                case GridDirection.West: {
                    _westWallTransform = transform_;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction_), direction_, null);
            }

            _gridXZ.TriggerGridObjectChanged(_x, _z);
        }

        public void ClearBuildingTransform() {
            //_buildingTransform.gameObject.
            _buildingTransform.GetComponent<DeleteObject>().Delete();
            _buildingTransform = null;
        }

        public static GridDirection GetClosestEdgeDirection(Vector3 worldPosition_, Vector3 squareCorner_,
            float squareSize_) {
            var accountedVector = worldPosition_ - squareCorner_;
            var squareCenter = squareCorner_ + new Vector3(squareSize_ * 0.5f, 0, squareSize_ * 0.5f);
            var relativePosition = worldPosition_ - squareCenter;

            if (Mathf.Abs(relativePosition.x) > Mathf.Abs(relativePosition.z))
                // Closest to left/right edges
                return relativePosition.x > 0 ? GridDirection.East : GridDirection.West;
            // Closest to top/bottom edges
            return relativePosition.z > 0 ? GridDirection.North : GridDirection.South;
        }

        public override string ToString() {
            return $"{_x},{_z}\n {_buildingTransform}";
        }

        public void DeleteWall(GridDirection closestEdge_) {
            switch (closestEdge_) {
                case GridDirection.North: {
                    _northWallTransform.GetComponent<DeleteObject>().Delete();
                    _northWallTransform = null;
                    break;
                }
                case GridDirection.South: {
                    _southWallTransform.GetComponent<DeleteObject>().Delete();
                    _southWallTransform = null;
                    break;
                }
                case GridDirection.East: {
                    _eastWallTransform.GetComponent<DeleteObject>().Delete();
                    _eastWallTransform = null;
                    break;
                }
                case GridDirection.West: {
                    _westWallTransform.GetComponent<DeleteObject>().Delete();
                    _westWallTransform = null;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(closestEdge_), closestEdge_, null);
            }

            _gridXZ.TriggerGridObjectChanged(_x, _z);
        }
    }
}