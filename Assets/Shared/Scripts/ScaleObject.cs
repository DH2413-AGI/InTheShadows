using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : MonoBehaviour
{
    private Vector3 scaleChange;
    // Start is called before the first frame update
    void Start()
    {
        scaleChange = new Vector3(-0.0f, -0.01f, -0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale += scaleChange;
        
        // Move the object up and down
        if (gameObject.transform.localScale.y < 0.2f || gameObject.transform.localScale.y > 1.0f)
        {
            scaleChange = -scaleChange;
        }
    }
}
