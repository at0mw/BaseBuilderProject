using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoundaries : MonoBehaviour {
    [SerializeField] private int maxGridSizeX = 1000;
    [SerializeField] private int minGridSizeX = 0;
    [SerializeField] private int maxGridSizeZ = 1000;
    [SerializeField] private int minGridSizeZ = 0;

    // Update is called once per frame
    private void LateUpdate() {
        var transformPosition = transform.position;
        transformPosition.x = Mathf.Clamp(transformPosition.x, minGridSizeX, maxGridSizeX);
        transformPosition.z = Mathf.Clamp(transformPosition.z, minGridSizeZ, maxGridSizeZ);
        transform.position = transformPosition;
    }
}
