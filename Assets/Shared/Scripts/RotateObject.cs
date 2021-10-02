using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float degreesPerSecond = 5.0f;


    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, degreesPerSecond * Time.deltaTime, 0, Space.Self);
    }
}
