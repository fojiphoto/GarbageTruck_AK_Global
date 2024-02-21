﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LevelCompletion : MonoBehaviour
{


    public GameObject _levelCompletePanel;
	public GameObject PickTrash;
	public GameObject playermaincanvas;
	public GameObject _levelFailPanel;
	public GameObject _barCompletion;
	public GameObject _controllerButtons;
    public static bool Ads = true;
	public GameObject TrashMan;
 	public RCC_Camera camera;
	public LevelManager Levelmanager;
	public GameObject StopPos;
	public int stoppingpoints;
	public GameObject TrashPickParticle;
	public Transform DirectionalArrow,Targetofarrow; 
    private void Awake()
    {
		camera = GameObject.FindObjectOfType<RCC_Camera>();
		Debug.Log(PlayerPrefs.GetInt("LevelNumber"));
		StopPos = GameObject.FindGameObjectWithTag("drop");
		stoppingpoints = Levelmanager.TrashPointsByLevel[PlayerPrefs.GetInt("LevelNumber")];
	}
    private void Start()
    {
		GameObject Target= GameObject.FindGameObjectWithTag("Trash Stop");
		Targetofarrow = Target.gameObject.transform;
	}
    private void Update()
    {
        if (StopPos==null)
        {
			StopPos = GameObject.FindGameObjectWithTag("drop");
		}
    }
    private void FixedUpdate()
    {
		DirectionalArrow.transform.LookAt(Targetofarrow.transform);
    }

    public GameObject trashcans;
	private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Trash Stop")
        {
			other.gameObject.GetComponent<Collider>().enabled = false;
			this.GetComponent<Rigidbody>().isKinematic = true;
			PickTrash.SetActive(true); 
			playermaincanvas.SetActive(false);
			camera.TPSMinimumFOV = 85;
			stoppingpoints = stoppingpoints - 1;
			trashcans = other.gameObject;

		}
		if(other.tag=="Reset")
        {
			this.transform.position = Levelmanager.PlayerPositions[PlayerPrefs.GetInt("LevelNumber")-1].transform.position;
			this.transform.rotation = Levelmanager.PlayerPositions[PlayerPrefs.GetInt("LevelNumber")-1].transform.rotation;
		}
    }
	public IEnumerator Complete()
    {
		yield return new WaitForSeconds(10f);
		this.GetComponent<Rigidbody>().isKinematic = false;
		PickTrash.SetActive(false);
		playermaincanvas.SetActive(true);
		camera.TPSMinimumFOV = 60;
		TrashMan.GetComponent<Animator>().Play("Hanging");
		TrashMan.transform.GetChild(0)
			.gameObject.SetActive(false);
		TrashMan.GetComponent<PlayerController>().DummyMonster.transform.GetChild(0).gameObject.SetActive(true);
		TrashMan.GetComponent<PlayerController>().TrashCan.SetActive(false);
		StopPos.SetActive(true);
		trashcans.SetActive(false);
		TrashPickParticle.SetActive(true);
		Targetofarrow= StopPos.gameObject.transform;
		PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1000);
	}

    void OnTriggerStay(Collider col)
	{

		if (col.gameObject.tag == "drop") {

			this.GetComponent<Rigidbody>().drag = 5;
			if (!_barCompletion.gameObject.activeSelf) 
			{
				_barCompletion.gameObject.SetActive (true);
			}


			if (_barCompletion.transform.GetChild (0).GetComponent<Image> ().fillAmount < 1) {

				_barCompletion.transform.GetChild (0).GetComponent<Image> ().fillAmount += 0.008f;

			} 
			else
            {
				int _levelcompleted = PlayerPrefs.GetInt ("LevelCompleted");
				_levelCompletePanel.SetActive (true);
				int a = PlayerPrefs.GetInt("LevelNumber");
				Debug.Log("a = " + PlayerPrefs.GetInt("LevelNumber"));
				_controllerButtons.SetActive (false);
				if (a == 1)
				{
					PlayerPrefs.SetInt("Unlock", 1);
				}
				if (a == 2)
				{
					PlayerPrefs.SetInt("Unlock", 2);
				}
				if (a == 3)
				{
					PlayerPrefs.SetInt("Unlock", 3);
				}
				if (a == 4)
				{
					PlayerPrefs.SetInt("Unlock", 4);
				}
				if (a == 5)
				{
					PlayerPrefs.SetInt("Unlock", 5);
				}
				if (a == 6)
				{
					PlayerPrefs.SetInt("Unlock", 6);
				}
				if (a == 7)
				{
					PlayerPrefs.SetInt("Unlock", 7);
				}
				if (a == 8)
				{
					PlayerPrefs.SetInt("Unlock", 8);
				}
				if (a == 9)
				{
					PlayerPrefs.SetInt("Unlock", 9);
				}
				if (a == 10)
				{
					PlayerPrefs.SetInt("Unlock", 10);
				}
				if (a == 11)
				{
					PlayerPrefs.SetInt("Unlock1", 1);
				}
				if (a == 12)
				{
					PlayerPrefs.SetInt("Unlock1", 2);
				}
				if (a == 13)
				{
					PlayerPrefs.SetInt("Unlock1", 3);
				}
				if (a == 14)
				{
					PlayerPrefs.SetInt("Unlock1", 4);
				}
				if (a == 15)
				{
					PlayerPrefs.SetInt("Unlock1", 5);
				}
				if (a == 16)
				{
					PlayerPrefs.SetInt("Unlock1", 6);
				}
				if (a == 17)
				{
					PlayerPrefs.SetInt("Unlock1", 7);
				}
				if (a == 18)
				{
					PlayerPrefs.SetInt("Unlock1", 8);
				}
				if (a == 19)
				{
					PlayerPrefs.SetInt("Unlock1", 9);
				}
				if (a == 20)
				{
					PlayerPrefs.SetInt("Unlock1", 10);
				}
				if (a == 21)
				{
					PlayerPrefs.SetInt("Unlock2", 1);
				}
				if (a == 22)
				{
					PlayerPrefs.SetInt("Unlock2", 2);
				}
				if (a == 23)
				{
					PlayerPrefs.SetInt("Unlock2", 3);
				}
				if (a == 24)
				{
					PlayerPrefs.SetInt("Unlock2", 4);
				}
				if (a == 25)
				{
					PlayerPrefs.SetInt("Unlock2", 5);
				}
				if (a == 26)
				{
					PlayerPrefs.SetInt("Unlock2", 6);
				}
				if (a == 27)
				{
					PlayerPrefs.SetInt("Unlock2", 7);
				}
				if (a == 28)
				{
					PlayerPrefs.SetInt("Unlock2", 8);
				}
				if (a == 29)
				{
					PlayerPrefs.SetInt("Unlock2", 9);
				}
				if (a == 30)
				{
					PlayerPrefs.SetInt("Unlock2", 10);
				}



			}
		}
	}
	void OnTriggerExit(Collider obj)
    {

		if (obj.gameObject.tag == "prop")
        {

			Time.timeScale = 0.2f;
			AudioListener.volume = 0;
			StartCoroutine (hide());
			_levelFailPanel.SetActive (true);
			_controllerButtons.SetActive (false);
        }

		if (obj.gameObject.tag == "drop") {
			if (_barCompletion.gameObject.activeSelf) {
				_barCompletion.transform.GetChild (0).GetComponent<Image> ().fillAmount = 0;
				_barCompletion.gameObject.SetActive (false);
			}
		}
	}




    IEnumerator hide()
	{
		yield return new WaitForSeconds(0.2f);

		Time.timeScale = 0;
		AudioListener.volume = 0;

    }
}
