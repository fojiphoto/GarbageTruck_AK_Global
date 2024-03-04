using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timmertext : MonoBehaviour
{  [HideInInspector] public float startTime;
    public TextMeshProUGUI Timmer;

    // Update is called once per frame
    void OnEnable()
    {
        startTime = Timer.instance.totalStartTime-Timer.instance.startTime;
        string minutes = ((int)startTime / 60).ToString();
        string seconds = (startTime % 60).ToString("f0");
        Timmer.text = minutes + " : " + seconds;
    }

}
