using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;

public class ShadowDetector : MonoBehaviour
{
    public Action OnEnterShadow;
    public Action OnLeavingShadow;

    private ARSessionSetup _arSessionSetup;

    public GameObject ARCameraLight;

    public GameObject desktopCameraLight;

    private GameObject _cameraLight;

    private bool _isInsideShadow = false;


    void Start()
    {
        _arSessionSetup = FindObjectOfType<ARSessionSetup>();
        StartCoroutine(_arSessionSetup.CheckForARSupport(this.SetLight));
    }

    private void SetLight(ARSessionState arSessionState)
    {
        _cameraLight = arSessionState == ARSessionState.Unsupported ? desktopCameraLight : ARCameraLight; 
    }

    void Update()
    {
        Vector3 sensorToLight = (_cameraLight.transform.position - this.gameObject.transform.position);
        float distanceToLight = sensorToLight.magnitude;
        bool currentlyInsideShadow = Physics.Raycast(this.gameObject.transform.position, sensorToLight.normalized, distanceToLight);

        if(currentlyInsideShadow && !_isInsideShadow ) {
            _isInsideShadow = true;

            if (this.OnEnterShadow == null) return;
            this.OnEnterShadow.Invoke();
        } 
        if(!currentlyInsideShadow && _isInsideShadow ) {
            _isInsideShadow = false;
            
            if (this.OnLeavingShadow == null) return;
            this.OnLeavingShadow.Invoke();
        } 
    }
}
