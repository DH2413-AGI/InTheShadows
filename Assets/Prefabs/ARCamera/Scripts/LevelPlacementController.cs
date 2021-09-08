using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ARPlaneFinder))]
public class LevelPlacementController : MonoBehaviour
{
    private ARPlaneFinder _arPlaneFinder; 
    [SerializeField] private GameObject _levelPlaceholderPrefab;
    private GameObject _levelPlaceholder;

    private bool _hasPlacedLevel = false;

    void Awake()
    {
        this._arPlaneFinder = this.gameObject.GetComponent<ARPlaneFinder>();
    }

    void Start()
    {
        _levelPlaceholder = Instantiate(_levelPlaceholderPrefab, Vector3.zero, Quaternion.identity);
        _levelPlaceholder.SetActive(false);
    }


    void Update()
    {
        if (!_hasPlacedLevel)
        {
            RenderLevelPlaceholderOnPlane();
            HandelUserTouch();
        }
    }


    private void HandelUserTouch()
    {
        bool hasStartedTouching = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
        if (hasStartedTouching) PlaceLevel();
    }

    private void PlaceLevel()
    {
        _hasPlacedLevel = true;
    }

    private void RenderLevelPlaceholderOnPlane()
    {
        ARPlaneSearch arPlaneSearch = this._arPlaneFinder.SearchForPointedARPlane();
        if (!arPlaneSearch.FoundPlane) 
        {
            _levelPlaceholder.SetActive(false);
        }
        else
        {
            _levelPlaceholder.SetActive(true);
            _levelPlaceholder.transform.SetPositionAndRotation(arPlaneSearch.Pose.position, arPlaneSearch.Pose.rotation);
        }
    }
}
