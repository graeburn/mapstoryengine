
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mapbox.Unity.MeshGeneration.Factories;

// A template title page that activates the search for local named map locations
public class Title : MonoBehaviour
{
	// Instructions panel follows after title screen
    [SerializeField]
    private GameObject InstructionsPanel_2;

    // Object deals with finding local sites to visit
    [SerializeField]
    private GameObject SearchLocalSites_12;

    // StoryManager links the story progression and sites to visit
    [SerializeField]
    private GameObject StoryManager_15;

    // Button to search for nearby named sites to visit
    [SerializeField]
    private Button SearchLocationsButton_1_2;

    // Button to continue to instructions panel
    [SerializeField]
    private Button ContinueButton_1_3;

    // Display text to show site search in progress
    [SerializeField]
    private Text SearchingText_1_4;

    // Variable confirms whether search location button has been displayed
    private bool buttonVar = false;

    // Variable confirms whether searchhas taken place
    private bool searchVar = false;

    void Awake()
    {
        SearchingText_1_4.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
    }

    // Add listeners to buttons on the panel and show search location button
    void Start()
    {
        SearchLocationsButton_1_2.onClick.AddListener(SearchForLocalPlaces);
        ContinueButton_1_3.onClick.AddListener(ShowInstructionsPage);

        StartCoroutine(Pause5s());

    }

    /// <summary>
	/// Activate object to search for local sites to visit around user
	/// </summary>
    private void SearchForLocalPlaces()
    {
        SearchLocalSites_12.GetComponent<SearchLocationsScript>().enabled = true;
        SearchLocalSites_12.SetActive(true);

        SearchLocationsButton_1_2.transform.gameObject.SetActive(false);
        SearchingText_1_4.CrossFadeAlpha(1.0f, 5.0f, false);

        StartCoroutine(PauseForSearch());
    }

    // Pause for 5 seconds before displaying locaion search button
    IEnumerator Pause5s()
    {
        if(buttonVar == false)
        {
            buttonVar = true;
            yield return new WaitForSeconds(5.0f);

            SearchLocationsButton_1_2.transform.gameObject.SetActive(true);

        }

    }

    // Pause for 5 seconds as local site search performed
    IEnumerator PauseForSearch()
    {
        if(searchVar == false)
        {
            searchVar = true;
            yield return new WaitForSeconds(5.0f);

            StoryManager_15.GetComponent<StoryManager>().enabled = true;
            SearchingText_1_4.transform.gameObject.SetActive(false);

            yield return new WaitForSeconds(1.0f);

            ContinueButton_1_3.transform.gameObject.SetActive(true);

        }

    }

    /// <summary>
	/// Load instructions page
	/// </summary>
    private void ShowInstructionsPage()
    {

        InstructionsPanel_2.GetComponent<Instructions>().enabled = true;
        InstructionsPanel_2.SetActive(true);

        SearchLocalSites_12.GetComponent<SearchLocationsScript>().enabled = false;
        SearchLocalSites_12.SetActive(false);

        this.enabled = false;
        this.transform.gameObject.SetActive(false);


    }

}