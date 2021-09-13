using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSessionSetup : MonoBehaviour
{
    void Start()
    {
        // If we destory the ARSession object between scenes, the camera needs to be reloaded
        // and the scene transition would feel buggy. We thefore dont destory this object on
        // load.
        DontDestroyOnLoad(this.gameObject);
    }

}
