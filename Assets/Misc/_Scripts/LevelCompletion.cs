using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

public class LevelCompletion : MonoBehaviour
{
	public static LevelCompletion instance;

	public GameObject _levelCompletePanel;
	public GameObject _levelFailPanel;
	public GameObject PickTrash;
	public GameObject playermaincanvas;
	public GameObject CollectCanvas;
	public GameObject _barCompletion;
	public GameObject _controllerButtons;
	//public static bool Ads = true;
	public GameObject TrashMan;
	public RCC_Camera camera;
	public LevelManager Levelmanager;
	[HideInInspector] public GameObject StopPos;
	[HideInInspector] public int stoppingpoints;
	public GameObject TrashPickParticle;
	public Transform DirectionalArrow, Targetofarrow;
	[HideInInspector]
	public GameObject Target1;
	[HideInInspector]
	public GameObject target;
	[HideInInspector] public GameObject trashcans;

	[HideInInspector] public int currentTargetIndex = 0;
	[HideInInspector] public int maxHealth = 100;
	[HideInInspector] public int currentHealth = 0;
	[HideInInspector] public int collectgarbage = 0;
	 public Truckprop healthbar, collect;
	 private bool Iscompl;
	public Image img;
	public GameObject timelineScene,rcccamera;


	private RCC_CarControllerV3 rccController;
	private float originalSpeed;
	[HideInInspector]public bool isCleaning = false;
	[SerializeField]public float cleaningSpeed = 5f;


