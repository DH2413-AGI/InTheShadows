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


    private GameObject _cameraLight;

    private bool _wasInsideShadow = false;


    void Start()
    {
        _arSessionSetup = FindObjectOfType<ARSessionSetup>();
    }

    private void TryToFindLight()
    {
        _cameraLight = GameObject.FindGameObjectWithTag("LightSource");
    }

    void Update()
    {
        if (_cameraLight == null) {
            this.TryToFindLight();
            return;
        }

        bool currentlyInsideShadow = this.IsInsideShadow();
        if(currentlyInsideShadow && !_wasInsideShadow ) {
            _wasInsideShadow = true;
            if (this.OnEnterShadow != null) this.OnEnterShadow.Invoke();
        } 
        if(!currentlyInsideShadow && _wasInsideShadow ) {
            _wasInsideShadow = false;
            if (this.OnLeavingShadow != null) this.OnLeavingShadow.Invoke();
        } 
    }

    public bool IsInsideShadow()
    {
        if (this._cameraLight == null) return true;
        Vector3 sensorToLight = (_cameraLight.transform.position - this.gameObject.transform.position);
        float distanceToLight = sensorToLight.magnitude;
        float marginOfErrorDistance = 1.01f;
        return Physics.Raycast(this.gameObject.transform.position, sensorToLight.normalized, distanceToLight * marginOfErrorDistance);
    }
}
