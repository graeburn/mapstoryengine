using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.MeshGeneration.Factories;

using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;

// A template instructions page providing information how to use the app
public class Instructions : MonoBehaviour
{
	// Object to change layer of displayed navigation route to target so hidden during AR sections of app
	[SerializeField]
    private GameObject MoveDirectionsLayer_19;

    // Display warning panel to user to take care using app in public
    [SerializeField]
    private GameObject WarningPanel_3;

    // Second set of instructions to be displayed
    [SerializeField]
    private GameObject Part2_2_5;

    // Third set of instructions to be displayed
    [SerializeField]
    private GameObject Part3_2_6;

    // Button to show next section of instruction text
	[SerializeField]
    private Button NextButton_2_1;

    // Button to start and displaying warning panel
	[SerializeField]
	private Button StartButton_2_2;

	// Image used to fade on instructions
    [SerializeField]
    private Image Black_2_7;

    // Variable to keep track of set of instructions displayed
    private int stageVar = 1;

    // Variable to add 2 second pause
    private bool pauseVar = false;

    // Add listeners to buttons
	void Awake()
	{
        NextButton_2_1.onClick.AddListener(NextInstructions);
		StartButton_2_2.onClick.AddListener(StartApp);
        
        Black_2_7.GetComponent<CanvasRenderer>().SetAlpha(1.0f);

	}

	// Wait for navigation path to be displayed and move to map layer so hidden during AR sections
    void Start()
    {
       Black_2_7.CrossFadeAlpha(0.0f, 2.0f, false);

       StartCoroutine(Pause2());

       MoveDirectionsLayer_19.GetComponent<MoveDirectionsObject>().enabled = true;
       MoveDirectionsLayer_19.SetActive(true);
    }

    // Add 2 second pause
    IEnumerator Pause2()
    {
        if(pauseVar == false)
        {
            pauseVar = true;
            yield return new WaitForSeconds(2.0f);
 			Black_2_7.transform.gameObject.SetActive(false);

        }

    }

    /// <summary>
	/// Show each set of instructions sequentially followed by start button
	/// </summary>
    void NextInstructions()
    {
        if(stageVar == 1)
        {
            Part2_2_5.SetActive(true);
            stageVar = 2;
        }
        else if(stageVar == 2)
        {
            Part3_2_6.SetActive(true);
            stageVar = 3;
            
            NextButton_2_1.transform.gameObject.SetActive(false);
            StartButton_2_2.transform.gameObject.SetActive(true);
        }
    }

    /// <summary>
	/// Show warning panel on clicking start button
	/// </summary>
    void StartApp()
    {
        WarningPanel_3.GetComponent<Warning>().enabled = true;
        WarningPanel_3.SetActive(true);

        this.enabled = false;
    	this.transform.gameObject.SetActive(false);

    }

}