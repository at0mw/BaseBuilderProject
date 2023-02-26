using UnityEngine;

namespace BuildingSystem.Scripts {
    public class CameraRotate : MonoBehaviour {
        [SerializeField] private float _nextRotation;
        [SerializeField] private float rotationSpeed = 30f;

        private void Start() {
            _nextRotation = transform.rotation.y;
        }

        private void LateUpdate() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                _nextRotation = (_nextRotation - 45);
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                _nextRotation = (_nextRotation + 45);
            }

            RotateCamera();
        }

        private void RotateCamera() {
            var rotation = transform.rotation.eulerAngles;
            rotation.y = _nextRotation;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}