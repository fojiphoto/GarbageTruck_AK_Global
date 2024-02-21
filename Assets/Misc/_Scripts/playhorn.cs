using UnityEngine;
using System.Collections;

public class playhorn : MonoBehaviour {

    public AudioClip impact;
    public AudioSource audio;

  
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void playhorn1()
    {
        audio.PlayOneShot(impact, 0.7F);

    }
}
