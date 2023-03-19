using UnityEngine;

namespace BuildingSystem.Scripts {
    public class Mouse3D : MonoBehaviour {
        [SerializeField] private LayerMask mouseColliderLayerMask;

        public static Mouse3D Instance { get; private set; }

        private void Awake() {
            Instance = this;
        }

        private void Update() {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var raycastHit, 999f, mouseColliderLayerMask))
                transform.position = raycastHit.point;
        }

        public static Vector3 GetMouseWorldPosition() {
            return Instance.GetMouseWorldPosition_Instance();
        }

        private Vector3 GetMouseWorldPosition_Instance() {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var raycastHit, 999f, mouseColliderLayerMask))
                return raycastHit.point;
            return Vector3.zero;
        }
    }
}