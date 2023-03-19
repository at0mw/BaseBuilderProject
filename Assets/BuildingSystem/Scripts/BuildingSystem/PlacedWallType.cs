using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace BuildingSystem.Scripts {
    [CreateAssetMenu]
    public class PlacedWallType : ScriptableObject {
        public string stringName;
        public Transform prefab;
        public Transform visual;
        public int width;
        public int height;

        // public int GetRotationAngle(Dir dir)
        // {
        //     return int =;
        // }

        // Depends on rotation of wall in its grid position
        public List<Vector2Int> GetGridPositionList(Vector2Int offset) {
            var gridPositionList = new List<Vector2Int>();
            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    gridPositionList.Add(offset + new Vector2Int(x, y));
                }
            }

            return gridPositionList;
        }
    }
}