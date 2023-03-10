using UnityEngine;

namespace BuildingSystem.Scripts {
    public class CameraFollow : MonoBehaviour {
        public Transform target;

        public float smoothSpeed = 0.125f;
        public Vector3 offset;

        private void LateUpdate() {
            transform.position = target.position + offset;
        }
    }
}