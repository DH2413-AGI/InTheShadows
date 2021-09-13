using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DesktopCameraControlls : MonoBehaviour
{

    [SerializeField] private GameObject _arCamera;
    [SerializeField] private GameObject _desktopCamera;

    private ARSessionSetup _arSessionSetup;

    private bool _isActivated = false;

    [Header("Start Rotation & Position")]
    [SerializeField] private float _startLeftRightRotation = 0.0f; 
    [SerializeField] private float _startUpDownRotation = 0.0f; 
    [SerializeField] private float _startZoom = 0.0f; 


    [Header("Zoom Settings")]
    [SerializeField] private float _minZoom = -10.0f; 
    [SerializeField] private float _maxZoom = -2.0f; 


    [Header("Movement Sensivity")]
    [SerializeField] private float _leftRightMovementSensitivity = 3.0f; 
    [SerializeField] private float _upDownMovementSensitivity = 3.0f; 
    [SerializeField] private float _zoomSensitivity = 5.0f; 
    [SerializeField] private float _cameraMovementSpeed = 4.0f; 


    [Header("Rotation Play and Smoothness")]
    
    [Tooltip("How much the user can move the camera up and down")]
    [SerializeField] private float _upDownRotationPlay = 70.0f; 

    [Tooltip("How much to smooth the up down rotation when reaching the max up down rotation play")]
    [SerializeField] private float _upDownRotationSmoothnessLength = 20.0f; 

    [Tooltip("The smooth curve for the up down rotation")]
    [SerializeField] private AnimationCurve _upDownRotationSmoothnessCurve = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 0.0f); 


    [Header("Required Objects")]
    [SerializeField] private GameObject _leftRightRotatorObject; 
    [SerializeField] private GameObject _upDownRotatorObject; 
    [SerializeField] private Camera _mainCamera; 


    void Start()
    {
        this._arSessionSetup = FindObjectOfType<ARSessionSetup>();
        StartCoroutine(_arSessionSetup.CheckForARSupport(this.CheckIfShouldActivateDesktopControlls));

        this.SetStartPosition();
    }

    private void CheckIfShouldActivateDesktopControlls(ARSessionState arSessionState)
    {
        if (arSessionState == ARSessionState.Unsupported) 
        {
            this.ActivateDesktopControlls();
        }
    }

    private void ActivateDesktopControlls()
    {
        this._arCamera.SetActive(false);
        this._desktopCamera.SetActive(true);
        this._isActivated = true;
    }

    // Update is called once per frame
    void Update()
    {
        // We do not want to run any logic here if the desktop controll are inactive.
        if(!_isActivated) return;

        HandleCameraRotation();
        HandleCameraZoom();
        HandleCameraMovement();
    }

    private void HandleCameraMovement()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) {
            direction += _leftRightRotatorObject.transform.forward;
        }
        if (Input.GetKey(KeyCode.S)) {
            direction -= _leftRightRotatorObject.transform.forward;
        }
        if (Input.GetKey(KeyCode.D)) {
            direction += _leftRightRotatorObject.transform.right;
        }
        if (Input.GetKey(KeyCode.A)) {
            direction -= _leftRightRotatorObject.transform.right;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            this.gameObject.transform.position = Vector3.zero;
            return;
        }
        this.gameObject.transform.Translate(direction * Time.deltaTime * _cameraMovementSpeed);
    }

    private void HandleCameraRotation() 
    {
        bool shouldDragCamera = Input.GetMouseButton(1);
        Cursor.visible = !shouldDragCamera;
        if (!shouldDragCamera) return;
        RotateCameraLeftAndRight(Input.GetAxis("Mouse X") * _leftRightMovementSensitivity);
        RotateCameraUpAndDown(-Input.GetAxis("Mouse Y") * _upDownMovementSensitivity);
    }

    private void HandleCameraZoom()
    {
        _mainCamera.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * _zoomSensitivity);
        if (_mainCamera.transform.localPosition.z < _minZoom) {
            _mainCamera.transform.localPosition = new Vector3(0.0f, 0.0f, _minZoom);
        }
        if (_mainCamera.transform.localPosition.z > _maxZoom) {
            _mainCamera.transform.localPosition = new Vector3(0.0f, 0.0f, _maxZoom);
        }
    }

    private void RotateCameraLeftAndRight(float rotation) 
    {
        _leftRightRotatorObject.transform.Rotate(
            Vector3.up * rotation,
            Space.Self
        );
    }

    private void RotateCameraUpAndDown(float rotationToApply) 
    {
        var currentRotation = _upDownRotatorObject.transform.rotation.eulerAngles.x;
        var newClampedRotation = SmoothClampNewRotationValue(currentRotation, rotationToApply);

        _upDownRotatorObject.transform.Rotate(
            Vector3.right * newClampedRotation,
            Space.Self
        );
    }

    private bool ShouldStartClampRotation(float currentRotation, float rotationToApply, float rangeStart, float rangeEnd) 
    {
        var newRotationValue = currentRotation + rotationToApply;

        bool isWithinRange = newRotationValue >= rangeStart && newRotationValue <= rangeEnd;
        if (isWithinRange) return false;

        bool isOutsideRangeButHeadingBack = newRotationValue <= rangeStart && rotationToApply > 0 || newRotationValue >= rangeEnd && rotationToApply < 0;
        if (isOutsideRangeButHeadingBack) return false;
        
        return true;
    }

    private float SmoothClampNewRotationValue(float currentRotation, float rotationToApply) 
    {
        var newRotationValue = currentRotation + rotationToApply;

        // This makes the angle go between postive, zero, and negative
        var currentValueModified = currentRotation < 180 ? currentRotation : currentRotation - 360;
        if (!ShouldStartClampRotation(currentValueModified, rotationToApply, -_upDownRotationPlay / 2.0f, _upDownRotationPlay / 2.0f)) return rotationToApply;
        var rotationOverMaxPlay = Mathf.Abs(currentValueModified + rotationToApply) - _upDownRotationPlay / 2.0f;

        return _upDownRotationSmoothnessCurve.Evaluate(rotationOverMaxPlay / _upDownRotationSmoothnessLength) * rotationToApply;
    }

    private void SetStartPosition() 
    {
        _leftRightRotatorObject.transform.localRotation = Quaternion.Euler(0.0f, _startLeftRightRotation, 0.0f);
        _upDownRotatorObject.transform.localRotation = Quaternion.Euler(_startUpDownRotation, 0.0f, 0.0f);
        _mainCamera.transform.localPosition = new Vector3(0, 0, _startZoom);
    }

}
