using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityRandomizer : MonoBehaviour
{
    [SerializeField] float _range = 0.5f;
    [SerializeField] float _speed = 0.5f;
    private float _startingIntensity = 0.0f;
    private float _noiseCoordinate = 0.0f; 

    private Light _light;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
        _startingIntensity = _light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        _noiseCoordinate += this._speed * Time.deltaTime;
        this._light.intensity = (Mathf.PerlinNoise(0.0f, _noiseCoordinate) - 0.5f) * this._range + this._startingIntensity;
    }
}
