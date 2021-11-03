using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    public Vector3 endPos;
    public float speed = 1.0f;
    public float setDelayTime = 1.0f;

    private bool moving = false;
    private bool opening = true;
    private Vector3 startPos;
    private float delay = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (opening)
            {
                MoveDoor(endPos);
            }
            else
            {
                MoveDoor(startPos);
            }
        }
    }

    void MoveDoor(Vector3 goalPos)
    {
        float dist = Vector3.Distance(transform.localPosition, goalPos);
        if (dist > 0.001f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, goalPos, Time.deltaTime * speed);
        }
        else
        {
            if (opening)
            {
                delay += Time.deltaTime;
                if (delay > setDelayTime)
                {
                    opening = false;
                    delay = 0.0f;
                }
            }
            else
            {
                moving = false; 
                opening = true;
            }
        }
    }

    public bool Moving
    {
        get { return moving; } 
        set { moving = value; } 
    }
}
