using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.MeshGeneration.Factories;

using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;

// Place and position a different 3D object over the surroundings of each map site visited using AR
public class PlaceObject : MonoBehaviour
{
    // Variable for how many map sites have been visited so far
	public int countSitesVisited;

    // Variable for whether user guided between sites or can choose where to go from sites marked on the map
    public bool guidedStory;

    // Variable identifier of current site
    public int currentSiteVar;

    // Link to AR Session Origin object
    private ARSessionOrigin arOrigin;

    // Link to AR camera
    [SerializeField]
    private Camera arCamera;

    // Buttons to finely position 3D objects once placed
    [SerializeField]
    private GameObject PositioningButtons_9_5;
    [SerializeField]
    private Button UpButton_9_5_1;
    [SerializeField]
    private Button DownButton_9_5_2;
    [SerializeField]
    private Button LeftButton_9_5_3;
    [SerializeField]
    private Button RightButton_9_5_4;
    [SerializeField]
    private Button RotLeftButton_9_5_5;
    [SerializeField]
    private Button RotRightButton_9_5_6;
    [SerializeField]
    private Button BackButton_9_5_7;
    [SerializeField]
    private Button ForwardButton_9_5_8;

    // Variables to limit how far 3D objects can be moved
    private int upCount = 0;
    private int sideCount = 0;
    private int inCount = 0;

    // Link to all 3D objects to be placed
    [SerializeField]
    private GameObject ObjectsToPlace_16;

    // Variable whther current 3D object has been placed
    private bool objectPlaced = false;

    // Position of current 3D object
    private Vector3 objectPosition;

    // Position of detected ground level
    private float groundLevel;

    // Variable whether a detected ground level has been set
    private bool groundLevelSet = false;

    // Button to continue app
    [SerializeField]
    private Button ContinueButton_9_4;

    // Button to reposition 3D object on surroundings
    [SerializeField]
    private Button RepositionButton_9_3;

    // Button to place 3D object in camera view
    [SerializeField]
    private Button PlaceObjectButton_9_2;

    // Text instructions to help user placing each 3D object
    [SerializeField]
	private Text PlacingInstructions_9_1_1;

    // Link to chapter panel offering story for each site
	[SerializeField]
    private GameObject SiteInfoPanel_7;

    // Text to display after placing 3D object and associated with current site
    [SerializeField]
    private GameObject OutroText_7_5;

    // Add listeners to buttons on panel
    void Awake()
    {
       ContinueButton_9_4.onClick.AddListener(ContinueApp);
       RepositionButton_9_3.onClick.AddListener(RePositionObject);
       PlaceObjectButton_9_2.onClick.AddListener(SetObject);
       
       UpButton_9_5_1.onClick.AddListener(MoveUp);
       DownButton_9_5_2.onClick.AddListener(MoveDown);
       LeftButton_9_5_3.onClick.AddListener(MoveLeft);
       RightButton_9_5_4.onClick.AddListener(MoveRight);
	   RotLeftButton_9_5_5.onClick.AddListener(RotLeft);
       RotRightButton_9_5_6.onClick.AddListener(RotRight);
       BackButton_9_5_7.onClick.AddListener(MoveBack);
       ForwardButton_9_5_8.onClick.AddListener(MoveForward);
    }

    // Detect AR Session origin object
    void Start()
    {     
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        arOrigin.GetComponent<ARPlaneManager>().enabled = true;

    }

    // Detect ground and place 3D object associated with current site
    void Update()
    {

        if(objectPlaced == false)
        {
            if(groundLevelSet == false)
            {
                
                GameObject[] groundPlanes = GameObject.FindGameObjectsWithTag("GroundPlane");
                if(groundPlanes.Length > 0)
                {
                    groundLevel = groundPlanes[0].transform.position.y;
                            
                    arOrigin.GetComponent<ARPlaneManager>().enabled = false;
                    groundLevelSet = true;

                    foreach(GameObject g in groundPlanes)
                    {
                        Destroy(g);
                    }
                    
                }
            }
            else if(groundLevelSet == true)
            {
            	PlacingInstructions_9_1_1.text = "As you move the virtual objects will move and rotate with you. Place them somewhere appropriate.";

                objectPosition = arCamera.transform.position;
                Vector3 camForward = arCamera.transform.forward;
                
                // Place distnce away according to size of object being placed
                if(guidedStory == true)
                {
                	objectPosition += 2.0f*camForward*ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.GetComponent<Renderer>().bounds.size.x;
                }
                else if(guidedStory == false)
                {
                	objectPosition += 2.0f*camForward*ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.GetComponent<Renderer>().bounds.size.x;
                }
                //objectPosition += 4.0f*camForward*;
                objectPosition.y = groundLevel;

                PlaceObjectButton_9_2.transform.gameObject.SetActive(true);

                if(guidedStory == true)
                {
                	ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.position = objectPosition;
                	ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.rotation = Quaternion.LookRotation(new Vector3(arCamera.transform.forward.x, 0, arCamera.transform.forward.z).normalized);
                	ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.SetActive(true);
                }
                else if(guidedStory == false)
                {
                	ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.position = objectPosition;
                	ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.rotation = Quaternion.LookRotation(new Vector3(arCamera.transform.forward.x, 0, arCamera.transform.forward.z).normalized);
                	ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.SetActive(true);
                }

                

            }       
                            
        }
    }

