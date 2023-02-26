using System;
using CodeMonkey.Utils;
using UnityEngine;

namespace BuildingSystem.Grid {
    public class GridXZ<TGridObject> {
        private readonly float _cellSize;
        private readonly TGridObject[,] _gridArray;
        private readonly int _height;
        private readonly Vector3 _originPosition;
        // TODO shift this to be serialisable and set in gridbuildingsystem
        private readonly int buildDistance = 1;

        private readonly int _width;

        public GridXZ(int width_, int height_, float cellSize_, Vector3 originPosition_,
            Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject_, Transform gridParent_) {
            _width = width_;
            _height = height_;
            _cellSize = cellSize_;
            _originPosition = originPosition_;

            _gridArray = new TGridObject[width_, height_];

            for (var x = 0; x < _gridArray.GetLength(0); x++) {
                for (var z = 0; z < _gridArray.GetLength(1); z++) {
                    _gridArray[x, z] = createGridObject_(this, x, z);
                }
            }
        }

        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

        public int GetWidth() {
            return _width;
        }

        public int GetHeight() {
            return _height;
        }

        public float GetCellSize() {
            return _cellSize;
        }

        public Vector3 GetWorldPosition(int x_, int z_) {
            return new Vector3(x_, 0, z_) * _cellSize + _originPosition;
        }

        public bool IsInGridSpace(Vector3 playerPosition_, Vector3 gridPosition_) {
            var accountX = playerPosition_.x - gridPosition_.x;
            var accountZ = playerPosition_.z - gridPosition_.z;

            if (!(accountX <= _cellSize) || !(accountX >= 0)) return false;
            return accountZ <= _cellSize && accountZ >= 0;
        }
        
        public bool IsWithinPlayerRange(Vector3 playerPosition_, Vector3 gridPosition_) {
            GetXZ(playerPosition_, out var playerX, out var playerZ);
            GetXZ(gridPosition_, out var gridX, out var gridZ);
            var adjustedX = Mathf.Abs(playerX - gridX);
            var adjustedZ = Mathf.Abs(playerZ - gridZ);
            return adjustedX <= buildDistance && adjustedZ <= buildDistance;
        }

        public void GetXZ(Vector3 worldPosition_, out int x_, out int z_) {
            x_ = Mathf.FloorToInt((worldPosition_ - _originPosition).x / _cellSize);
            z_ = Mathf.FloorToInt((worldPosition_ - _originPosition).z / _cellSize);
        }

        public void GetPreciseXZ(Vector3 worldPosition_, out float x_, out float z_) {
            x_ = (worldPosition_ - _originPosition).x / _cellSize;
            z_ = (worldPosition_ - _originPosition).z / _cellSize;
        }

        public void SetGridObject(int x_, int z_, TGridObject value_) {
            if (x_ >= 0 && z_ >= 0 && x_ < _width && z_ < _height) {
                _gridArray[x_, z_] = value_;
                TriggerGridObjectChanged(x_, z_);
            }
        }

        public void TriggerGridObjectChanged(int x_, int z_) {
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { X = x_, Z = z_ });
        }

        public void SetGridObject(Vector3 worldPosition_, TGridObject value_) {
            GetXZ(worldPosition_, out var x, out var z);
            SetGridObject(x, z, value_);
        }

        public TGridObject GetGridObject(int x_, int z_) {
            if (x_ >= 0 && z_ >= 0 && x_ < _width && z_ < _height) {
                return _gridArray[x_, z_];
            }
            return default;
        }

        public TGridObject GetGridObject(Vector3 worldPosition_) {
            GetXZ(worldPosition_, out var x, out var z);
            return GetGridObject(x, z);
        }

        public Vector2Int ValidateGridPosition(Vector2Int gridPosition_) {
            return new Vector2Int(
                Mathf.Clamp(gridPosition_.x, 0, _width - 1),
                Mathf.Clamp(gridPosition_.y, 0, _height - 1)
            );
        }

        public bool IsValidGridPosition(Vector2Int gridPosition_) {
            int x = gridPosition_.x;
            int z = gridPosition_.y;

            if (x >= 0 && z >= 0 && x < _width && z < _height) {
                return true;
            }
            else {
                return false;
            }
        }

        public bool IsValidGridPositionWithPadding(Vector2Int gridPosition_) {
            var padding = new Vector2Int(2, 2);
            var x = gridPosition_.x;
            var z = gridPosition_.y;

            return x >= padding.x && z >= padding.y && x < _width - padding.x && z < _height - padding.y;
        }

        public class OnGridObjectChangedEventArgs : EventArgs {
            public int X;
            public int Z;
        }


    }
}