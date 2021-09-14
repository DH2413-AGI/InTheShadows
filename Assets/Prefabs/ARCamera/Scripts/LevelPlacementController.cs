
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneFinder))]
public class LevelPlacementController : MonoBehaviour
{
    private ARSessionOrigin _arSessionOrigin;

    private ARPlaneFinder _arPlaneFinder;

    [SerializeField] private GameObject _levelPlaceholderPrefab;
    private GameObject _levelPlaceholder;

    private LevelManager _levelManager;

    private bool _hasPlacedLevel = false;

    public ARPlaneSearch LatestARPlaneSearch = new ARPlaneSearch() 
    {
        FoundPlane = false
    };

    void Awake()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        _arSessionOrigin = this.gameObject.GetComponent<ARSessionOrigin>();
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
            SearchForPlane();
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
        if (!this.LatestARPlaneSearch.FoundPlane) return;

        _hasPlacedLevel = true;
        _levelManager.UpdateLevelSpawnPosition(new Pose(this.LatestARPlaneSearch.PlaneHitPosition, this.LatestARPlaneSearch.CameraRotationTowardsPlane));
        _levelManager.LoadCurrentLevel();
    }

    private void SearchForPlane()
    {
        this.LatestARPlaneSearch = this._arPlaneFinder.SearchForPointedARPlane();
    }

    private void RenderLevelPlaceholderOnPlane()
    {
        if (!this.LatestARPlaneSearch.FoundPlane) 
        {
            _levelPlaceholder.SetActive(false);
        }
        else
        {
            _levelPlaceholder.SetActive(true);
            _levelPlaceholder.transform.SetPositionAndRotation(this.LatestARPlaneSearch.PlaneHitPosition, this.LatestARPlaneSearch.CameraRotationTowardsPlane);
        }
    }
}
