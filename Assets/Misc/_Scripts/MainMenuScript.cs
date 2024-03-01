using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
	
	// Use this for initialization
	public GameObject _modeSelection;
	public GameObject _garageCamera;
	public GameObject _garage;
	public GameObject _mainMenuPanel;
	public GameObject _carSelectionPanel;
    public GameObject _quitPanel;
	public GameObject _loadingPanel;
	public GameObject _levelSlectionPanel;
	public GameObject _settingPanel;
	public GameObject _soundON;
	public GameObject _soundOFF;
	public GameObject _musicOFF;
	public GameObject _musicON;
	public GameObject _backGroundMusic;
	public Slider _musicSlider;
	private float _dummySliderValue;
	public GameObject PrivicyPenal;

	public TextMeshProUGUI _coins;


	private void Awake()
    {
		


		if (PlayerPrefs.GetInt("PP")==0)
        {
			//PrivicyPenal.SetActive(true);
        }
		else
		{
			//PrivicyPenal.SetActive(false);
    	}
    }
    void Start()
    {
		Invoke("YourFunction", 2f);

		
		AudioListener.volume = 1;
		if (PlayerPrefs.GetInt("LevelCompleted") == 0)
			PlayerPrefs.SetInt("LevelCompleted", 1); 
		PlayerPrefs.SetInt("Car0", 1);
		Time.timeScale = 1;

		_coins.text = " " + PlayerPrefs.GetInt("Coins");


	}	
	public void YourFunction()
	{
		//AbdulRehman
		//AdsManager.instance?.ShowBanner();
		CASAds.instance.ShowBanner(CAS.AdPosition.BottomCenter);
		//AdsManager.instance?.ShowAppOpenAdIfReady();
	}
	public void Play()
    {                                      
		_carSelectionPanel.SetActive (true);
		_garage.SetActive (true);
		_garageCamera.SetActive (true);
		_mainMenuPanel.SetActive(false);
		
    }
	public void RevokeConcent()
	{
		CASAds.instance?.HideBanner();
		CASAds.instance?.HideMrecBanner();
		PlayerPrefs.SetInt("GDPR", 0);
		Application.LoadLevel("GDPR");
	}
	public void More()
	{
		Application.OpenURL("https://play.google.com/store/apps/developer?id=AK+Global+Games");  
	}
	public void ViewPrivacyPolicy()
	{
		Application.OpenURL("https://akglobalgames-privacy-policy.blogspot.com/2022/10/ak-global-games-privacy-policy.html");
	}

	public void RateUs()
    {
		Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);       
    }

    public void Yes()
    {
		buttonClicked ();
		PlayerPrefs.GetInt ("Restart", 0);
        Application.Quit();
    }


    public void No()
    {
		buttonClicked ();
		_quitPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
    }
	public void QuitPenal(){
		buttonClicked ();
		_quitPanel.SetActive (true);
        if (PlayerPrefs.GetInt("RemoveAds") == 1)
        {
            Debug.Log("Ads Removed");
        }
        else
        {
			//Advertisements.Instance.ShowInterstitial();
		}
    }
	public void backToMainMenu()
	{
		buttonClicked ();

		_carSelectionPanel.SetActive(false);
		_garage.SetActive (false);
		_garage.SetActive (false);
		_carSelectionPanel.SetActive (false);
	}


    void Update()
    {
		_backGroundMusic.gameObject.GetComponent<AudioSource> ().volume = _musicSlider.value;
		if (_backGroundMusic.gameObject.GetComponent<AudioSource> ().volume <= 0) {

			_musicON.SetActive (false);
			_musicOFF.SetActive (true);
		} else {

			_musicON.SetActive (true);
			_musicOFF.SetActive (false);
		}
			
	  
		if(Input.GetKeyDown(KeyCode.Escape) && _mainMenuPanel.activeSelf)
        {
            _quitPanel.SetActive(true);
        }
    }

	public void buttonClicked(){
		this.gameObject.GetComponent<AudioSource> ().Play ();
	}

	public void Settings()
	{
		buttonClicked ();
		_settingPanel.SetActive (true);
	}

	public void SoundOn()
	{
		buttonClicked ();
		AudioListener.volume = 0;
		_soundON.SetActive (false);
		_soundOFF.SetActive (true);
	}

	public void SoundOFF()
	{
		buttonClicked ();
		AudioListener.volume = 1;
		_soundON.SetActive (true);
		_soundOFF.SetActive (false);
	}

	public void MusicOn()
	{
		buttonClicked ();
		_backGroundMusic.gameObject.GetComponent<AudioSource> ().volume = 0;
		_dummySliderValue = _musicSlider.value;
		_musicSlider.value = 0;
		_musicON.SetActive (false);
		_musicOFF.SetActive (true);
	}

	public void MusicOFF()
	{
		buttonClicked ();
		_backGroundMusic.gameObject.GetComponent<AudioSource> ().volume = 1;
		_musicSlider.value = _dummySliderValue;
		_musicON.SetActive (true);
		_musicOFF.SetActive (false);
	}
	public void closSettingPanels()
	{
		buttonClicked ();
		_settingPanel.SetActive (false);
	}
	public void Privcy()
    {

		Application.OpenURL("https://akglobalgames-privacy-policy.blogspot.com/2022/10/ak-global-games-privacy-policy.html");   
	}
	public void Privicy_PenalAccept()
	{
		PrivicyPenal.SetActive (false);
		PlayerPrefs.SetInt ("PP",1);
		AudioListener.volume = 1;

        if (PlayerPrefs.GetInt("RemoveAds") == 1)
        {
            Debug.Log("Ads Removed");
        }
        else
        {
		}
    }
	public void watchvideoforrewarded()
    {   //AbdulRehman
		//AdsManager.instance.Admob_Rewarded();
		//AdsManager.instance.Admob_Unity_Rewarded(GETCOIN);
		CASAds.instance.ShowRewarded(GETCOIN);
    }
    private void OnDisable()
    {/*
        if(AdsManager.instance)
        {
			AdsManager.instance.HideBanner();
        }
		*/
    }

	public void GETCOIN()
	{
		PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 500);
		_coins.text = " " + PlayerPrefs.GetInt("Coins");
	}

}


