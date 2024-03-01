using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinBar : MonoBehaviour
{
    public TextMeshProUGUI _coins;

    public void watchvideoforrewarded()
    {  
        CASAds.instance.ShowRewarded(GETCOIN);
    }
    public void GETCOIN()
    {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 500);
        _coins.text = " " + PlayerPrefs.GetInt("Coins");
    }
}
