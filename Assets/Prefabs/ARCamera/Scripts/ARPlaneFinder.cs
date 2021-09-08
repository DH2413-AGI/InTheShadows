using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlaneFinder : MonoBehaviour
{
    private ARRaycastManager _arRaycastManager;
    [SerializeField] private Camera _arCamera;

    void Awake()
    {
        this._arRaycastManager = this.gameObject.GetComponent<ARRaycastManager>();
    }

    public ARPlaneSearch SearchForPointedARPlane()
    {
        Vector2 screenCenter = _arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        this._arRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        if (hits.Count == 0) return new ARPlaneSearch() { FoundPlane = false };
        return new ARPlaneSearch() { 
            FoundPlane = true,
            Pose = hits[0].pose
        };
    }

}
public class ARPlaneSearch 
{
    public bool FoundPlane { set; get; } = false;
    public Pose Pose { set; get; } = new Pose();
}
