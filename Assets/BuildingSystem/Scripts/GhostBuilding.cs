using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem.Scripts {
    public class GhostBuilding : MonoBehaviour {
        public MeshRenderer[] meshRenderers;
        public MeshRenderer ghostRenderer;
        public BoxCollider[] boxColliders;
        public Transform area;
        //public Transform building;

        public Material safeToPlace;
        public Material unsafeToPlace;
        public Material tooFarToPlace;
        // public Material ghost;
        // public Material real;

        [ContextMenu("GetBoxColliders")]
        private void GetBoxColliders() {
            var boxCollider = new List<BoxCollider>();
            boxCollider.AddRange(GetComponentsInChildren<BoxCollider>());
            boxColliders = boxCollider.ToArray();
        }

        [ContextMenu("GetMeshRenderers")]
        private void GetMeshRenderers() {
            var meshRenderer = new List<MeshRenderer>();
            meshRenderer.AddRange(GetComponentsInChildren<MeshRenderer>());
            meshRenderers = meshRenderer.ToArray();
        }

        [ContextMenu("DeactivateMeshRenderers")]
        public void DeactivateMeshRenderers() {
            foreach (var meshRenderer in meshRenderers) {
                meshRenderer.enabled = false;
            }
            foreach (var boxCollider in boxColliders) {
                boxCollider.enabled = false;
            }
            ghostRenderer.enabled = true;
        }

        [ContextMenu("ActivateMeshRenderers")]
        public void ActivateMeshRenderers() {
            foreach (var meshRenderer in meshRenderers) {
                meshRenderer.enabled = true;
            }

            foreach (var boxCollider in boxColliders) {
                boxCollider.enabled = true;
            }

            ghostRenderer.enabled = false;
        }
        //
        // public void ChangeToGhost() {
        //     building.GetComponent<Renderer>().material = ghost;
        // }
        //
        // public void ChangeToReal() {
        //     building.GetComponent<Renderer>().material = real;
        // }


        public void ChangeToUnsafe() {
            area.GetComponent<Renderer>().material = unsafeToPlace;
        }
        
        public void ChangeToTooFar() {
            area.GetComponent<Renderer>().material = tooFarToPlace;
        }
        
        public void ChangeToSafe() {
            area.GetComponent<Renderer>().material = safeToPlace;
        }
    }
}