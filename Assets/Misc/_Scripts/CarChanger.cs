using UnityEngine;
using System.Collections;

public class CarChanger : MonoBehaviour {

    public GameObject[] _vehicles;
	void OnEnable () {
		Debug.Log("-->>OnEnabled Function of CarChanger Script (Attached to -LIVE-MENU- in hierarachy) is called");
		for (int i = 0; i < _vehicles.Length; i++) {
			_vehicles [i].SetActive (false);
		}
		_vehicles [PlayerPrefs.GetInt ("LiveMenuCar")].SetActive (true);
	}
	



}
