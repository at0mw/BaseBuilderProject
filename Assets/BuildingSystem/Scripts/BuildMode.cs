using System.Collections;
using System.Collections.Generic;
using BuildingSystem;
using UnityEngine;

public class BuildMode : MonoBehaviour
{
    [SerializeField]private GameObject gridBuildingSystem;
    private bool _currentState;
    private void Start()
    {
        var buildButton = GetComponent<UnityEngine.UI.Button>();
        buildButton.onClick.AddListener(ToggleBuildMode);
    }

    private void ToggleBuildMode()
    {
        Debug.Log("Build Mode Script Running");
        _currentState = !_currentState;
        gridBuildingSystem.GetComponent<GridBuildingSystem>().SetBuildMode(_currentState);
    }
}
