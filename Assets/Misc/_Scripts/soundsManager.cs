using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class soundsManager : MonoBehaviour {

	public GameObject RCCCanvas;
	public GameObject Canvas321;
	public Image[] images;
	private AudioSource aSource;
	public AudioClip[] countDown;
	public int soundIndex;
	public float delay;
	// Use this for initialization

	void OnEnable () 
	{
		if (GetComponent<AudioSource>()) 
		{
			aSource = GetComponent<AudioSource> ();
		}
		soundIndex = 0;
		Invoke ("countDownSounds",1.0f);
		//countDownSounds ();
	}

	IEnumerator StartCountDown()
	{
		aSource.clip = countDown [soundIndex];
		aSource.Play ();
		if (soundIndex>0) {
			images[soundIndex-1].gameObject.SetActive(false);
		}

		images[soundIndex].gameObject.SetActive(true);
		yield return new WaitForSeconds(countDown [soundIndex].length+delay);
		soundIndex += 1;
		countDownSounds ();
	}

	public void countDownSounds(){
		if (soundIndex < countDown.Length) {
			StartCoroutine (StartCountDown ());
		} else{
			RCCCanvas.SetActive (true);
			Canvas321.SetActive (false);

		}
	}

	void OnDisable()
	{
		for (int i = 0; i < images.Length; i++) 
		{
			images [i].gameObject.SetActive (false);
		}
		
	}
}
