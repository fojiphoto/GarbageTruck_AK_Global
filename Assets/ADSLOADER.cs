using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ADSLOADER : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
       
    }
    void Start()
    {
        
    }
    public void addloader()
    {
        //if (Ads_Manager.instance)
        //{
        //    Ads_Manager.instance.ShowIntersitialMediationFunction();
        //}
    }
    public void secondstep()
    {
        SceneManager.LoadScene(0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
