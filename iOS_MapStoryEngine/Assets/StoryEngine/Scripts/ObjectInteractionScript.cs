using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;

// A basic template to allow the user to interact with the placed AR object
public class ObjectInteractionScript : MonoBehaviour
{
	// Variable for whether user guided between sites or can choose where to go from sites marked on the map
    public bool guidedStory;

    // Link to chapter panel offering story for each site
	[SerializeField]
    private GameObject SiteInfoPanel_7;

    // Text to display after placing 3D object and associated with current site
    [SerializeField]
    private GameObject OutroText_7_5;

    [SerializeField]
    private GameObject ExampleInteractionPanel_11;

    [SerializeField]
    private Camera arCamera;

    [SerializeField]
    private Text Instructions_11_1_1;

    private Vector2 touchPosition = default;

    private float angle = 0.0f;
    private float distanceMoved = 0.0f;

    private int interactionPhase = 0;

    void Start()
    {
    	ExampleInteractionPanel_11.SetActive(true);
    }

    void Update()
    {
        if(interactionPhase == 0)
        {
            this.transform.gameObject.transform.Rotate(0, -angle, 0, Space.Self);

            if(this.transform.gameObject.GetComponent<Renderer>().isVisible && Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                
                touchPosition = touch.position;

                if(touch.phase == TouchPhase.Moved)
                {
                	angle += 0.05f*Input.GetTouch(0).deltaPosition.magnitude;        
                    distanceMoved += 0.05f*Input.GetTouch(0).deltaPosition.magnitude;

                    if(angle > 30.0f)
                    	angle = 30.0f;
                }
            }
            else if(Input.touchCount == 0 && angle > 0.0f)
            {
                angle -= 0.5f;
                if(angle < 0.0f)
                    angle = 0.0f;
            }

            if(distanceMoved > 100.0f)
            {
                this.transform.gameObject.GetComponent<Renderer>().enabled = false;
                this.transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = true;
                interactionPhase = 1;
            }
            

        }
        else if(interactionPhase == 1)
        {
            StartCoroutine(Pause2s());
        }
        else if(interactionPhase == 3)
        {
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                touchPosition = touch.position;

                if(touch.phase == TouchPhase.Began)
                {
                    Ray ray = arCamera.ScreenPointToRay(touch.position);
                    RaycastHit hitObject;
                    if(Physics.Raycast(ray, out hitObject))
                    {
                        if(hitObject.transform.gameObject != null && GameObject.ReferenceEquals(hitObject.transform.gameObject, this.transform.gameObject))
                        {
                        	ExampleInteractionPanel_11.SetActive(false);
                            Instructions_11_1_1.text = "Use your finger on the screen to rotate the cube.";
    						
    						OutroText_7_5.SetActive(true);
    						SiteInfoPanel_7.GetComponent<SiteInfo>().enabled = true;
    						SiteInfoPanel_7.SetActive(true);

    						this.enabled = false;
    						this.transform.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    // Pause for 2 seconds
    IEnumerator Pause2s()
    {
        if(interactionPhase == 1)
        {
            interactionPhase = 2;
            yield return new WaitForSeconds(2.0f);

            Instructions_11_1_1.text = "Congratulations, press the red face of the cube to continue";
            interactionPhase = 3;

        }

    }

}
