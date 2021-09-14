using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;

public class ShadowDetector : MonoBehaviour
{
    private Vector3 _originalPosition;

    private ARSessionSetup _arSessionSetup;

    public GameObject ARCameraLight;

    public GameObject desktopCameraLight;

    public GameObject player;

    public NavMeshAgent playerNavMeshAgent;

    private GameObject _cameraLight;


    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = player.transform.position;       
        _arSessionSetup = FindObjectOfType<ARSessionSetup>();
        StartCoroutine(_arSessionSetup.CheckForARSupport(this.setLight));
    }

    private void setLight(ARSessionState arSessionState)
    {
        _cameraLight = arSessionState == ARSessionState.Unsupported ? desktopCameraLight : ARCameraLight; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerToLight = _cameraLight.transform.position - player.transform.position;
        float distance = playerToLight.magnitude;
        Vector3 origin = player.transform.position;
        Vector3 direction = playerToLight.normalized;

        if(Physics.Raycast(origin,direction,distance)) {
            // Reset the player position if they are in the shadows.
            playerNavMeshAgent.ResetPath();
            player.transform.position = this._originalPosition;
        } 
    }
}
