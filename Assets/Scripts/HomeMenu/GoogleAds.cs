using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAds : MonoBehaviour
{
    private InterstitialAd interstitial;

    // Start is called before the first frame update
    void Start()
    {
        RequestInterstitial();
        if(!PlayerPrefs.HasKey("CountForAdd"))
        {
            PlayerPrefs.SetInt("CountForAdd", 0);
        }
        PlayerPrefs.SetInt("CountForAdd", PlayerPrefs.GetInt("CountForAdd") + 1);
        Debug.Log(PlayerPrefs.GetInt("CountForAdd"));
        if(PlayerPrefs.GetInt("CountForAdd") % 5 == 0)
        {
            this.interstitial.Show();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RequestInterstitial()
    {
        string appId = "ca-app-pub-5395055065464098/8141889331";
        this.interstitial = new InterstitialAd(appId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);        
    }
}
