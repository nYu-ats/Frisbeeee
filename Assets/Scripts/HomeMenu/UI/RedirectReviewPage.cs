using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectReviewPage : MonoBehaviour
{
    private const string thisAppId = "com.Nyu.PollCut";
    private const string fixedUrlPhraseAndroid = "https://play.google.com/store/apps/details?id=";
    public void OnReviewButtonClicked()
    {
        Application.OpenURL(fixedUrlPhraseAndroid + thisAppId);
    }
}
