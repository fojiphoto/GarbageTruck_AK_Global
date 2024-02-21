using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class unlock : MonoBehaviour {
    public GameObject [] Locks;
    public GameObject[] Locks1;
	public GameObject[] Locks2;
    public GameObject[] Unlockcheck;
    public static bool Ads;
	private int loci, loci1,loci2;
        // Use this for initialization
	void Start () {

        Ads = true;
        if (PlayerPrefs.GetInt("UnlockStatus") < PlayerPrefs.GetInt("Unlock"))
        {
            PlayerPrefs.SetInt("UnlockStatus", PlayerPrefs.GetInt("Unlock"));
            //Debug.Log("UNlock" + PlayerPrefs.GetInt("UnlockStatus"));
        }
        if (PlayerPrefs.GetInt("UnlockStatus1") < PlayerPrefs.GetInt("Unlock1"))
        {
            PlayerPrefs.SetInt("UnlockStatus1", PlayerPrefs.GetInt("Unlock1"));
        }
		if (PlayerPrefs.GetInt("UnlockStatus2") < PlayerPrefs.GetInt("Unlock2"))
		{
			PlayerPrefs.SetInt("UnlockStatus2", PlayerPrefs.GetInt("Unlock2"));
		}
        for (int loci = 0; loci <= PlayerPrefs.GetInt("UnlockStatus"); loci++)
        {
           
            Locks[loci].SetActive(false);
        }
        for (int loci1 = 0; loci1 <= PlayerPrefs.GetInt("UnlockStatus1"); loci1++)
        {
       
            Locks1[loci1].SetActive(false);
        }
		for (int loci2 = 0; loci2 <= PlayerPrefs.GetInt("UnlockStatus2"); loci2++)
		{

			Locks2[loci2].SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
       if( PlayerPrefs.GetInt("unlockMode1") == 9)
        {
            if (Ads == true)
            {
                for (int i=0;i<=Unlockcheck.Length;i++)
                {
                    Unlockcheck[i].SetActive(false);
                    Ads = false;
                }
            }
        }
    }
}
