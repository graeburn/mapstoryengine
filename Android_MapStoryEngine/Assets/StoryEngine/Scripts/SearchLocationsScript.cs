using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Networking;
using System.Text;
using System;
using System.Linq;
using SimpleJSON;

using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;

// Use the Google Places API to look for local map sites of significance to use as part of the story
public class SearchLocationsScript : MonoBehaviour
{
	// Variable to set number of map locations to visit during story
	[Tooltip("Set the number of local sites that will be visited as part of the story")]
	[Range(2, 20)]
	public int noLocationsToVisit;

	// Variable to set the play area in metres around the user's starting position
	[Tooltip("Set the search radius in metres as a multiple of 50 to look for map sites around the user's position")]
	public string searchRadius = "250";

	// User's Google API key
	[Tooltip("Enter your Google API key")]
    public string API_KEY = "AIzaSyA0_8mYpCq7NxULk75T-zwlFM4fCTdWYX4";

    // Google API Nearby Places url
    private string Places_url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json";

    // Create an instance of the script
    public static SearchLocationsScript instance;

    // Link to map object
    [SerializeField]
    private AbstractMap _map;

    // Link to map marker than displays user's current location
    [SerializeField]
    private GameObject ARPlayerMapMarker;

    // Latitude and longitude of user's starting location
    private string currentLat;
    private string currentLong;
    private Vector2d playerLatLong;

    // JSON result from API request
    public JSONNode jsonResult;

    // Check if API request has been made
    private bool localDataRequested = false;

    // Confirm API reqsult has been received
    private bool updatedInfo = false;

    // Link to Story Manager object that controls the stoiry flow and movement between map sites
    [SerializeField]
    private GameObject StoryManager_15;

    // Problem panel to load if not enough local map sites can be detected
    [SerializeField]
    private GameObject ProblemPanel_4;

    // Create instance of script
    void Awake()
    {
    	instance = this;
    }

    // Detect user's starting location and send request for local named map sites nearby
    void Start()
    {
        playerLatLong = _map.WorldToGeoPosition(ARPlayerMapMarker.transform.position);
        currentLat = playerLatLong.x.ToString();
        currentLong = playerLatLong.y.ToString();

        StartCoroutine(SearchNearbyPlaces(currentLat, currentLong));
    }

    // Submit received API results to Story Manager
    void Update()
    {

        // Nearby sites
        if(updatedInfo == false && jsonResult != null)
        {
            StoryManager_15.GetComponent<StoryManager>().LocationsToVisit = jsonResult;
            updatedInfo = true;
        }

        // Not enough sites found
        if(localDataRequested == true && (jsonResult == null || jsonResult["results"].Count < noLocationsToVisit))
        {
        	ProblemPanel_4.GetComponent<Problem>().enabled = true;
            ProblemPanel_4.SetActive(true);
        }

        
    }

    // Send API request and return JSON file
    IEnumerator SearchNearbyPlaces(string lat, string lng)
    {
        if(localDataRequested == false)
        {
            UnityWebRequest webReq = new UnityWebRequest();
            webReq.downloadHandler = new DownloadHandlerBuffer();

            webReq.url = string.Format("{0}?location={1},{2}&radius={3}&rankby=prominence&key={4}", Places_url, lat, lng, searchRadius, API_KEY);

            yield return webReq.SendWebRequest();

            string rawJson = Encoding.Default.GetString(webReq.downloadHandler.data);
            jsonResult = JSON.Parse(rawJson);

            localDataRequested = true;
        }

    }

}
