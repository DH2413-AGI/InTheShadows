using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CrossPlatformCameraSetup : MonoBehaviour
{
    [SerializeField] private GameObject _arCamera;
    [SerializeField] private GameObject _desktopCamera;
    [SerializeField] private GameObject _desktopControll;

    private ARSessionSetup _arSessionSetup;

    void Start()
    {
        this._arSessionSetup = FindObjectOfType<ARSessionSetup>();
        StartCoroutine(_arSessionSetup.CheckForARSupport(this.ActiveCameraForCorrectPlatform));
    }

    private void ActiveCameraForCorrectPlatform(ARSessionState arSessionState)
    {
        if (arSessionState == ARSessionState.Unsupported) 
        {
            this._desktopCamera.SetActive(true);
            this._desktopControll.SetActive(true);
        }
        else {
            this._arCamera.SetActive(true);
            this._desktopControll.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
