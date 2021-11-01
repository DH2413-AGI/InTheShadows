using System;
using UnityEngine;

public class LightSensor : MonoBehaviour
{
    [SerializeField] private ShadowDetector _shadowDetector;
    private Animator _lightSensorAnimator;

    public Action InShadow;
    public Action InLight;

    private bool _isInsideShadow = false;
    public bool HasLight {
        get => !_isInsideShadow;
    }

    void Awake() 
    {
        _lightSensorAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        this._shadowDetector.OnEnterShadow += this.OnEnterShadow;
        this._shadowDetector.OnLeavingShadow += this.OnLeavingShadow;
        _lightSensorAnimator.SetBool("LightSensorActive", this._shadowDetector.IsInsideShadow());
        _isInsideShadow = this._shadowDetector.IsInsideShadow();
    }

    private void OnEnterShadow()
    {
        this.InShadow.Invoke();
        _isInsideShadow = true;
        _lightSensorAnimator.SetBool("LightSensorActive", false);
    }

    private void OnLeavingShadow()
    {
        this.InLight.Invoke();
        _isInsideShadow = false;
        _lightSensorAnimator.SetBool("LightSensorActive", true);
    }
}
