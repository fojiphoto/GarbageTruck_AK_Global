using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using admob;

public class LoadingScene : MonoBehaviour {


	void Start () {

	//ADS
	/*	
	 	Admob.Instance ().removeBanner ();
		Admob.Instance ().showBannerRelative (AdSize.MediumRectangle, AdPosition.TOP_LEFT, 0);
		if(Admob.Instance().isInterstitialReady())
		{
			Admob.Instance ().showInterstitial ();
			Admob.Instance ().loadInterstitial ();
		}
		
	*/
		StartCoroutine (Loading ());
	}

	IEnumerator Loading()
	{
		yield return new WaitForSeconds(1.0f); 
		SceneManager.LoadScene (1);
	}
	

}
