using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRandomizer : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private Vector3 _space = Vector3.one;

    private float _noiseCoordinate = 0.0f;
    
    private Vector3 GetPositionFromNoise() 
    {
        return new Vector3(
            (Mathf.PerlinNoise(_noiseCoordinate, 0.0f) - 0.5f) * _space.x,
            (Mathf.PerlinNoise(_noiseCoordinate, 10.0f) - 0.5f) * _space.y,
            (Mathf.PerlinNoise(_noiseCoordinate, 20.0f) - 0.5f) * _space.z
        );
    }

    // Update is called once per frame
    void Update()
    {
        this._noiseCoordinate += this._speed * Time.deltaTime;
        this.gameObject.transform.localPosition = this.GetPositionFromNoise(); 
    }
}
