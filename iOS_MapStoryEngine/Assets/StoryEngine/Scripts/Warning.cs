using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Examples;

// A template warning page to remind the user to take care when using the app in public
public class Warning : MonoBehaviour
{
	// Link to StoryManager
	[SerializeField]
    private GameObject StoryManager_15;
	
	// Button to start the story
	[SerializeField]
	private Button StartButton_3_4;

	// Link to camera that displays the local map
    [SerializeField]
    private Camera mapCamera;

    // Variable for 2s pause to ensure user has read warning instructions before continueing
    private bool pauseVar = false;

    // Add start button listener
    void Start()
    {
    	StartButton_3_4.onClick.AddListener(StartStory);
       	StartCoroutine(Pause2());
    }

    // Show start button after 2 second pause
    IEnumerator Pause2()
    {
    	if(pauseVar == false)
    	{
    		pauseVar = true;
    		yield return new WaitForSeconds(2);
    		StartButton_3_4.transform.gameObject.SetActive(true);
    	}
    }

    /// <summary>
	/// Begin story code by activating associated variable in StoryManager. Activate map touch controls.
	/// </summary>
    void StartStory()
    {

        StoryManager_15.GetComponent<StoryManager>().startVar = true;
        mapCamera.transform.gameObject.GetComponent<TouchCamera>().enabled = true;

        this.enabled = false;
        this.transform.gameObject.SetActive(false);
    }


}