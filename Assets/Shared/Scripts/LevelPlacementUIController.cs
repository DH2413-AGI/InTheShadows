using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class LevelPlacementUIController : MonoBehaviour
{
    [SerializeField] private GameObject _levelPlacementLookAroundText;
    
    [SerializeField] private GameObject _levelPlacementPlaceText;

    [SerializeField] private LevelPlacementController _levelPlacementController;

    void Start()
    {
    }

    void Update()
    {
        bool levelPlacementFound = _levelPlacementController.LatestARPlaneSearch.IsReasonableForLevelPlacement;
        _levelPlacementPlaceText.SetActive(levelPlacementFound);
        _levelPlacementLookAroundText.SetActive(!levelPlacementFound);
    }
}
