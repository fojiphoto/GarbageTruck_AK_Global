using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMusic : MonoBehaviour
{
    public GameObject _soundON;
    public GameObject _soundOFF;
    public GameObject _musicOFF;
    public GameObject _musicON;
    public GameObject _backGroundMusic;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (_backGroundMusic.gameObject.GetComponent<AudioSource>().volume <= 0)
        {

            _musicON.SetActive(false);
            _musicOFF.SetActive(true);
        }
        else
        {

            _musicON.SetActive(true);
            _musicOFF.SetActive(false);
        }
    }
	public void SoundOn()
	{
		buttonClicked();
		AudioListener.volume = 0;
		_soundON.SetActive(false);
		_soundOFF.SetActive(true);
	}

	public void SoundOFF()
	{
		buttonClicked();
		AudioListener.volume = 1;
		_soundON.SetActive(true);
		_soundOFF.SetActive(false);
	}

	public void MusicOn()
	{
		buttonClicked();
		_backGroundMusic.gameObject.GetComponent<AudioSource>().volume = 0;
		//_dummySliderValue = _musicSlider.value;
		//_musicSlider.value = 0;
		_musicON.SetActive(false);
		_musicOFF.SetActive(true);
	}

	public void MusicOFF()
	{
		buttonClicked();
		_backGroundMusic.gameObject.GetComponent<AudioSource>().volume = 1;
		//_musicSlider.value = _dummySliderValue;
		_musicON.SetActive(true);
		_musicOFF.SetActive(false);
	}
	public void buttonClicked()
	{
		_backGroundMusic.gameObject.GetComponent<AudioSource>().Play();
	}
}
