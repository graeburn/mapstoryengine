using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Examples;

using System;
using TMPro;

// Lets the user select the next target map location to visit from those still available
public class SelectTargetLocation : MonoBehaviour
{
    // Variable to store index of target site
    public int selectedSiteNumber = -1;

    // Load pop-window to select next target site
    [SerializeField]
    private GameObject SetNextSitePanel_8;

    // Button to confirm next location to walk to
	[SerializeField]
    private Button Accept_Button_8_2;

    // Return to the map rather than assigning next location
    [SerializeField]
    private Button Back_Button_8_3;

    // Variable to confirm where pop-window is active
    private bool targetPanelActive = false;

    // Position of user's finger on phone touchscreen
    private Vector2 touchPosition = default;

    // Parent object containing list of all map sites to visit
    [SerializeField]
    private GameObject TargetSites_13;

    // Link to map camera
    [SerializeField]
    private Camera mapCamera;

    // Banner offering user instructions to select next site to visit
    [SerializeField]
    private GameObject MapSite_Banner_5_1;

    // Assign selected site as the next target to visit
    [SerializeField]
    private GameObject CurrentSite_14;

    // Link to the Story Manager that handles the story flow and whether user has arrived at each target site
    [SerializeField]
    private GameObject StoryManager_15;

    // Overlaid map tools to recentre camera and trigger the next story site manually
    [SerializeField]
    private GameObject MapToolsPanel_5;

    // Panel to place associated 3D objects on user's surroundings
    [SerializeField]
    private GameObject PlaceObjectPanel_9;

    // Add listeners to buttons
    void Start()
    {
  		Accept_Button_8_2.onClick.AddListener(SetNextTarget);
  		Back_Button_8_3.onClick.AddListener(ChooseAgain);

    }
   
    // Detect if the user has touched one of the possible target locations on the displayed map
    void Update()
    {

        if(Input.touchCount > 0 && targetPanelActive == false)
        {
            Touch touch = Input.GetTouch(0);
            
            touchPosition = touch.position;

            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = mapCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if(Physics.Raycast(ray, out hitObject))
                {
                
            	    if(hitObject.transform.gameObject != null)
                    {
                    	for(int i = 0; i < TargetSites_13.transform.childCount; i++)
                    	{
                    		if(GameObject.ReferenceEquals(hitObject.transform.gameObject, TargetSites_13.transform.GetChild(i).gameObject))
                    		{
                    			selectedSiteNumber = i;
                    		}
                            
                    	}

                        SetNextSitePanel_8.SetActive(true);
                        targetPanelActive = true;
                        MapToolsPanel_5.SetActive(false);
                    }    
                
                }
            }
        }
        

    }

    // Assign the pressed target as the next location to visit and update the story manager accordingly
    void SetNextTarget()
    {
        for(int i = 0; i < TargetSites_13.transform.childCount; i++)
        {
            TargetSites_13.transform.GetChild(i).gameObject.SetActive(false);
        }
        

    	CurrentSite_14.transform.position = TargetSites_13.transform.GetChild(selectedSiteNumber).gameObject.transform.position;
        CurrentSite_14.SetActive(true);

        StoryManager_15.GetComponent<StoryManager>().visitedSites.Add(selectedSiteNumber);
        StoryManager_15.GetComponent<StoryManager>().currentSiteVar = selectedSiteNumber;
        PlaceObjectPanel_9.GetComponent<PlaceObject>().currentSiteVar = selectedSiteNumber;
        StoryManager_15.GetComponent<StoryManager>().countSitesVisited++;

        StoryManager_15.GetComponent<StoryManager>().startVar = true;
        StoryManager_15.GetComponent<StoryManager>().audioVar = false;
        
        MapSite_Banner_5_1.SetActive(false);   
    	SetNextSitePanel_8.SetActive(false);
    	targetPanelActive = false;
    	MapToolsPanel_5.SetActive(true);

    	this.enabled = false;
    	this.transform.gameObject.SetActive(false);
    }

    // Return to the map to select a different target site to walk to next
    void ChooseAgain()
    {
    	MapToolsPanel_5.SetActive(true);
        selectedSiteNumber = -1;
    	SetNextSitePanel_8.SetActive(false);
    	targetPanelActive = false;
    }

}