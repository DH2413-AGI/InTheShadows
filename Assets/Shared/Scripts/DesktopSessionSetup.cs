using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DesktopSessionSetup : MonoBehaviour
{
    [SerializeField] private ARSessionSetup _arSessionSetup;
    [SerializeField] private LevelManager _levelManager;

    void Start()
    {
        _arSessionSetup = FindObjectOfType<ARSessionSetup>();
        StartCoroutine(_arSessionSetup.CheckForARSupport(this.CheckIfShouldBeEnabled));

        DontDestroyOnLoad(this.gameObject);
    }

    private void CheckIfShouldBeEnabled(ARSessionState arSessionState)
    {
        if (arSessionState == ARSessionState.Unsupported) 
        {
            this.StartDesktopSetup();
        }
    }

    private void StartDesktopSetup()
    {
        _levelManager.LoadCurrentLevel();
    }

    void Update()
    {
        
    }
}
