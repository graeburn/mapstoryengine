using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Change the layer of the suggested walking route map line so it does not show up in the AR camera view
public class MoveDirectionsObject : MonoBehaviour
{

    // Move suggested walking route overlaid on the map to a map layer so it is not visible during AR sections
    void Update()
    {
        if(GameObject.Find("direction waypoint  entity") != null && GameObject.Find("direction waypoint  entity").layer != 9)
                GameObject.Find("direction waypoint  entity").layer = 9;
    }
}
