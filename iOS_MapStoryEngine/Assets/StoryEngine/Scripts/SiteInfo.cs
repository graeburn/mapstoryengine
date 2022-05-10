using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Examples;

using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;

// A template page to provide story information related to each map site visited
public class SiteInfo : MonoBehaviour
{
    // Continue to placing the associated AR object or to trigger the next site to visit
    [SerializeField]
    private Button ContinueButton_7_1;

    // Introduction text associated with current site
    [SerializeField]
    private GameObject IntroText_7_4;

    // Ending text associated with the current site
    [SerializeField]
    private GameObject OutroText_7_5;

    // Panel to place a 3D object against surroundings using AR
    [SerializeField]
    private GameObject PlaceObjectPanel_9;

    // Instruction text associtaed with placing 3D objects
    [SerializeField]
    private Text PlacingInstructions_9_1_1;

    // Link to current ARSession
    [SerializeField]
    private ARSession arSession;

    // Link to map camera object
    [SerializeField]
    private Camera mapCamera;

    // Map tools panel containing buttons to trigger sites manually and reset camera position
    [SerializeField]
    private GameObject MapToolsPanel_5;

    // Banner object to select next map site to visit
    [SerializeField]
    private GameObject MapSiteBanner_5_1;

    // Variable whether currently in introduction or outro stage of current site
    public bool siteIntro = true;

    // Variable whether guided from location to location or able to choose next local site to visit
    public bool guidedStory;

    // Ending panel to load when visited all local map sites
    [SerializeField]
    private GameObject EndingPanel_10;

    // Link to story manager object
    [SerializeField]
    private GameObject StoryManager_15;

    // Object that allows user which site to visit next if guidedStory is false
    [SerializeField]
    private GameObject TargetSites_13;

    // Link to confirm how many sites user is required to visit before ending
    [SerializeField]
    private GameObject SearchLocalSites_12;

    // Add listener to button on page
    void Start()
    {
        ContinueButton_7_1.onClick.AddListener(ContinueApp);
    }

    /// <summary>
    /// Pressing button will link to placing a 3D object after introduction phase, or will
    /// trigger the next site to visit after outro phase. It will link to the ending once
    /// all map sites have been visited.
    /// </summary>
    void ContinueApp()
    {
        if(siteIntro == true)
        {
            arSession.enabled = true;
            PlaceObjectPanel_9.GetComponent<PlaceObject>().enabled = true;
            PlaceObjectPanel_9.SetActive(true);
            PlaceObjectPanel_9.GetComponent<PlaceObject>().RePositionObject();
            PlacingInstructions_9_1_1.text = "Hold your phone in front of you and a 3D object will appear. Move and place it somewhere appropriate.";

        }
        else if(siteIntro == false)
        {
            if(StoryManager_15.GetComponent<StoryManager>().countSitesVisited == SearchLocalSites_12.GetComponent<SearchLocationsScript>().noLocationsToVisit - 1)
            {
                EndingPanel_10.GetComponent<Ending>().enabled = true;
                EndingPanel_10.SetActive(true);
            }
            else if(guidedStory == false)
            {
                StoryManager_15.GetComponent<StoryManager>().resetCamVar = false;

                mapCamera.enabled = true;
                mapCamera.transform.gameObject.GetComponent<TouchCamera>().enabled = true;

                MapToolsPanel_5.SetActive(true);
                MapSiteBanner_5_1.SetActive(true);

                TargetSites_13.GetComponent<SelectTargetLocation>().enabled = true;
                TargetSites_13.SetActive(true);

                StoryManager_15.GetComponent<StoryManager>().enabled = true;
                StoryManager_15.GetComponent<StoryManager>().setNextSite();
                StoryManager_15.GetComponent<StoryManager>().ReCentreCam();


            }
            else if(guidedStory == true)
            {
            	StoryManager_15.GetComponent<StoryManager>().resetCamVar = false;
                StoryManager_15.GetComponent<StoryManager>().countSitesVisited++;
                StoryManager_15.GetComponent<StoryManager>().audioVar = false;

                mapCamera.enabled = true;
                mapCamera.transform.gameObject.GetComponent<TouchCamera>().enabled = true;

                MapToolsPanel_5.SetActive(true);

                StoryManager_15.GetComponent<StoryManager>().enabled = true;
                StoryManager_15.GetComponent<StoryManager>().setNextSite();
                StoryManager_15.GetComponent<StoryManager>().ReCentreCam();
            }
            
        }
        IntroText_7_4.SetActive(false);
        OutroText_7_5.SetActive(false);
        
        siteIntro = false;
        this.enabled = false;
        this.transform.gameObject.SetActive(false);

    }
}