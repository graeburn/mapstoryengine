namespace Mapbox.Unity.MeshGeneration.Factories
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    using Mapbox.Directions;
    using Mapbox.Examples;
    using Mapbox.Unity.Map;
    using Mapbox.Utils;
    using Mapbox.Unity.Utilities;

    using UnityEngine.XR.ARFoundation;
    using UnityEngine.Experimental.XR;

    using SimpleJSON;
    using System.Collections.Generic;
    using System.Linq;

    using Data;
    using Modifiers;
    using Random = UnityEngine.Random;

    // Manages the story flow and whether the user has arrived at the next target map location
    public class StoryManager : MonoBehaviour
    {
        // User option to be guided between map sites according to the shortest walk, or to select the next target from the map
        [Tooltip("If unselected the user can choose the next location to walk to, rather than the route being preselected.")]
        public bool guidedStory;

        // Link to map object
        [SerializeField]
        private AbstractMap _map;

        // Link to map camera
        [SerializeField]
        private Camera mapCamera;

        // Link to AR Session object
        [SerializeField]
        private ARSession arSession;

        // Map tools to recentre map camera and trigger next site manually
        [SerializeField]
        private GameObject MapToolsPanel_5;

        // Button to recentre map camera
        [SerializeField]
        private Button ReCentre_Button_5_2;

        // Button to trigger next map site without walking to the target location
        [SerializeField]
        private Button ManualTrigger_Button_5_3;

        // Button to trigger manually trigger the story to continue
        [SerializeField]
        private Button LocationTriggerGo_Button_6_2;

        // Button to return to the map
        [SerializeField]
        private Button LocationTriggerBack_Button_6_3;

        // Variable to progress the story associated with the next map site
        private bool arrivedAtLocationVar = false;

        // Pop-up window to allow the user to trigger manual continuation of the story
        [SerializeField]
        private GameObject SiteTriggerPanel_6;

        // Panel to overlay 3D objects on user's surroundings using AR
        [SerializeField]
        private GameObject PlaceObjectPanel_9;

        // Variable representing total number of map sites user is required to visit
        private int noLocationsToVisit;

        // Panel to load if enough map sites to visit cannot be found
        [SerializeField]
        private GameObject ProblemPanel_4;

        // Activated panel to allow user to minimise app during use
        [SerializeField]
        private GameObject GamePause_18;

        // List of local locations identified through Google API
        public JSONNode LocationsToVisit;

        // List of map sites to visit
        public GameObject[] MapSites;

        // Whether a map site has been visited yet
        private bool[] mapSitesVisited;

        // Object containing map sites user is required to visit
        [SerializeField]
        private GameObject TargetSites_13;

        // Basic prefab to spawn number of map targets to visit
        [SerializeField]
        private GameObject TargetPrefab;

        // Basic prefab to spawn number of map sites to visit
        [SerializeField]
        private GameObject MapSitePrefab;

        // List containing indices of map sites
        private int[] siteOrder;

        // Sorted list of map indices based on shortest walk between all map sites
        private int[] shortestSiteOrder;

        // Latitude and longitude variable
        private Vector2d siteLatLong;

        // Object to display user's current position on the map
        [SerializeField]
        private GameObject ARPlayerMapMarker;

        // Object to display next target location to walk to on the map
        [SerializeField]
        private GameObject CurrentSite_14;

        // Variable whether map sites have all been selected and sorted
        private bool finishedSortingSitesVar = false;

        // Larger number used for sorting sites according to distance
        private float maxDist = float.PositiveInfinity;

        // List of which map site indices have already been visited
        public List<int> visitedSites = new List<int>();

        // Audio tracks appropriate to each story site
        public GameObject SiteAudio_17;

        // 3D models to place at each location
        public GameObject ObjectsToPlace_16;

        // List of which audio tracks have been played
        private bool[] siteAudioPlayed;

        // Panel displaying the story section associated with each map site
        [SerializeField]
        private GameObject SiteInfoPanel_7;

        // Current chapter title of story
        [SerializeField]
        private Text Title_Text_7_2;

        // Name of the current site name
        [SerializeField]
        private Text SiteName_Text_7_3;

        // Placeholder text for story chapter introduction
        [SerializeField]
        private GameObject Intro_Text_7_4;

        // Link to API search and number of map sites set as part of story
        [SerializeField]
        private GameObject SearchLocalSites_12;

        // Index of current map site
        public int currentSiteVar = -1;

        // Count of number of map sites visited so far
        public int countSitesVisited = -1;

        // Variable to activate Story Manager
        public bool startVar = false;

        // Variable to confirm map camera position has been reset
        public bool resetCamVar = false;

        // Variable to confirm whether audio track has played
        public bool audioVar = false;

        // Add listeners to buttons
        void Awake()
        {
            ReCentre_Button_5_2.onClick.AddListener(ReCentreCam);
            ManualTrigger_Button_5_3.onClick.AddListener(ManualTriggerLocation);

            LocationTriggerGo_Button_6_2.onClick.AddListener(LocationTriggerGo);
            LocationTriggerBack_Button_6_3.onClick.AddListener(LocationTriggerBack);
        }

        // Identify and select local map sites
        void Start()
        {

            noLocationsToVisit = SearchLocalSites_12.GetComponent<SearchLocationsScript>().noLocationsToVisit;

            MapSites = new GameObject[noLocationsToVisit];
            mapSitesVisited = new bool[noLocationsToVisit];

            for(int i = 0; i < noLocationsToVisit; i++)
            {
            	MapSites[i] = Instantiate(MapSitePrefab, MapSitePrefab.transform.position, MapSitePrefab.transform.rotation);
            }

            for(int j = 0; j < noLocationsToVisit; j++)
            {
            	Instantiate(TargetPrefab, TargetPrefab.transform.position, TargetPrefab.transform.rotation, TargetSites_13.transform);
            }
            
            siteAudioPlayed = new bool[noLocationsToVisit];

            siteOrder = Enumerable.Range(0, noLocationsToVisit).ToArray();
            shortestSiteOrder = new int[noLocationsToVisit];

            PlaceObjectPanel_9.GetComponent<PlaceObject>().guidedStory = guidedStory;
            SiteInfoPanel_7.GetComponent<SiteInfo>().guidedStory = guidedStory;

            int noMapLocationsSet = 0;

            for(int i = 0; i < LocationsToVisit["results"].Count; i++)
            {
                if(LocationsToVisit["results"][i]["name"] != null && LocationsToVisit["results"][i]["name"].ToString().Length <= 30 && LocationsToVisit["results"][i]["types"].Count > 2 && noMapLocationsSet < noLocationsToVisit)
                {
                    // Set site name and loction
                    MapSites[noMapLocationsSet].name = LocationsToVisit["results"][i]["name"];
                    siteLatLong = new Vector2d(LocationsToVisit["results"][i]["geometry"]["location"]["lat"].AsFloat, LocationsToVisit["results"][i]["geometry"]["location"]["lng"].AsFloat);

                    Vector3 mapPosition = _map.GeoToWorldPosition(siteLatLong, true);
                    mapPosition.y = 0;
                    MapSites[noMapLocationsSet].transform.position = mapPosition;
                    TargetSites_13.transform.GetChild(noMapLocationsSet).gameObject.transform.position = mapPosition;
                    TargetSites_13.transform.GetChild(noMapLocationsSet).gameObject.name = MapSites[noMapLocationsSet].name;

                    // Identify closest distance to another site
                    float siteSpacing = 1000.0f;
                    for(int j = 0; j < noMapLocationsSet; j++)
                    {
                        if(noMapLocationsSet != 0 && Vector3.Distance(MapSites[j].transform.position, mapPosition) < siteSpacing)
                            siteSpacing = Vector3.Distance(MapSites[j].transform.position, mapPosition);
                    }

                    // Make sure new site not too far away or too near another site
                    if(Vector3.Distance(ARPlayerMapMarker.transform.position, mapPosition) > 800.0f || siteSpacing < 50.0f)
                        continue;
                    else
                    {

                        noMapLocationsSet++;
                    }

                }
            }

            // Relax condition if sites missing to fill with sites with only 1 or 2 types
            if(noMapLocationsSet < noLocationsToVisit)
            {
                for(int i = 0; i < LocationsToVisit["results"].Count; i++)
                {
                    if(LocationsToVisit["results"][i]["name"] != null && LocationsToVisit["results"][i]["name"].ToString().Length <= 30 && LocationsToVisit["results"][i]["types"].Count >= 1 && LocationsToVisit["results"][i]["types"].Count < 3 && noMapLocationsSet < noLocationsToVisit)
                    {
                        // Set site name and loction
                        MapSites[noMapLocationsSet].name = LocationsToVisit["results"][i]["name"];
                        Vector2d siteLatLong = new Vector2d(LocationsToVisit["results"][i]["geometry"]["location"]["lat"].AsFloat, LocationsToVisit["results"][i]["geometry"]["location"]["lng"].AsFloat);

                        Vector3 mapPosition = _map.GeoToWorldPosition(siteLatLong, true);
                        mapPosition.y = 0;
                        MapSites[noMapLocationsSet].transform.position = mapPosition;
                        TargetSites_13.transform.GetChild(noMapLocationsSet).gameObject.transform.position = mapPosition;
                        TargetSites_13.transform.GetChild(noMapLocationsSet).gameObject.name = MapSites[noMapLocationsSet].name;

                        // Identify closest distance to another site
                        float siteSpacing = 1000.0f;
                        for(int j = 0; j < noMapLocationsSet; j++)
                        {
                            if(noMapLocationsSet != 0 && Vector3.Distance(MapSites[j].transform.position, mapPosition) < siteSpacing)
                                siteSpacing = Vector3.Distance(MapSites[j].transform.position, mapPosition);
                        }

                        // Make sure new site not too far away or too near another site
                        if(Vector3.Distance(ARPlayerMapMarker.transform.position, mapPosition) > 800.0f || siteSpacing < 50.0f)
                            continue;
                        else
                        {

                            noMapLocationsSet++;
                        }
                    }
                }
            }

            // If not enough sites found show error page
            if(noMapLocationsSet < noLocationsToVisit)
            {
            	ProblemPanel_4.GetComponent<Problem>().enabled = true;
                ProblemPanel_4.SetActive(true);
                return;
            }

            // Make sure audio tracks are active
            for(int n = 0; n < SiteAudio_17.transform.childCount; n++)
            {
                SiteAudio_17.transform.GetChild(n).gameObject.SetActive(true);
            }

            // Make sure 3D objects to be placed are not active at start
            for(int m = 0; m < ObjectsToPlace_16.transform.childCount; m++)
            {
                ObjectsToPlace_16.transform.GetChild(m).gameObject.SetActive(false);
            }

            // Activate code in case game minimsed
            GamePause_18.SetActive(true);

            setStartingSite();

        }

        // Confirm whether a map site has been activated and the associated audio track has played
        void Update()
        {
            // Make sure returns until setup finsihed
            if(finishedSortingSitesVar == false || startVar == false)
            {
                return;
            }

            if(guidedStory == false && countSitesVisited >= 0 && countSitesVisited <= noLocationsToVisit && siteAudioPlayed[currentSiteVar] == false && !SiteAudio_17.transform.GetChild(currentSiteVar).gameObject.GetComponent<AudioSource>().isPlaying)
            {
                MapToolsPanel_5.SetActive(true);
                StartCoroutine(PlayNextSiteAudio());
            }
            else if(guidedStory == true && countSitesVisited >= 0 && countSitesVisited <= noLocationsToVisit && siteAudioPlayed[countSitesVisited] == false && !SiteAudio_17.transform.GetChild(countSitesVisited).gameObject.GetComponent<AudioSource>().isPlaying)
            {
                MapToolsPanel_5.SetActive(true);
                StartCoroutine(PlayNextSiteAudio());
            }
            else if(guidedStory == false && countSitesVisited >= 0 && countSitesVisited <= noLocationsToVisit && siteAudioPlayed[currentSiteVar] == true && !SiteAudio_17.transform.GetChild(currentSiteVar).gameObject.GetComponent<AudioSource>().isPlaying)
            {
            	ManualTrigger_Button_5_3.transform.gameObject.SetActive(true);
            }
            else if(guidedStory == true && countSitesVisited >= 0 && countSitesVisited <= noLocationsToVisit && siteAudioPlayed[countSitesVisited] == true && !SiteAudio_17.transform.GetChild(countSitesVisited).gameObject.GetComponent<AudioSource>().isPlaying)
            {
            	ManualTrigger_Button_5_3.transform.gameObject.SetActive(true);
            }

            // Check if arrived at next site
            if(countSitesVisited >= 0 && countSitesVisited < noLocationsToVisit && mapSitesVisited[countSitesVisited] == false)
            {
                // Trigger next part of story if close enough or pressed button to activate
                if((Vector3.Distance(ARPlayerMapMarker.transform.position, CurrentSite_14.transform.position) < 30.0f && siteAudioPlayed[countSitesVisited] == true && !SiteAudio_17.transform.GetChild(countSitesVisited).gameObject.GetComponent<AudioSource>().isPlaying) || arrivedAtLocationVar == true)
                {
                	startVar = false;

                	mapSitesVisited[countSitesVisited] = true;

                	ManualTrigger_Button_5_3.transform.gameObject.SetActive(false);
                    MapToolsPanel_5.SetActive(false);           
                    
                    SiteName_Text_7_3.text = "" + MapSites[currentSiteVar].name;
                    Title_Text_7_2.text = "Chapter " + (countSitesVisited+1);
                    Intro_Text_7_4.SetActive(true);
                    SiteInfoPanel_7.GetComponent<SiteInfo>().siteIntro = true;
                    SiteInfoPanel_7.GetComponent<SiteInfo>().enabled = true;
                    SiteInfoPanel_7.SetActive(true);

                    arrivedAtLocationVar = false;
                    mapCamera.enabled = false;
                    mapCamera.transform.gameObject.GetComponent<TouchCamera>().enabled = false;

                    CurrentSite_14.SetActive(false);

                    if(countSitesVisited == 0)
                    {
                        arSession.Reset();
                    }

                    PlaceObjectPanel_9.GetComponent<PlaceObject>().countSitesVisited = countSitesVisited;
                    PlaceObjectPanel_9.GetComponent<PlaceObject>().currentSiteVar = currentSiteVar;

                    this.enabled = false;

                }

            }
        }

        /// <summary>
        /// Set nearest map site identified as first location to visit
        /// </summary>
        void setStartingSite()
        {
            heapPermutation(siteOrder, siteOrder.Length, siteOrder.Length);

            CurrentSite_14.transform.position = MapSites[shortestSiteOrder[0]].transform.position;
            CurrentSite_14.SetActive(true);
            countSitesVisited = 0;

            visitedSites.Add(shortestSiteOrder[0]);

            currentSiteVar = shortestSiteOrder[0];

            finishedSortingSitesVar = true;

        }

        /// <summary>
        /// Set later map sites to visit next according to whether story is guided or map sites selected by the user
        /// </summary>
        public void setNextSite()
        {
            if(guidedStory == false)
            {
                for(int i = 0; i < TargetSites_13.transform.childCount; i++)
                {
                    if(!visitedSites.Contains(i))
                        TargetSites_13.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            else if(guidedStory == true)
            {
                currentSiteVar = shortestSiteOrder[countSitesVisited];
                CurrentSite_14.transform.position = MapSites[currentSiteVar].transform.position;
                CurrentSite_14.SetActive(true);
                startVar = true;
            }
        }

        /// <summary>
        /// Recentre map camera
        /// </summary>
        public void ReCentreCam()
        {
            resetCamVar = false;
            StartCoroutine(ResetCamPosition());
        }

        // Reset map camera above user when their location can be found
        IEnumerator ResetCamPosition()
        {
            while(resetCamVar == false)
            {
                Vector3 playerPos = ARPlayerMapMarker.transform.position;
                playerPos.y = 125.0f;
                mapCamera.transform.position = playerPos;
                yield return null;

                if(ARPlayerMapMarker.transform.GetChild(0).gameObject.GetComponent<Renderer>().isVisible)
                {
                    resetCamVar = true;
                }
            }

        }

        // Play audio track associated with target map site
        IEnumerator PlayNextSiteAudio()
        {
            if(audioVar == false)
            {
                audioVar = true;
                yield return new WaitForSeconds(1.0f);
                if(guidedStory == false)
                {
                    SiteAudio_17.transform.GetChild(currentSiteVar).gameObject.GetComponent<AudioSource>().Play();
                    siteAudioPlayed[currentSiteVar] = true;
                }
                else if(guidedStory == true)
                {
                	SiteAudio_17.transform.GetChild(countSitesVisited).gameObject.GetComponent<AudioSource>().Play();
                	siteAudioPlayed[countSitesVisited] = true;
                }
            }
        }

        /// <summary>
        /// Select whether to trigger target site without walking to it
        /// </summary>
        void ManualTriggerLocation()
        {

            SiteTriggerPanel_6.SetActive(true);
            MapToolsPanel_5.SetActive(false);

        }

        /// <summary>
        /// Return to map rather than manually trigger story to continue
        /// </summary>
        void LocationTriggerBack()
        {
            SiteTriggerPanel_6.SetActive(false);
            MapToolsPanel_5.SetActive(true);
        }

        /// <summary>
        /// Select whether to trigger target site without walking to it
        /// </summary>
        void LocationTriggerGo()
        {
            arrivedAtLocationVar = true;
            SiteTriggerPanel_6.SetActive(false);
        }

        /// <summary>
        /// Identify the shortest walk between the user's current position and all story sites they are required to visit.
        /// This order is used as part when selecting the guided story option
        /// </summary>
        void heapPermutation(int[] arr, int size, int n)
        {
            if (size == 1)
            {
            	float dist = 0.0f;
            	for(int j = 0; j < n-1; j++)
            	{
            		if (j == 0)
            			dist += Vector3.Distance(ARPlayerMapMarker.transform.position, MapSites[arr[j]].transform.position);
            		else
            			dist += Vector3.Distance(MapSites[arr[j]].transform.position, MapSites[arr[j+1]].transform.position);
            	}
                if (dist < maxDist)
                {
                    maxDist = dist;
                    for(int j = 0; j < shortestSiteOrder.Length; j++)
                    {
                        shortestSiteOrder[j] = arr[j];
                    }

                }
            }

            for (int i = 0; i < size; i++)
            {
                heapPermutation(arr, size - 1, n);

                // If size is odd, swap first and last element
                if (size % 2 == 1)
                {
                    int temp = arr[0];
                    arr[0] = arr[size - 1];
                    arr[size - 1] = temp;
                }

                // If size is even, swap ith and last element
                else
                {
                    int temp = arr[i];
                    arr[i] = arr[size - 1];
                    arr[size - 1] = temp;
                }
            }
        }



    }
}

