
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using Mirror;

[RequireComponent(typeof(ARPlaneFinder))]
public class LevelPlacementController : MonoBehaviour
{
    [SerializeField] private PlayerPlacedLevelTracker _playerReadyTracker;

    [SerializeField] private LevelManager _levelManager;

    private ARSessionSetup _arSessionSetup;

    private ARSessionOrigin _arSessionOrigin;

    private ARPlaneFinder _arPlaneFinder;

    [SerializeField] private GameObject _levelPlaceholderPrefab;


    private GameObject _levelPlaceholder;

    private bool _hasPlacedLevel = false;

    public ARPlaneSearch LatestARPlaneSearch = new ARPlaneSearch() 
    {
        FoundPlane = false
    };

    void Awake()
    {
        _arSessionOrigin = this.gameObject.GetComponent<ARSessionOrigin>();
        this._arPlaneFinder = this.gameObject.GetComponent<ARPlaneFinder>();
    }

    void Start()
    {
        _levelPlaceholder = Instantiate(_levelPlaceholderPrefab, Vector3.zero, Quaternion.identity);
        _levelPlaceholder.SetActive(false);

        _arSessionSetup = FindObjectOfType<ARSessionSetup>();

        _playerReadyTracker.OnStart += () => StartCoroutine(_arSessionSetup.CheckForARSupport(this.PlaceLevelDesktop));
        // StartCoroutine(_arSessionSetup.CheckForARSupport(this.PlaceLevelDesktop));
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
        if (!this.LatestARPlaneSearch.IsReasonableForLevelPlacement) return;
        
        _hasPlacedLevel = true;
        if (_levelManager != null) {
            _levelManager.UpdateLevelSpawnPosition(new Pose(
                this.LatestARPlaneSearch.PlaneHitPosition, 
                this.LatestARPlaneSearch.CameraRotationTowardsPlane
            ));
        }
        this._playerReadyTracker.MarkPlayerPlacedLevel();
    }

    private void PlaceLevelDesktop(ARSessionState arSessionState)
    {
        if (arSessionState != ARSessionState.Unsupported) return;
        Debug.Log("Place level desktop");
        _hasPlacedLevel = true;
        this._levelManager.UpdateLevelSpawnPosition(new Pose(Vector3.zero, Quaternion.Euler(0.0f, 45.0f, 0.0f)));
        this._playerReadyTracker.MarkPlayerPlacedLevel();
    }

    private void SearchForPlane()
    {
        this.LatestARPlaneSearch = this._arPlaneFinder.SearchForPointedARPlane();
    }

    private void RenderLevelPlaceholderOnPlane()
    {
        if (!this.LatestARPlaneSearch.IsReasonableForLevelPlacement) 
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
