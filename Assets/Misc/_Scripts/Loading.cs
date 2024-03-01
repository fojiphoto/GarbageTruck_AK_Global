using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using admob;
public class Loading : MonoBehaviour {

	// Use this for initialization
	void OnEnable () 
	{
	
		StartCoroutine (_Loading());
	
	}

	IEnumerator _Loading()
	{
			yield return new WaitForSeconds(1.0f);
			SceneManager.LoadScene("Gameplay");
		

		//	Admob.Instance ().removeBanner ();
		//	this.gameObject.SetActive (false);
	}

}
