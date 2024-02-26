using DG.Tweening;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CheckPointHandler : MonoBehaviour
{
    Transform playercar;
    public Transform NextNode;
    public Text DistanceText;
    public float startAfter = 5f;
    bool start = false;
    float dis = 0f;
    void Start()
    {
      //  playercar = GameUIManager._instance.GetGameCar();
        StartCoroutine(StartAfter5Sec());
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            NextNode.gameObject.SetActive(true);
            //other.gameObject.GetComponent<ArrowNavigationMessageHandler>().NextTarget(NextNode);
           // GameUIManager._instance.PlayCheckPOintSound();
            transform.gameObject.SetActive(false);
           

        }
    }
    void Update()
    {
        if (start)
        {
            dis = Vector3.Distance(transform.position, playercar.position);
            DistanceText.text = "" + (int)Vector3.Distance(transform.position, playercar.position);
        }
    }

    IEnumerator StartAfter5Sec()
    {
        start = false;
        DistanceText.text = "0";
        yield return new WaitForSeconds(startAfter);
        start = true;
    }
}
