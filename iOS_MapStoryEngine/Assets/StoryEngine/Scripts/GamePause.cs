using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.MeshGeneration.Factories;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;

// A basic template to allow the app to sucessfully minimised during use
public class GamePause : MonoBehaviour
{
    // Link to current ARSession object
    [SerializeField]
    private ARSession arSession;

    // Check if app has been closed or minimised
    private bool focusChange = true;

    
    // Reset ARSession if app has been closed or minimised
    #if UNITY_EDITOR || UNITY_IOS
        void OnApplicationFocus(bool focus)
        {
            if(focus && focusChange == false)
            {
            	arSession.enabled = true;

            	focusChange = true;
            }    
            else if(!focus && focusChange == true)
            {
                arSession.Reset();
            	arSession.enabled = false;

                focusChange = false;
            }
        }
    #endif
}