using System;
using System.Collections;
using System.Collections.Generic;
using CAS;
using UnityEngine;

public class CASAds : MonoBehaviour
{
    public static CASAds instance = null;

    private static IMediationManager _manager = null;
    private static IAdView _lastAdView = null;
    private static IAdView _lastMrecAdView = null;
    private static Action _lastAction = null;

    public GameObject NoInternet;

    private void Awake()
    {
        if ( instance == null )
            instance = this;
        
        DontDestroyOnLoad( this );
    }

    private void Start()
    {
        CAS.MobileAds.settings.isExecuteEventsOnUnityThread = true;

        Init();
    }

    private void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            NoInternet.SetActive(true);
        else 
            NoInternet.SetActive(false);
    }

    private void _manager_OnInterstitialAdImpression(AdMetaData meta)
    {
        double revenue = meta.revenue;
        var impressionParameters = new[] {
            new Firebase.Analytics.Parameter("ad_platform", "CAS"),
            new Firebase.Analytics.Parameter("ad_source", meta.network.ToString()),
            new Firebase.Analytics.Parameter("ad_unit_name", meta.identifier),
            new Firebase.Analytics.Parameter("ad_format", meta.type.ToString()),
            new Firebase.Analytics.Parameter("value", revenue),
            new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression_CAS", impressionParameters);
    }


    private void Init()
    {
        _manager = MobileAds.BuildManager()
            .WithInitListener(CreateAdView)
            // Call Initialize method in any case to get IMediationManager instance
            .Initialize();
        InterstitialEvents();
        RewardedEvents();
        BannerEvents();
    }

    private void BannerEvents()
    {
        
    }
    #region Rewarded Events
    private void RewardedEvents() 
    {
        _manager.OnRewardedAdCompleted += _manager_OnRewardedAdCompleted;
        _manager.OnRewardedAdImpression += _manager_OnInterstitialAdImpression;
        _manager.OnRewardedAdFailedToLoad += _manager_OnRewardedAdFailedToLoad;
        _manager.OnRewardedAdClicked += _manager_OnRewardedAdClicked;
        _manager.OnRewardedAdLoaded += _manager_OnRewardedAdLoaded;
        _manager.OnRewardedAdShown += _manager_OnRewardedAdShown;
    }

    private void _manager_OnRewardedAdShown()
    {
        AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdDisplayed);
    }

    private void _manager_OnRewardedAdLoaded()
    {
        AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdLoaded);
    }

    private void _manager_OnRewardedAdClicked()
    {
        AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdClicked);
    }

    private void _manager_OnRewardedAdCompleted()
    {
        _lastAction.Invoke();
        AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdClosed);

    }

    private void _manager_OnRewardedAdFailedToLoad(AdError error)
    {
        AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdNoInventory);

    }
    #endregion

    #region Intestitial Events
    private void InterstitialEvents() 
    {
        _manager.OnInterstitialAdImpression += _manager_OnInterstitialAdImpression;
        _manager.OnInterstitialAdFailedToLoad += _manager_OnInterstitialAdFailedToLoad;
        _manager.OnInterstitialAdFailedToShow += _manager_OnInterstitialAdFailedToShow;
        _manager.OnInterstitialAdLoaded += _manager_OnInterstitialAdLoaded;
        _manager.OnInterstitialAdShown += _manager_OnInterstitialAdShown;
    }

    private void _manager_OnInterstitialAdShown()
    {
        AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdDisplayed);
    }

    private void _manager_OnInterstitialAdLoaded()
    {
        AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdLoaded);
    }

    private void _manager_OnInterstitialAdFailedToShow(string error)
    {
        AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdFailToShow);
    }

    private void _manager_OnInterstitialAdFailedToLoad(AdError error)
    {
        AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdFailToLoad);

    }
    #endregion

    private void CreateAdView(bool success, string error)
    {
        if (PlayerPrefs.GetInt("NoAds") < 1)
        {
            _lastAdView = _manager.GetAdView(AdSize.AdaptiveFullWidth);
            _lastMrecAdView = _manager.GetAdView(AdSize.MediumRectangle);
            _lastAdView.SetActive(false);
            _lastMrecAdView.SetActive(false);
        }

       // AdmobGA_Helper.GA_Log(AdmobGAEvents.RequestBannerAd);
    }
 
    public void ShowBanner(AdPosition position)
    {
        if (PlayerPrefs.GetInt("NoAds") < 1)
        {
            if (_lastAdView == null)
            {
                CreateAdView(true, "");
            }

            if (_lastAdView != null)
            {
                _lastAdView.position = position;
                _lastAdView.SetActive(true);
            }
       // AdmobGA_Helper.GA_Log(AdmobGAEvents.BannerAdDisplayed);
        }
    }

    public void ShowMrecBanner(AdPosition position)
    {
        if (PlayerPrefs.GetInt("NoAds") <1 )
        {
            if (_lastMrecAdView == null)
            {
                CreateAdView(true, "");
            }

            if (_lastMrecAdView != null)
            {
                _lastMrecAdView.position = position;
                _lastMrecAdView.SetActive(true);
            }
           // AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowMREC);
        }

    }

    public void HideBanner()
    {
        if ( _lastAdView != null )
        {
            _lastAdView.SetActive( false );
        }
       // AdmobGA_Helper.GA_Log(AdmobGAEvents.BannerAdRemoved);
    }

    public void HideMrecBanner()
    {
        if (_lastMrecAdView != null)
        {
            _lastMrecAdView.SetActive(false);
        }
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.HideMREC);
    }

    public void ShowInterstitial()
    {
        if (PlayerPrefs.GetInt("NoAds") < 1)
        {
            _manager?.ShowAd(AdType.Interstitial);
           // AdmobGA_Helper.LogGAEvent("CAS:Show:Interstitial");
        }
    }

    public void ShowRewarded( Action complete )
    {
        if ( _manager == null )
            return;
        
        if ( _lastAction != null)
        {
            _manager.OnRewardedAdCompleted -= _lastAction;
        }

        _lastAction = complete;
        _manager.OnRewardedAdCompleted += _lastAction;
        _manager?.ShowAd(AdType.Rewarded);

        //AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowRewardedAd);
    }
}