	private void Awake()
    {
		if(instance == null)
        {
			instance = this;
        }
		camera = GameObject.FindObjectOfType<RCC_Camera>();
		StopPos = GameObject.FindGameObjectWithTag("drop");	
		stoppingpoints = Levelmanager.TrashPointsByLevel[PlayerPrefs.GetInt("LevelNumber")];
		Iscompl = true;
	}
	private void Start()
    {
		rccController = GetComponent<RCC_CarControllerV3>();

		if (rccController == null)
		{
			Debug.LogError("RCC_CarControllerV3 not found on the GameObject!");
		}
		originalSpeed = rccController.speed;  // Set the original speed
		currentHealth = maxHealth;
		healthbar.SetMaxHealth(maxHealth);
		Target1 = GameObject.FindGameObjectWithTag("Trash Stop" + currentTargetIndex);
		Targetofarrow = Target1.transform;	
	}
	public void TargetToFollow()
	{
		if (currentTargetIndex < 4)
		{
			//Debug.Log("currentTargetIndexATTOP" + currentTargetIndex);
			Target1 = GameObject.FindGameObjectWithTag("Trash Stop" + currentTargetIndex);
			if (Target1 != null)
			{
				Targetofarrow = Target1.transform;
				//Debug.Log("collectgarbage" + collectgarbage);
				//currentTargetIndex++;
				//Debug.Log("currentTargetIndex" + currentTargetIndex);
				
				if (currentTargetIndex == 1)
				{

					//Debug.Log("collectgarbage" + collectgarbage);
					//Debug.Log("currentTargetIndex" + currentTargetIndex);
					Target1 = GameObject.FindGameObjectWithTag("Trash Stop" + currentTargetIndex);
					Targetofarrow = Target1.transform;
				
				}
				
			}
			if (currentTargetIndex == 2)
			{
				
				Target1 = GameObject.FindGameObjectWithTag("Trash Stop" + currentTargetIndex);
				Targetofarrow = Target1.transform;
				//Debug.Log("collectgarbage" + collectgarbage);
				//currentTargetIndex++;
				//Debug.Log("currentTargetIndex" + currentTargetIndex);
			
			}			
		}
	}
	void HealthDemage(int demage)
    {
		if (currentHealth == 0) {
			_levelFailPanel.SetActive(true);
		}
		else
		{
			currentHealth -= demage;
			healthbar.SetHealth(currentHealth);
		}
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("City"))
		{
			Debug.Log("I am Colliding");
			HealthDemage(10);
        }
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
	private void OnTriggerEnter(Collider other)
	{
		if (currentTargetIndex < 3)
		{
			if (other.tag == "Trash Stop" || other.tag == ("Trash Stop" + currentTargetIndex))
			{
				StartCoroutine(TrashPickRoutine(other.gameObject));
				currentTargetIndex++;				
			}
		}
		if (other.gameObject.tag ==  "drop")
		{
			playermaincanvas.SetActive(false);
			CollectCanvas.SetActive(false);
			PickTrash.SetActive(false);
			this.GetComponent<Rigidbody>().isKinematic = true;	
			rcccamera.SetActive(false);
			timelineScene.SetActive(true);

		}
		if (other.tag == "Water Stop0")
		{
			Debug.Log("I am water truck and at Waterpoint0");
			StartCoroutine(StartCleaning());
		}

	}
	private IEnumerator StartCleaning()
	{
		// Check if the truck controller is not null
		if (rccController != null)
		{
			// Decrease the truck speed using DoTween
			DOTween.To(() => rccController.speed, x => rccController.speed = x, cleaningSpeed, 1f).SetEase(Ease.OutQuad);

			// Directly reference the child objects without using Find
			Transform brush1 = this.transform.GetChild(0); // Assuming Brush1 is the first child
			Transform brush2 = this.transform.GetChild(1); // Assuming Brush2 is the second child
			Vector3 Roatation = new Vector3(0,90,0);

			if (brush1 != null && brush2 != null)
			{
				Vector3 targetPosition = new Vector3(brush1.position.x, brush1.position.y - .5f, brush1.position.z);



				// Use DoTween for smooth transitions
				brush1.DOLocalRotate(Roatation, 1f, RotateMode.Fast).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
				brush2.DOLocalRotate(Roatation, 1f, RotateMode.Fast).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);



				// Move the brushes to the target position
				brush1.DOMoveY(targetPosition.y, 1f).SetEase(Ease.Linear);
				brush2.DOMoveY(targetPosition.y, 1f).SetEase(Ease.Linear);

				// Adjust duration and ease type as needed
				yield return new WaitForSeconds(1f);  // Adjust this time based on your animation duration


				// Cleaning process finished, you can reset brushes and restore speed if needed
				DOTween.To(() => rccController.speed, x => rccController.speed = x, originalSpeed, 1f).SetEase(Ease.OutQuad);
			}
			else
			{
				Debug.LogError("Brush1 or Brush2 not found!");
			}
		}
		else
		{
			Debug.LogError("RCC_CarControllerV3 not found!");
		}
	}
	private IEnumerator TrashPickRoutine(GameObject trashStop)
		{
		 //PlayerController.instance.SplineMove.currentPoint = 0;
			collectgarbage++;
			trashStop.gameObject.GetComponent<Collider>().enabled = false;
			this.GetComponent<Rigidbody>().isKinematic = true;
			PickTrash.SetActive(true);
			playermaincanvas.SetActive(false);
			camera.TPSMinimumFOV = 85;

			trashcans = trashStop.gameObject;
		
			yield return FadeInOut(1f, .2f);
			
			this.transform.position = new Vector3(trashStop.transform.position.x, trashStop.transform.position.y + 3.25f, trashStop.transform.position.z);
		this.transform.rotation = Quaternion.Euler(0f, 90f, 0f) * trashStop.transform.rotation;
		//this.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
		yield return FadeInOut(0f, .2f);
			TargetToFollow();

			GameObject dropObject = GameObject.FindGameObjectWithTag("drop");
        //dropObject.SetActive(true);
        //Targetofarrow = StopPos.gameObject.transform;
        if (dropObject != null)
        {
            if (currentTargetIndex == 3)
            {
                dropObject.SetActive(true);
                Targetofarrow = StopPos.gameObject.transform;
            }

        }

        if (trashStop.tag == "Reset")
			{
				this.transform.position = Levelmanager.PlayerPositions[PlayerPrefs.GetInt("LevelNumber") - 1].transform.position;
				this.transform.rotation = Levelmanager.PlayerPositions[PlayerPrefs.GetInt("LevelNumber") - 1].transform.rotation;
			}
		}
	
	public IEnumerator Complete()
    {

		trashcans.SetActive(false);

		Debug.Log("I changed the direction");
		//PlayerController.instance.SplineMove.currentPoint = 1;
		yield return new WaitForSeconds(5f);
		this.GetComponent<Rigidbody>().isKinematic = false;
		PickTrash.SetActive(false);
		playermaincanvas.SetActive(true);
		camera.TPSMinimumFOV = 60;
		TrashMan.GetComponent<Animator>().Play("Hanging");
		TrashMan.transform.GetChild(0).gameObject.SetActive(false);
		TrashMan.GetComponent<PlayerController>().DummyMonster.transform.GetChild(0).gameObject.SetActive(true);
		TrashMan.GetComponent<PlayerController>().TrashCan.SetActive(false);
		StopPos.SetActive(true); 
		collect.GarbageFill(collectgarbage);
		//PlayerController.instance.SplineMove.currentPoint = 1;

		//PlayerController.instance.SplineMove.reverse = true;
		//PlayerController.instance.SplineMove.currentPoint = 1;
		TrashPickParticle.SetActive(true);
        if ((currentTargetIndex == 3)) {
			Targetofarrow = StopPos.gameObject.transform;
			GameObject.FindGameObjectWithTag("drop").SetActive(true);
			 }
  //      else
  //      {
		//	GameObject.FindGameObjectWithTag("drop").SetActive(false);

		//}
		
		PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1000);
	}

    void OnTriggerStay(Collider col)
	{
			//this.GetComponent<Rigidbody>().drag = 5;
			//if (!_barCompletion.gameObject.activeSelf) 
			//{
			//	_barCompletion.gameObject.SetActive (true);
			//}
			//if (_barCompletion.transform.GetChild(0).GetComponent<Image>().fillAmount < 1)
			//{

			//	_barCompletion.transform.GetChild(0).GetComponent<Image>().fillAmount += 0.008f;

			//}
			//else
			//{
			//	int _levelcompleted = PlayerPrefs.GetInt("LevelCompleted");
			//	_levelCompletePanel.SetActive(true);
			//	int a = PlayerPrefs.GetInt("LevelNumber");
			//	Debug.Log("a = " + PlayerPrefs.GetInt("LevelNumber"));
   //             //PlayerPrefs.SetInt("LevelNumber", PlayerPrefs.GetInt("LevelNumber") + 1);
   //             if (Iscompl)
   //             {
			//		PlayerPrefs.SetInt("LevelNumber", PlayerPrefs.GetInt("LevelNumber") + 1);
			//		Iscompl = false;
			//	}
			//	_controllerButtons.SetActive(false);
			//	if (a == 1)
			//	{
			//		PlayerPrefs.SetInt("Unlock", 1);
			//	}
			//	if (a == 2)
			//	{
			//		PlayerPrefs.SetInt("Unlock", 2);
			//	}
			//	if (a == 3)
			//	{
			//		PlayerPrefs.SetInt("Unlock", 3);
			//	}
			//	if (a == 4)
			//	{
			//		PlayerPrefs.SetInt("Unlock", 4);
			//	}
			//	if (a == 5)
			//	{
			//		PlayerPrefs.SetInt("Unlock", 5);
			//	}
			//	if (a == 6)
			//	{
			//		PlayerPrefs.SetInt("Unlock", 6);
			//	}
			//	if (a == 7)
			//	{
			//		PlayerPrefs.SetInt("Unlock", 7);
			//	}
			//	if (a == 8)
			//	{
			//		PlayerPrefs.SetInt("Unlock", 8);
			//	}
			//	if (a == 9)
			//	{
			//		PlayerPrefs.SetInt("Unlock", 9);
			//	}
			//	if (a == 10)
			//	{
			//		PlayerPrefs.SetInt("Unlock", 10);
			//	}
			//	if (a == 11)
			//	{
			//		PlayerPrefs.SetInt("Unlock1", 1);
			//	}
			//	if (a == 12)
			//	{
			//		PlayerPrefs.SetInt("Unlock1", 2);
			//	}
			//	if (a == 13)
			//	{
			//		PlayerPrefs.SetInt("Unlock1", 3);
			//	}
			//	if (a == 14)
			//	{
			//		PlayerPrefs.SetInt("Unlock1", 4);
			//	}
			//	if (a == 15)
			//	{
			//		PlayerPrefs.SetInt("Unlock1", 5);
			//	}
			//	if (a == 16)
			//	{
			//		PlayerPrefs.SetInt("Unlock1", 6);
			//	}
			//	if (a == 17)
			//	{
			//		PlayerPrefs.SetInt("Unlock1", 7);
			//	}
			//	if (a == 18)
			//	{
			//		PlayerPrefs.SetInt("Unlock1", 8);
			//	}
			//	if (a == 19)
			//	{
			//		PlayerPrefs.SetInt("Unlock1", 9);
			//	}
			//	if (a == 20)
			//	{
			//		PlayerPrefs.SetInt("Unlock1", 10);
			//	}
			//	if (a == 21)
			//	{
			//		PlayerPrefs.SetInt("Unlock2", 1);
			//	}
			//	if (a == 22)
			//	{
			//		PlayerPrefs.SetInt("Unlock2", 2);
			//	}
			//	if (a == 23)
			//	{
			//		PlayerPrefs.SetInt("Unlock2", 3);
			//	}
			//	if (a == 24)
			//	{
			//		PlayerPrefs.SetInt("Unlock2", 4);
			//	}
			//	if (a == 25)
			//	{
			//		PlayerPrefs.SetInt("Unlock2", 5);
			//	}
			//	if (a == 26)
			//	{
			//		PlayerPrefs.SetInt("Unlock2", 6);
			//	}
			//	if (a == 27)
			//	{
			//		PlayerPrefs.SetInt("Unlock2", 7);
			//	}
			//	if (a == 28)
			//	{
			//		PlayerPrefs.SetInt("Unlock2", 8);
			//	}
			//	if (a == 29)
			//	{
			//		PlayerPrefs.SetInt("Unlock2", 9);
			//	}
			//	if (a == 30)
			//	{
			//		PlayerPrefs.SetInt("Unlock2", 10);
			//	}
			//}
			
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

		//if (obj.gameObject.tag == "drop") {
		//	if (_barCompletion.gameObject.activeSelf) {
		//		_barCompletion.transform.GetChild (0).GetComponent<Image> ().fillAmount = 0;
		//		_barCompletion.gameObject.SetActive (false);
		//	}
		//}
	}
	private IEnumerator FadeInOut(float targetAlpha, float duration)
	{
		float currentAlpha = img.color.a;
		float startTime = Time.time;

		while (Time.time < startTime + duration)
		{
			float elapsedTime = Time.time - startTime;
			float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsedTime / duration);
			img.color = new Color(0f, 0f, 0f, newAlpha);
			img.gameObject.SetActive(true);

			yield return null;
		}

		img.color = new Color(0f, 0f, 0f, targetAlpha);
	}
	IEnumerator hide()
	{
		yield return new WaitForSeconds(0.2f);

		Time.timeScale = 0;
		AudioListener.volume = 0;

    }
}
