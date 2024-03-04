using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

	public GameObject _controlButtons;
	public GameObject _levelPausePanel;
	public GameObject _levelCompletePanel;
	public GameObject _levelFailPanel;
	public GameObject _timmer;
	public GameObject _instructionPanel;
	public GameObject _controlpanel;
	public GameObject _bgMusic;
	public GameObject _rccCanvas;
	public GameObject _carCamera;
	public GameObject[] _levels;
	public Transform[] PlayerPositions;
	public Transform[] CamerPositions;
	public GameObject[] PlayerVehicles;
	public int[] TrashPointsByLevel;
	public Material[] weather;
	public GameObject AdsPannel;


	public static int n;
	private int _selectedCarIndex;
	private int _selectedCarCamerIndex;
	private int _selectedLevelIndex;
	public CameraFilterPack_3D_Snow Snow;

	public void closeInstructionPanel()
	{
		_instructionPanel.SetActive(false);
		_controlpanel.SetActive(true);
	}

	public void closeControlPanel()
	{
		_controlpanel.SetActive(false);
		_rccCanvas.SetActive(true);

	}



	void Start()
	{

		try
		{
			FirebaseInitialize.instance.LogEvent1("User started level"+ PlayerPrefs.GetInt("LevelNumber"));
			Debug.Log("User started level" + PlayerPrefs.GetInt("LevelNumber"));

		}
		catch (System.Exception ex)
		{
			Debug.Log(ex.Message);
		}
	
	/*	if(AdsManager.instance)
		{
			AdsManager.instance.ShowBanner();
		}
	*/
	Time.timeScale = 1;
		AudioListener.volume = 1;

		Debug.Log("LevelManager OnEnable Called...!!");

		_timmer.gameObject.SetActive(true);


		Debug.Log("Curent Level Selected:--->> " + PlayerPrefs.GetInt("LevelNumber"));

		for (int i = 0; i < _levels.Length; i++)
			_levels[i].SetActive(false);

		for (int i = 0; i < PlayerVehicles.Length; i++)
			PlayerVehicles[i].SetActive(false);

		_selectedCarIndex = PlayerPrefs.GetInt("SelectedCar");
		_selectedCarCamerIndex = PlayerPrefs.GetInt("LevelNumber") - 1;
		_selectedLevelIndex = _selectedCarCamerIndex;
		_carCamera.SetActive(true);

		Debug.Log("Level Manager Selected Car Index: " + _selectedCarIndex);


		PlayerVehicles[_selectedCarIndex].SetActive(true);

		_carCamera.gameObject.GetComponent<RCC_Camera>().SetPlayerCar(PlayerVehicles[_selectedCarIndex]);

		_carCamera.transform.position = CamerPositions[_selectedLevelIndex].position;
		_carCamera.transform.rotation = CamerPositions[_selectedLevelIndex].rotation;

		PlayerVehicles[_selectedCarIndex].GetComponent<RCC_CarControllerV3>().StartEngine();
		PlayerVehicles[_selectedCarIndex].transform.position = PlayerPositions[_selectedLevelIndex].position;
		PlayerVehicles[_selectedCarIndex].transform.rotation = PlayerPositions[_selectedLevelIndex].rotation;
		_levels[_selectedLevelIndex].SetActive(true);

		// Handling props for vehicles

		_levels[_selectedLevelIndex].gameObject.transform.GetChild(_selectedCarIndex).gameObject.SetActive(true);
		Debug.Log("LETS CHECK------>>>: " + _levels[_selectedCarIndex].gameObject.transform.GetChild(_selectedCarIndex).gameObject.name);
		
		Debug.Log("LevelCheckkkkkkkkkkkk"+ PlayerPrefs.GetInt("LevelNumber"));
		if (PlayerPrefs.GetInt("LevelNumber") < 10)
		{
			RenderSettings.skybox = weather[0];
		}
		else if (PlayerPrefs.GetInt("LevelNumber") < 20)
		{
			RenderSettings.skybox = weather[1];
			Snow.enabled = true;
		}
		else if (PlayerPrefs.GetInt("LevelNumber") < 30)
		{

			RenderSettings.skybox = weather[2];
		}





	}

	public void Pause()
	{   //AbdulRehman
		//AdsManager.instance.isAdShowing = true; 
		//AdsManager.instance?.ShowInterstitialWithoutConditions("showing ad");
		CASAds.instance.ShowInterstitial();

		Invoke("Function_to_enable_appopen", 3f);
		

		Time.timeScale = 0f;
		AudioListener.volume = 0;
		_controlButtons.SetActive(false);
		_levelPausePanel.SetActive(true);
		_levelFailPanel.SetActive(false);
		_levelCompletePanel.SetActive(false);
	}


	public void Resume()
	{
		Time.timeScale = 1;
		AudioListener.volume = 1;

		_levelPausePanel.SetActive(false);
		_controlButtons.SetActive(true);
		_levelFailPanel.SetActive(false);
		_levelCompletePanel.SetActive(false);
	}

	public void NextLevel()
	{   //AbdulRehman
		//AdsManager.instance.isAdShowing = true;
		//AdsManager.instance?.ShowInterstitialWithoutConditions("showing ad");
		CASAds.instance.ShowInterstitial();
        //Invoke("Function_to_enable_appopen", 3f);

        PlayerPrefs.SetInt("Gameplay", 1);
        Time.timeScale = 1;
		AudioListener.volume = 1;
		n = 1;
		//Here i need to write a code
		AdsPannel.SetActive(true);
		
	}

	public void SkipLevel()
	{
		//	AdsManager.instance.Mediation_Rewarded();
		//if(Ads_Manager.instance)
		//      {
		//	Ads_Manager.instance.ShowIntersitialMediationFunction();
		NextLevel();


	}

	


	public void MainMenu()
	{
		Time.timeScale = 1;
		AudioListener.volume = 1;

		//Level
        SceneManager.LoadScene("Main Menu");
		//AbdulRehman
		//AdsManager.instance.isAdShowing = true;
		//AdsManager.instance?.ShowInterstitialWithoutConditions("showing ad");
		CASAds.instance.ShowInterstitial();

		Invoke("Function_to_enable_appopen", 3f);
	}

	public void Restart()
	{
		Scene scene = SceneManager.GetActiveScene();
		//SceneManager.LoadScene(scene.name);
		SceneManager.LoadScene("Gameplay");
		//AdsManager.instance.Admob_Unity_Intersitital();
	}



	public void Function_to_enable_appopen()
	{
		//AbdulRehman
		//AdsManager.instance.isAdShowing = false;
		
	}

}
