using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mrecscript : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnEnable()
    {
        //AbdulRehman
        //AdsManager.instance?.ShowMRec(); 
        CASAds.instance.ShowMrecBanner(CAS.AdPosition.TopLeft);
    }

    private void OnDisable()
    {
        //AbdulRehman
        //AdsManager.instance?.HideMRec();
        CASAds.instance?.HideMrecBanner();
    }
}
