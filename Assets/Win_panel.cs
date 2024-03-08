using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win_panel : MonoBehaviour
{   
    public GameObject _rccCanvas;
    public GameObject _controllerButtons;
    public GameObject _levelCompletePanel;
    public GameObject _levelFailPanel;
    private bool Iscompl;


    private void Awake()
    {
        Iscompl = true;
    }
    private void OnEnable()
    {
        {
            int _levelcompleted = PlayerPrefs.GetInt("LevelCompleted");
            _rccCanvas.SetActive(true);
            _levelCompletePanel.SetActive(true);
            _levelFailPanel.SetActive(false);
            int a = PlayerPrefs.GetInt("LevelNumber");
            //Debug.Log("a = " + PlayerPrefs.GetInt("LevelNumber"));
            //PlayerPrefs.SetInt("LevelNumber", PlayerPrefs.GetInt("LevelNumber") + 1);
            if (Iscompl)
            {
                PlayerPrefs.SetInt("LevelNumber", PlayerPrefs.GetInt("LevelNumber") + 1);
                Iscompl = false;
            }
            _controllerButtons.SetActive(false);
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
