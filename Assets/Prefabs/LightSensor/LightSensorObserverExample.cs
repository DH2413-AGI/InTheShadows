using UnityEngine;

public class LightSensorObserverExample : MonoBehaviour
{
    [SerializeField] private LightSensor _lightSensor;

    void Start()
    {
        this._lightSensor.InLight += this.InLight;
        this._lightSensor.InShadow += this.InShadow;
    }

    private void InLight()
    {
        Debug.Log("In light!");
    }

    private void InShadow()
    {
        Debug.Log("In shadow!");
    }
}