    /// <summary>
    /// Place current 3D object on user's surroundings in their camera view
    /// </summary>
    void SetObject()
    {
    	PlacingInstructions_9_1_1.text = "Use the arrows to position the objects more accurately relative to your surroundings.";
        objectPlaced = true;
        PositioningButtons_9_5.SetActive(true);

        PlaceObjectButton_9_2.transform.gameObject.SetActive(false);
        RepositionButton_9_3.transform.gameObject.SetActive(true);
        ContinueButton_9_4.transform.gameObject.SetActive(true);
        
    }

    /// <summary>
    /// Reposition current 3D object on user's surroundings in their camera view
    /// </summary>
    public void RePositionObject()
    {
    	PlacingInstructions_9_1_1.text = "As you move the virtual objects will move and rotate with you. Place them somewhere appropriate.";
 		objectPlaced = false;
 		PositioningButtons_9_5.SetActive(false);

        RepositionButton_9_3.transform.gameObject.SetActive(false);
        ContinueButton_9_4.transform.gameObject.SetActive(false);
        
    }

    /// <summary>
    /// Interact with placed 3D object or return to the chapter outro associated with the current site
    /// </summary>
    void ContinueApp()
    {
    	PositioningButtons_9_5.SetActive(false);
    	arOrigin.GetComponent<ARPlaneManager>().enabled = false;

    	/*******************************************************
		Activate a script to allow the user to complete a bespoke
		interaction with the 3D object placed before returning
		to the MapSitePanel and story chapter outro.
    	

        if(guidedStory == true)
        {
            ObjectsToPlace.transform.GetChild(countSitesVisited).gameObject.SetActive(false);
        }
        else if(guidedStory == false)
        {
            ObjectsToPlace.transform.GetChild(currentSiteVar).gameObject.SetActive(false);
        }

		MapSiteOutroText.SetActive(true);
        MapSitePanel.GetComponent<MapSiteInfo>().enabled = true;
        MapSitePanel.SetActive(true);
        *******************************************************/
        
        // Replace the template interaction script with a bespoke script relevant to each user object 
        if(guidedStory == true)
        {
            ObjectsToPlace_16.transform.GetChild(countSitesVisited).GetComponent<ObjectInteractionScript>().enabled = true;
        }
        else if(guidedStory == false)
        {
            ObjectsToPlace_16.transform.GetChild(currentSiteVar).GetComponent<ObjectInteractionScript>().enabled = true;
        }

        upCount = 0;
        inCount = 0;
        sideCount = 0;
        
        PlacingInstructions_9_1_1.text = "Hold your phone in front of you and a 3D object will appear. Move and place it somewhere appropriate.";   

    	this.transform.gameObject.GetComponent<PlaceObject>().enabled = false;
        this.transform.gameObject.SetActive(false);

    }

    /// <summary>
    /// Use associated button to fine tune placed object's position
    /// </summary>
    void MoveUp()
    {
        if(upCount <= 20)
        {
        	if(guidedStory == true)
        	{
        		Vector3 tempPosition = ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.position;
            	tempPosition.y += 0.1f;
            	ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.position = tempPosition;
            	upCount += 1;
        	}
        	else if(guidedStory == false)
            {
        		Vector3 tempPosition = ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.position;
            	tempPosition.y += 0.1f;
            	ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.position = tempPosition;
            	upCount += 1;
        	}
        }
    	
    }

