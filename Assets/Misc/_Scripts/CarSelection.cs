using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{

	public GameObject _modeSelection;
	public Material _dumperMaterial;
	public Texture[] _dumperTruckTextures;
	public Material _cargoMaterial;
	public Texture[] _cargoTruckTextures;
	public Material _garbageTruckMaterial;
	public Texture[] _garbageTruckTextures;
	public GameObject[] _vehicleColorPanels;
	public GameObject _animatingHand;
	public GameObject _mainMenuCanvas;
	public GameObject _rotatingCamera;
	public GameObject _leftButton;
	public GameObject _rightButton;
	//public GameObject _liveMenu;
	public GameObject _mainMenuPanel;
	public GameObject _levelSelectionPanel;
	public GameObject _notEnoughPanel;
	public GameObject[] _vehicles;
	public GameObject _select;
	public GameObject _buy;
	public Button _priceButton;
	public GameObject _carSelection;
	public Text _coins;

	private int _current;
	private int _prev;
	private int[] _prices = { 0,10000, 12000};
	private int _coinsCash;
	private Transform[] _carsTranforms;

	// Use this for initialization
	void OnEnable () 
	{
		//DEFAULT TEXTURES Handling 

		_garbageTruckMaterial.mainTexture = _garbageTruckTextures [0];
		_dumperMaterial.mainTexture = _dumperTruckTextures[0];
		_cargoMaterial.mainTexture = _cargoTruckTextures [0];

		StartCoroutine (animateHand ());
	
		_rotatingCamera.SetActive(true);
		Debug.Log ("-->> OnEnable Method of CarSelection is Called\n\n");
		_leftButton.SetActive (false);
		_rightButton.SetActive (true);
		_current = 0;
		_coinsCash = PlayerPrefs.GetInt ("Coins");


		// Coins dtails showing on the car selection canvas
		_coins.text = " " + PlayerPrefs.GetInt("Coins");

		// Activating Car Selection panel if its inactive 
		if(!_carSelection.activeSelf)
			_carSelection.SetActive (true);

		// Deactivating all Vehicles if any one is enabled
		for (int i = 0; i < _vehicles.Length; i++)
			_vehicles [i].SetActive (false);

	
		// Activating first Vehicle
		_vehicles [0].SetActive (true);

		// Activating first Vehicle color panel
		_vehicleColorPanels [0].SetActive (true);

		// we dont need to show price & buy of first car as its free for ride
		_priceButton.gameObject.SetActive (false);
		_buy.SetActive (false);
		_select.SetActive (true);

	//	_rotatingCamera.GetComponent<CameraMove> ().target = _vehicles [0].gameObject.transform;
		// showing the detail of cars
		Debug.Log (" Car Selection Rotating Camera Target:->>> " + _rotatingCamera.GetComponent<CameraMove> ().target );
		Debug.Log ("CurrentCarIndex-->>: " + _current + " Price--->> 0");
		Debug.Log ("Color Panel of " + _vehicleColorPanels [_current].gameObject.name  + " is activated ");
	}


	public void right()
    {

		buttonClicked ();

		if (_current < _vehicles.Length-1) 
		{


			_vehicles [_current].SetActive (false);
			_vehicles [_current + 1].SetActive (true);


			_vehicleColorPanels [_current].SetActive (false);
			_vehicleColorPanels [_current + 1].SetActive (true);

			
			_current = _current+1;
			if (_current>0) {
				_leftButton.SetActive (true);
				_rightButton.SetActive (true);
			}
			if (_current==_vehicles.Length-1) 
				_rightButton.SetActive (false);
			
			if (PlayerPrefs.GetInt ("Car" + _current)>0) {
				_priceButton.gameObject.SetActive (false);
				_buy.SetActive (false);
				_select.SetActive (true);
			}else{
				_priceButton.gameObject.SetActive (true);
				_priceButton.transform.GetChild(0).GetComponent<Text>().text= _prices [_current].ToString ();
				_select.SetActive (false);
				_buy.SetActive (true);
			}
				
		}
		Debug.Log ("CurrentCarIndex-->>: " + _current + " Price--->> "+_prices[_current]);
		Debug.Log ("Color Panel of " + _vehicleColorPanels [_current].gameObject.name  + " is activated ");
	}
	public void left()
	{
		buttonClicked ();

		if (_current>=1) {


			_vehicles [_current].SetActive (false);
			_vehicles [_current - 1].SetActive (true);

	

			_vehicleColorPanels[_current].SetActive(false);
			_vehicleColorPanels [_current - 1].SetActive (true);

			_current = _current-1;

			if (_current>0) {
				_leftButton.SetActive (true);
				_rightButton.SetActive (true);
			}
			if (_current== 0) 
				_leftButton.SetActive (false);

			if (PlayerPrefs.GetInt ("Car" + _current)>0) {
				_priceButton.gameObject.SetActive (false);
				_buy.SetActive (false);
				_select.SetActive (true);
			}else{
				_priceButton.gameObject.SetActive (true);
				_priceButton.transform.GetChild (0).GetComponent<Text> ().text = _prices [_current].ToString ();
				_select.SetActive (false);
				_buy.SetActive (true);
			}
		}
		Debug.Log ("CurrentCarIndex-->>: " + _current + " Price--->> "+_prices[_current]);
	}
	public void buy()
	{
		// if total coin cash is greater than or eqaul to current car price 
		if (_coinsCash >= _prices [_current]) 
		{
			// we dont need to show price & buy of first car as it is purchased for ride
			_buy.SetActive (false);

			//Activating the Select Button true
			_select.SetActive (true);

			_priceButton.gameObject.SetActive (false);

			// deduct the actual car price from total coin cash
			_coinsCash = _coinsCash - _prices [_current];   

			//Displaying the Total Remaing cash
			_coins.transform.GetChild (0).GetComponent<Text> ().text = _coinsCash.ToString();

			//Displaing the CarIndex which is being purchased
			Debug.Log ("The Car Index: " + _current + " Price: " + _prices[_current] + "IsPurchased");

			//Setting the PlayerPref for total cash after purcahsing the vehical 
			PlayerPrefs.SetInt ("Coins", _coinsCash);  

			//Displaying Total Cash on console
			Debug.Log ("TotalCash: " + PlayerPrefs.GetInt ("Coins"));


			PlayerPrefs.SetInt ("Car" + _current, 1); // set the playerpreference for car been purchased

		} else 
		{

			_notEnoughPanel.SetActive (true);

		}
	}

	public void exitNotEnough()
    {

		_notEnoughPanel.SetActive (false);
	}

	public void back()
	{
		_mainMenuCanvas.gameObject.GetComponent<AudioSource> ().Play ();

		

		_rotatingCamera.SetActive(false);
		PlayerPrefs.GetInt ("Restart", 0);
		_mainMenuPanel.SetActive (true);
	
		_carSelection.SetActive(false);
		this.gameObject.SetActive (false);
	}
	public void select()
	{
		buttonClicked ();

		_rotatingCamera.SetActive(false);
		Debug.Log ("Car " + _current + " Selected ");
		PlayerPrefs.SetInt ("SelectedCar",_current);
		Debug.Log ("SelectedCar Player pref value: " + _current);
		_modeSelection.SetActive(true);
		_carSelection.SetActive (false);
		
		this.gameObject.SetActive (false);

        
    }
	public void buttonClicked()
    {
		this.gameObject.GetComponent<AudioSource> ().Play ();
	}
		
	public void TrashTruckColorChange(int index)
    {
		buttonClicked ();
		if (_current == 2)
			_garbageTruckMaterial.mainTexture = _garbageTruckTextures [index];

	}

	public void DumperColorChange(int index)
    {
		buttonClicked ();
		if (_current == 0)
			_dumperMaterial.mainTexture = _dumperTruckTextures[index];
		
	}

	public void CargoTruckColorChange(int index)
    {
		buttonClicked ();
		if (_current == 1)
			_cargoMaterial.mainTexture = _cargoTruckTextures[index];
	}




	IEnumerator animateHand()
	{
		yield return new WaitForSeconds (1.2f);
		_animatingHand.SetActive (false);
	}

}

