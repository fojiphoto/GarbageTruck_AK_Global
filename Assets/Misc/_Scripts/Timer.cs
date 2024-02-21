using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using admob;
//using UnityEngine.//Advertisements;
//using Heyzap;
//using ChartboostSDK;
//using admob;
//using Heyzap;
//using UnityEngine.Advertisements;
public class Timer : MonoBehaviour {

								// 1    2    3    4    5     6    7   8    9    10   11   12   13   14   15   16   17   18   19   20   21   22   23   24   25   26   27   28   29   30
	private float[] TimeValues = {220f,320f,420f,520f,370f,350f,350f,330f,340f,380f,540f,340f,340f,340f,370f,330f,350f,350f,340f,380f,340f,340f,340f,530f,370f,340f,340f,340f,340f,380f};
    public Text timeText;
	//public GameObject _map;
	public GameObject levelFail;
	public GameObject _GameUI;
	public float startTime;
	public static bool Ads;
	void Start()
	{
		startTime = TimeValues [PlayerPrefs.GetInt ("LevelNumber") - 1]+60*5;
		Ads = true;
	}
	void Update () 
    {
		
		startTime = startTime - Time.deltaTime;
		
		string minutes = ((int)startTime / 60).ToString();
		string seconds = (startTime % 60).ToString("f0");
		
		timeText.text = minutes + " : " + seconds;
		
		if (startTime <= 0&&Ads==true) {


			Time.timeScale = 0.1f;
			AudioListener.volume = 0;


		//	_map.SetActive (false);
			levelFail.SetActive(true);
			//Advertisements.Instance.ShowInterstitial();
			_GameUI.SetActive (false);
			Ads = false;
		}
	}
}
