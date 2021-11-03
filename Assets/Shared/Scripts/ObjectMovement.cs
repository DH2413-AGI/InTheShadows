using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public Vector3 endPos;
    public float speed = 0.5f;
    public float setDelayTime = 1.0f;
    

    private bool moveOut = true;              // move out or move back.
    private Vector3 startPos;
    private float delay = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("moveOut:" + moveOut);

        if (moveOut)
            MoveTo(endPos);
        else
            MoveTo(startPos);
    }

    void MoveTo (Vector3 goalPos)
    {
        float dist = Vector3.Distance(transform.localPosition, goalPos);

        
        
        if (dist > 0.01f)
            // transform.position = Vector3.Lerp(transform.position, goalPos, speed * Time.deltaTime);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, goalPos, Time.deltaTime * speed);
        else
        {
            delay += Time.deltaTime;
            if (delay > setDelayTime)            // stops for 1.5 second
            {
                delay = 0.0f;
                if (moveOut)                     // switch moveOut state
                    moveOut = false;
                else 
                    moveOut = true;
            }
            
        }
    }
}
