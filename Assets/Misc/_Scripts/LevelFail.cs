using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Heyzap;
//using UnityEngine.//Advertisements;

public class LevelFail : MonoBehaviour 
{

	public GameObject _levelFailPanel;
	public GameObject _rccCanvas;
	public void Failed()
	{
		Debug.Log ("level fail");
		_rccCanvas.SetActive (false);
		_levelFailPanel.SetActive (true);
		Time.timeScale = 0.1f;
		AudioListener.volume = 0;

//		if(HZInterstitialAd.isAvailable())
//		{
//			HZInterstitialAd.show();
//		}
//		else if(HZVideoAd.isAvailable())
//		{
//			HZVideoAd.show();
//		}
//		else if (Advertisement.IsReady ()) 
//		{
//			Advertisement.Show ();
//		} 
	}
}
