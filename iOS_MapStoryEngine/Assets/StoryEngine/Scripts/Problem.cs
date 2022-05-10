using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Display a bsic problem panel to close the app in cases where enough local sites could not be identified
public class Problem : MonoBehaviour
{
	// Button to quit application if not enough local map sites can be found
    [SerializeField]
	private Button ExitButton_4_4;

	// Add listener to button
    void Start()
	{
		ExitButton_4_4.onClick.AddListener(EndApp);

	}

	// Quit application
    void EndApp()
    {
    	Application.Quit();
    }
}