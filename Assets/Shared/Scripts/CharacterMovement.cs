using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;

public class CharacterMovement : MonoBehaviour
{
    private ARSessionSetup _arSessionSetup;

    public NavMeshAgent playerNavMeshAgent;

    public Camera ARCamera;

    public Camera desktopCamera;

    private Camera _playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        _arSessionSetup = FindObjectOfType<ARSessionSetup>();
        StartCoroutine(_arSessionSetup.CheckForARSupport(this.setCamera));
    }

    private void setCamera(ARSessionState arSessionState)
    {
        _playerCamera = arSessionState == ARSessionState.Unsupported ? desktopCamera : ARCamera; 
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
 
            if (Physics.Raycast(_playerCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                playerNavMeshAgent.SetDestination(hit.point);
            }
        }
    }
}
