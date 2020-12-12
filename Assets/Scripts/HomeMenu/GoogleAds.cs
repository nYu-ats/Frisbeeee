using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAds : MonoBehaviour
{
    //広告表示の頻度
    [SerializeField] int adDisplayFrequency = 5;
    private static int homeCount = 0;

    void Start()
    {
        //ホーム画面の読み込み回数に応じて広告を表示
        homeCount += 1;
        if(homeCount % adDisplayFrequency == 0)
        {
            RequestInterstitial();
            homeCount = 1;
        }
    }
    private InterstitialAd interstitial;
    private void RequestInterstitial()
    {
        //インタースティシャルオブジェクトの作成
        string appId = "ca-app-pub-5395055065464098/8141889331";
        this.interstitial = new InterstitialAd(appId);
        //広告の読み込み
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
        //広告表示
        this.interstitial.Show();      
    }
}
