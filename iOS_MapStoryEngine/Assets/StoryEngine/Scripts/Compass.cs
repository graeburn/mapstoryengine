using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using System; 

// Display the user heading on the map via the phone's compass 
public class Compass : MonoBehaviour 
{ 
	// Compass tracking user position variable
	private bool startTracking = false;

	// Wedge object points in compass direction user is facing
	[SerializeField]
	private GameObject compassWedge;

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
		if (startTracking) 
		{   
			if(float.IsNaN(Input.compass.trueHeading))
			{
				return;
			}
			else
			{
				compassWedge.transform.rotation = Quaternion.Euler(0, Input.compass.trueHeading, 0); 
			}
			
		}
	} 

	// Initialize phone compass
	IEnumerator InitializeCompass() 
	{ 
		yield return new WaitForSeconds(5.0f); 
		startTracking |= Input.compass.enabled; 
	} 

}