    /// <summary>
    /// Use associated button to fine tune placed object's position
    /// </summary>
    void MoveDown()
    {
        if(upCount >= -20)
        {
        	if(guidedStory == true)
        	{
        		Vector3 tempPosition = ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.position;
            	tempPosition.y -= 0.1f;
            	ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.position = tempPosition;
            	upCount -= 1;
        	}
        	else if(guidedStory ==  false)
        	{
        		Vector3 tempPosition = ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.position;
            	tempPosition.y -= 0.1f;
            	ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.position = tempPosition;
            	upCount -= 1;
        	}
            
        }
    	
    }

    /// <summary>
    /// Use associated button to fine tune placed object's position
    /// </summary>
    void MoveLeft()
    {
        if(sideCount >= -20)
        {
        	if(guidedStory == true)
        	{
        		Vector3 tempPosition = ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.position;
            	tempPosition += 0.12f*Vector3.Cross(arCamera.transform.forward, Vector3.up).normalized;
            	ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.position = tempPosition;
            	sideCount -= 1;
        	}
        	else if(guidedStory == false)
        	{
        		Vector3 tempPosition = ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.position;
            	tempPosition += 0.12f*Vector3.Cross(arCamera.transform.forward, Vector3.up).normalized;
            	ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.position = tempPosition;
            	sideCount -= 1;
        	}
            
        }
        
    }

    /// <summary>
    /// Use associated button to fine tune placed object's position
    /// </summary>
    void MoveRight()
    {
        if(sideCount <= 20)
        {
        	if(guidedStory == true)
        	{
        		Vector3 tempPosition = ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.position;
            	tempPosition -= 0.12f*Vector3.Cross(arCamera.transform.forward, Vector3.up).normalized;
            	ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.position = tempPosition;
            	sideCount += 1;
        	}
        	else if(guidedStory == false)
        	{
        		Vector3 tempPosition = ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.position;
            	tempPosition -= 0.12f*Vector3.Cross(arCamera.transform.forward, Vector3.up).normalized;
            	ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.position = tempPosition;
            	sideCount += 1;
        	}
            
        }
        
    }

    /// <summary>
    /// Use associated button to fine tune placed object's position
    /// </summary>
    void RotLeft()
    {
    	if(guidedStory == true)
        	ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.Rotate(0, 10.0f, 0);
        else if(guidedStory == false)
        	ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.Rotate(0, 10.0f, 0);
    }

    /// <summary>
    /// Use associated button to fine tune placed object's position
    /// </summary>
    void RotRight()
    {
    	if(guidedStory == true)
        	ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.Rotate(0,-10.0f, 0);
        else if(guidedStory == false)
        	ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.Rotate(0,-10.0f, 0);
    }

    /// <summary>
    /// Use associated button to fine tune placed object's position
    /// </summary>
    void MoveBack()
    {
        if(inCount <= 20)
        {
        	if(guidedStory == true)
        	{
        		Vector3 tempPosition = ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.position;
            	float tempHeight = tempPosition.y;
            	tempPosition += 0.1f*arCamera.transform.forward;
            	tempPosition.y = tempHeight;
            	ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.position = tempPosition;
            	inCount += 1;
        	}
        	else if(guidedStory == false)
        	{
        		Vector3 tempPosition = ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.position;
            	float tempHeight = tempPosition.y;
            	tempPosition += 0.1f*arCamera.transform.forward;
            	tempPosition.y = tempHeight;
            	ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.position = tempPosition;
            	inCount += 1;
        	}
            
        }
        
    }

    /// <summary>
    /// Use associated button to fine tune placed object's position
    /// </summary>
    void MoveForward()
    {
        if(inCount >= -20)
        {
        	if(guidedStory == true)
        	{
        		Vector3 tempPosition = ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.position;
            	float tempHeight = tempPosition.y;
            	tempPosition -= 0.1f*arCamera.transform.forward;
            	tempPosition.y = tempHeight;
            	ObjectsToPlace_16.transform.GetChild(countSitesVisited).gameObject.transform.position = tempPosition;
            	inCount -= 1;
        	}
        	else if(guidedStory == false)
        	{
        		Vector3 tempPosition = ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.position;
            	float tempHeight = tempPosition.y;
            	tempPosition -= 0.1f*arCamera.transform.forward;
            	tempPosition.y = tempHeight;
            	ObjectsToPlace_16.transform.GetChild(currentSiteVar).gameObject.transform.position = tempPosition;
            	inCount -= 1;
        	}
            
        }
        
    }

}
