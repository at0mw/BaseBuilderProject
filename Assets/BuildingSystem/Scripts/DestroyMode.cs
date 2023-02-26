using System.Collections;
using System.Collections.Generic;
using BuildingSystem;
using UnityEngine;

public class DestroyMode : MonoBehaviour
{
    [SerializeField]private GameObject gridBuildingSystem;
    // TODO - Needs to be relayed back from GridBuildingSystem?
    private bool _currentState;
    private void Start()
    {
        var destroyButton = GetComponent<UnityEngine.UI.Button>();
        destroyButton.onClick.AddListener(ToggleDestroyMode);
    }

    private void ToggleDestroyMode()
    {
        Debug.Log("Destroy Mode Script Running");
        _currentState = !_currentState;
        gridBuildingSystem.GetComponent<GridBuildingSystem>().SetDestroyMode(_currentState);
    }
}
