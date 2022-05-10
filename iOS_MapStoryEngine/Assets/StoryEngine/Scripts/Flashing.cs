using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Flash a UI image on and off
public class Flashing : MonoBehaviour
{
	// Associated flashing style curve
	[SerializeField]
	AnimationCurve _curve;

	// Image to flash
	[SerializeField]
	private Image TargetMarker_2_4_1;

	// Flash dot on and off
	void Update()
	{
		var t = _curve.Evaluate(Time.time);
		TargetMarker_2_4_1.color = Color.Lerp(Color.clear, Color.white, t);
	}
}
