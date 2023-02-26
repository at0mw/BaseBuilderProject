using System;
using BuildingSystem.Enums;

namespace BuildingSystem.Scripts {
    public static class GridDirectionExtensions {
        public static GridDirection GetOpposite(this GridDirection direction_) {
            return direction_ switch {
                GridDirection.North => GridDirection.South,
                GridDirection.East => GridDirection.West,
                GridDirection.South => GridDirection.North,
                GridDirection.West => GridDirection.East,
                _ => throw new ArgumentException("Invalid direction.")
            };
        }

        public static bool IsHorizontal(this GridDirection direction_) {
            return direction_ == GridDirection.East || direction_ == GridDirection.West;
        }

        public static int GetAdjacentX(this GridDirection direction_) {
            return direction_ switch {
                GridDirection.North => 0,
                GridDirection.East => 1,
                GridDirection.South => 0,
                GridDirection.West => -1,
                _ => throw new ArgumentException("Invalid direction.")
            };
        }

        public static int GetAdjacentZ(this GridDirection direction_) {
            return direction_ switch {
                GridDirection.North => 1,
                GridDirection.East => 0,
                GridDirection.South => -1,
                GridDirection.West => 0,
                _ => throw new ArgumentException("Invalid direction.")
            };
        }

        public static int GetPlacementX(this GridDirection direction_) {
            return direction_ == GridDirection.East ? 1 : 0;
        }

        public static int GetPlacementZ(this GridDirection direction_) {
            return direction_ == GridDirection.North ? 1 : 0;
        }
    }
}