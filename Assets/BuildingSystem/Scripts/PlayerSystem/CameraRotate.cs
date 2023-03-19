using UnityEngine;
using UnityEngine.Serialization;

namespace BuildingSystem.Scripts.PlayerSystem {
    public class CameraRotate : MonoBehaviour {
        [FormerlySerializedAs("_nextRotation")] [SerializeField] private float nextRotation;
        // [SerializeField] private float rotationSpeed = 30f;

        private void Start() {
            nextRotation = transform.rotation.y;
        }

        private void LateUpdate() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                nextRotation = (nextRotation + 45);
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                nextRotation = (nextRotation - 45);
            }

            RotateCamera();
        }

        private void RotateCamera() {
            var rotation = transform.rotation.eulerAngles;
            rotation.y = nextRotation;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}