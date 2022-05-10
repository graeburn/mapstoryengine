using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Flash a map object n and off
public class FlashingMarker : MonoBehaviour
{
	// Associated curve to flash object
	[SerializeField]
	AnimationCurve _curve;

	// Object to flash on and off
	[SerializeField]
	private GameObject Marker_14_1;

	// Colour variable to alter object opacity
	private Color col;

	// Get initial object colour
	void Start()
	{
		col = Marker_14_1.GetComponent<MeshRenderer>().material.color;
	}

	// Alter alpha channel of object colour to vary its opacity
	void Update()
	{
		var t = _curve.Evaluate(Time.time);
		Marker_14_1.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.clear, col, t);
	}
}
