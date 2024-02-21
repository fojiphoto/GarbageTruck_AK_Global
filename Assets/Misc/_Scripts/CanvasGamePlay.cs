using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.//Advertisements;
//using admob;
//using Heyzap;

public class CanvasGamePlay : MonoBehaviour {

	// Use this for initialization

//	public GameObject _kgfMap;
	public GameObject[] _vehicles;
	public GameObject _backGroundMusic;
	public GameObject _soundON;
	public GameObject _soundOFF;
	public GameObject _musicOFF;
	public GameObject _musicON;
	public GameObject _mainMenuPanel;
	public GameObject _liveMenu;
	public GameObject _levelSelectionPanel;
	public GameObject _levelManager;
	public GameObject _loadingPanel;
    public GameObject _levelPausePanel;
    public GameObject _levelCompletePanel;
    public GameObject _levelFailPanel;
	public GameObject _rccCanvas;
	public GameObject _rccMainCamera;
	public GameObject _settingPanel;
	public Slider _musicSlider;
	private float _dummySliderValue;


	void Start () 
    {
		

		AudioListener.volume = 1;
        Time.timeScale = 1;
        _levelPausePanel.SetActive(false);
        _levelCompletePanel.SetActive(false);
        _levelFailPanel.SetActive(false);
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


	public void Pause()
    {



		buttonClicked ();


        Time.timeScale = 0f;
        AudioListener.volume = 0;
		_rccCanvas.SetActive (false);
        _levelPausePanel.SetActive(true);
        _levelFailPanel.SetActive(false);
        _levelCompletePanel.SetActive(false);      

    }

    public void Resume()
    {


		buttonClicked ();
        Time.timeScale = 1;
        AudioListener.volume = 1;
        _levelPausePanel.SetActive(false);
		_rccCanvas.SetActive (true);
        _levelFailPanel.SetActive(false);
        _levelCompletePanel.SetActive(false);      
	
    }
     		
	public void buttonClicked(){
		this.gameObject.GetComponent<AudioSource> ().Play ();
	}

	void Update()
	{
		_backGroundMusic.gameObject.GetComponent<AudioSource> ().volume = _musicSlider.value;
		if (_backGroundMusic.gameObject.GetComponent<AudioSource> ().volume <= 0) {

	//		_musicON.SetActive (false);
	//		_musicOFF.SetActive (true);
		}

	}
}
