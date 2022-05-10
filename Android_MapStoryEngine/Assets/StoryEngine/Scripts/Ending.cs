using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// A basic ending template with an optional audio track to play before closing the app
public class Ending : MonoBehaviour
{
    // Close app on reaching the end
    [SerializeField]
	private Button EndButton_10_2;

    // Ending audio track to play
    [Tooltip("Assign an ending audio track here")]
    [SerializeField]
    private GameObject EndingAudio;

    // Variables to confirm that audio has finished
    private bool audioVar = false;
    private bool audioPlayed = false;

    // Add button listener and play any ending audio track
    void Start()
	{
		EndButton_10_2.onClick.AddListener(EndApp);
        StartCoroutine(PlayEndingAudio());

	}

    // Display exit button once any ending audio has finished playing
    void Update()
    {
        if(EndingAudio != null && !EndingAudio.GetComponent<AudioSource>().isPlaying && audioPlayed == true)
        {
             EndButton_10_2.transform.gameObject.SetActive(true);
        }
    }

    // Play ending audio track if one has been assigned to the associated inspector slot of the script
    IEnumerator PlayEndingAudio()
    {
        if(audioVar == false)
        {
            audioVar = true;
            yield return new WaitForSeconds(1.0f);

            if(EndingAudio != null && !EndingAudio.GetComponent<AudioSource>().isPlaying)
            {
                EndingAudio.GetComponent<AudioSource>().Play();
                audioPlayed = true;
            }
            else if(EndingAudio == null)
            {
                audioPlayed = true;
                yield return new WaitForSeconds(2.0f);
                EndButton_10_2.transform.gameObject.SetActive(true);
            }

        }

    }

    // Close app on pushing exit button
    void EndApp()
    {
    	Application.Quit();
    }
}