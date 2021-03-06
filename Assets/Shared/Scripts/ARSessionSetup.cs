using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARSessionSetup : MonoBehaviour
{
    private ARSession _arSession;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        _arSession = this.gameObject.GetComponent<ARSession>();

        // If we destory the ARSession object between scenes, the camera needs to be reloaded
        // and the scene transition would feel buggy. We thefore dont destory this object on
        // load.
        DontDestroyOnLoad(this.gameObject);
    }

    public IEnumerator CheckForARSupport(Action<ARSessionState> OnCheckCompleted) {
        
        if ((ARSession.state == ARSessionState.None) ||
            (ARSession.state == ARSessionState.CheckingAvailability))
        {
            yield return ARSession.CheckAvailability();
        }
        OnCheckCompleted(ARSession.state);
    }
}

