using System;
using BuildingSystem.Scripts.Interfaces;
using UnityEngine;

namespace BuildingSystem.Scripts.BuildingSystem {
    public class BuildManager : MonoBehaviour, IUIObserver
    {
        [SerializeField]private GameObject gridBuildingSystem;
        

        // Responds to messages received from UI Manager notifying of deconstruct or construct mode
        // button presses.
        public void NotifyUpdate(UpdateMessage message_) {
            Debug.Log($"In Build Manager {message_.ButtonType}");
            // Inform GridBuildingSystem
            switch (message_.ButtonType) {
                case ButtonType.ConstructMode:
                    gridBuildingSystem.GetComponent<GridBuildingSystem>().SetBuildMode();
                    break;
                case ButtonType.DeconstructMode:
                    gridBuildingSystem.GetComponent<GridBuildingSystem>().SetDestroyMode();
                    break;
                default:
#if UNITY_EDITOR
                    Debug.LogWarning(
                        $"A notify message has been received with unexpected ButtonType {message_.ButtonType}");       
#endif
                    break;
            }
        }
    }
}