using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Mirror;

public class TimerManager : NetworkBehaviour
{
	public float timeValue;
	public float startTime = 10;
	private TMP_Text timeText;
	public GameObject timeUI;
	public CharacterController character;
	// private bool shouldRunTimer = true;
    // Update is called once per frame
    
    void Start()
    {
    	//character = FindObjectOfType<CharacterController>();
    	timeValue = startTime;
    	timeText = timeUI.GetComponent<TMP_Text>();
    	InvokeRepeating("RunTimer", 0, 1);
    }
  
    void RunTimer()
    {
    	if (timeValue > 0)
    	{
        	timeValue -= 1;
    	}
    	else if (timeValue <= 5 && timeValue > 0)
    	{
    		Handheld.Vibrate();
    		timeValue -= 1;
    	}
    	else
    	{
    		if (isServer) 
    		{
    			character.CommandDie();
    		}
    		timeValue += startTime;
    		// Kill the character and restart
    	}
    	DisplayTime(Mathf.Floor(timeValue));
    }

    void DisplayTime(float timeToDisplay)
    {
		timeText.text = timeToDisplay.ToString();
    }

    void Update()
    {
    	/* if (character.SpawnModeActivated)
    	{
    		shouldRunTimer = false;
    	}
    	else
    	{
    		shouldRunTimer = true;
    	}
		
    	if (shouldRunTimer)
    	{
    		RunTimer();
    	}
    	*/
    }
}
