using UnityEngine;

namespace BuildingSystem.Scripts {
    public class DeleteObject : MonoBehaviour {
        public void Delete() {
            Destroy(gameObject);
        }
    }
}