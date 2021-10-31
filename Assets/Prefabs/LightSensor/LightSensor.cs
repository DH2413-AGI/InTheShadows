using System;
using UnityEngine;

public class LightSensor : MonoBehaviour
{
    [SerializeField] private ShadowDetector _shadowDetector;

    public Action InShadow;
    public Action InLight;

    void Start()
    {
        this._shadowDetector.OnEnterShadow += this.OnEnterShadow;
        this._shadowDetector.OnLeavingShadow += this.OnLeavingShadow;
    }

    private void OnEnterShadow()
    {
        this.InShadow.Invoke();
    }

    private void OnLeavingShadow()
    {
        this.InLight.Invoke();
    }
}
