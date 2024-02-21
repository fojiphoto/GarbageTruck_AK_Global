using UnityEngine;
using System.Collections;

public class VehicleChanger : MonoBehaviour {
	public GameObject[] vehicles;
	public RaycastHit hit;

	void Update() {

		vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
			
		foreach(GameObject vehicle in vehicles){
			if(vehicle.transform != Camera.main.GetComponent<CameraDefault>().TargetLookAt){
				vehicle.GetComponent<CarController>().enabled = false;
                vehicle.GetComponent<AudioSource>().mute = true;
            } else {
				vehicle.GetComponent<CarController>().enabled = true;
                vehicle.GetComponent<AudioSource>().mute = false;
            }
		}


		if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit)){
				if(hit.transform.tag == "Vehicle"){
					Camera.main.GetComponent<CameraDefault>().TargetLookAt = hit.transform;
				}
			}
		}
	}
}
