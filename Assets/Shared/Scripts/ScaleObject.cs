using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : MonoBehaviour
{
    [SerializeField] private Vector3 scaleChange = new Vector3(0.0f, -0.2f, 0.0f);


    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale += scaleChange * Time.deltaTime;
        
        // Move the object up and down
        if (gameObject.transform.localScale.y < 0.2f || gameObject.transform.localScale.y > 1.0f)
        {
            scaleChange = -scaleChange;
        }
    }
}
