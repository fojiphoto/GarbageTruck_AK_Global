using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionPanel : MonoBehaviour
{

	public GameObject _modeSelectionPanel;
	public GameObject _carSelection;
	public GameObject _loadingPanel;
    public GameObject _carSelectionPanel;

	public static int x,y,z;
	public GameObject[] _modeLockImages;
	public GameObject _levelSelectionPanelMode1;
	public GameObject _levelSelectionPanelMode2;
	public GameObject _levelSelectionPanelMode3;
	public GameObject MainPenal;

    //ConsoliAdsBannerView consoliAdsBannerView;
  //  AdSize size = new AdSize(320, 50);



    void Start()
    {
		if(LevelManager.n==1){
			if(x==1){
				_levelSelectionPanelMode1.SetActive (true);
				  MainPenal.SetActive (false);
			}
			if(y==1){
				_levelSelectionPanelMode2.SetActive (true);
				MainPenal.SetActive (false);
			}
			if(z==1){
				_levelSelectionPanelMode3.SetActive (true);
				MainPenal.SetActive (false);
			}
		}

	}
    void OnEnable()
    {

		int _completeLevel = PlayerPrefs.GetInt("LevelCompleted");

		Debug.Log ("Number of Levels Completed---->>:" + _completeLevel);
		// Unlocking Modes	
		if (_completeLevel >= 10 && _completeLevel <= 19)
        {
			_modeLockImages[0].SetActive(false);
		} 
		else if(_completeLevel >= 20 && _completeLevel <= 30)
        {
			_modeLockImages[1].SetActive(false);
		}

		
    }

	public void ModeSelected(int mode)
	{
		
		buttonClicked();
		
		PlayerPrefs.SetInt("mode",mode);
		
		if (mode == 1) {
			//AbdulRehman
			//AdsManager.instance.isAdShowing = true; 
			//AdsManager.instance?.ShowInterstitialWithoutConditions("showing ad");
			CASAds.instance.ShowInterstitial();


			//Invoke("Function_to_enable_appopen", 3f); 
			_modeSelectionPanel.SetActive (false);
			_levelSelectionPanelMode1.SetActive	(true);
			_levelSelectionPanelMode2.SetActive (false);
			_levelSelectionPanelMode3.SetActive (false);
			x = 1;
		}
		else if (mode == 2) {
			//AbdulRehman
			//AdsManager.instance.isAdShowing = true;
			//AdsManager.instance?.ShowInterstitialWithoutConditions("showing ad");
			CASAds.instance.ShowInterstitial();

			Invoke("Function_to_enable_appopen", 3f);
			_modeSelectionPanel.SetActive (false);
			_levelSelectionPanelMode1.SetActive (false);
			_levelSelectionPanelMode2.SetActive (true);
			_levelSelectionPanelMode3.SetActive (false);
			y = 1;
		} 
		else if (mode == 3) {
			//AbdulRehman
			//AdsManager.instance.isAdShowing = true;
			//AdsManager.instance?.ShowInterstitialWithoutConditions("showing ad");
			CASAds.instance.ShowInterstitial();


			Invoke("Function_to_enable_appopen", 3f);
			_modeSelectionPanel.SetActive (false);
			_levelSelectionPanelMode1.SetActive (false);
			_levelSelectionPanelMode2.SetActive (false);
			_levelSelectionPanelMode3.SetActive (true);
			z = 1;
		}

	}
    public void LeveSelected(int levelNumber) 
    {
		buttonClicked ();
        PlayerPrefs.SetInt("LevelNumber", levelNumber);
		Debug.Log ("Level NO"+ levelNumber);
		StartCoroutine (click());
		Debug.Log("LevelCheck__________" + PlayerPrefs.GetInt("LevelNumber"));

	}

	public void backToCarSelectionPanel()
	{
		_modeSelectionPanel.SetActive (false);
		_carSelectionPanel.SetActive(true);
		_carSelection.SetActive (true);
		buttonClicked ();
	}
    public void Back()
    {
		buttonClicked ();
		_modeSelectionPanel.SetActive (true);
		_levelSelectionPanelMode1.SetActive (false);
		_levelSelectionPanelMode2.SetActive (false);
		_levelSelectionPanelMode3.SetActive (false);

    }

	public void buttonClicked(){
		this.gameObject.GetComponent<AudioSource> ().Play ();
	}

	IEnumerator click()
	{
		yield return new WaitForSeconds (0.7f);
		_loadingPanel.SetActive (true);

        if (PlayerPrefs.GetInt("RemoveAds") == 1)
        {
            Debug.Log("Ads Removed");
        }
        else
        {
			//Advertisements.Instance.ShowInterstitial();
            //Advertisements.Instance.HideBanner();
		}
    }
	public void Function_to_enable_appopen()
	{
		//AbdulRehman
		//AdsManager.instance.isAdShowing = false;
		
	}

}
