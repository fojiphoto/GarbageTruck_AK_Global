using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTrashTruckColor : MonoBehaviour {

	public Material Color01;
	public Texture Tex01,Tex02;

	// Use this for initialization
	void Start () {

		Color01.mainTexture = Tex01;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
