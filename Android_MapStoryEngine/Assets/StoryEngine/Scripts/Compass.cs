using System.Collections; 
using System.Collections.Generic; 
using UnityEngine;  
using System; 

public class Compass : MonoBehaviour 
{ 
 
 	// Compass tracking user position variable
	private bool startTracking = false;

	// Wedge object points in compass direction user is facing
	[SerializeField]
	private GameObject compassWedge;

	private int count = 0;
	private float sum = 0.0f;
	private float previous = 0;

	// Enable phone compass
	void Start() 
	{ 
		Input.compass.enabled = true; 
		Input.location.Start(); 
		StartCoroutine(InitializeCompass()); 
	} 

	// Update wedge position according to compass heading
	void Update() 
	{ 
		if(startTracking && count < 15)
		{
			if(float.IsNaN(Input.compass.trueHeading))
			{
				sum = 0.0f;
				count = 0;
				return;
			}
			else if(Mathf.Abs(Input.compass.trueHeading - previous) > 20.0f && count > 0)
			{
				previous = Input.compass.trueHeading;
				sum = 0.0f;
				count = 0;
				return;
			}
			else
			{
				sum += Input.compass.trueHeading;
				previous = Input.compass.trueHeading;
				count++;
			}
			
		}
		else if (startTracking && count == 15) 
		{ 
			float average = sum/15;
			compassWedge.transform.rotation = Quaternion.Euler(0, average, 0);
			sum = 0.0f;
			count = 0;
		}
	} 

	// Initialize phone compass
	IEnumerator InitializeCompass() 
	{ 
		yield return new WaitForSeconds(5.0f); 
		startTracking |= Input.compass.enabled; 
	} 

}