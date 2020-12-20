using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSettingPanel : MonoBehaviour
{
    public Button[] volumeButton;
    public Image[] volumeImage;
    public Text[] volumeText;
    public Button[] guideSettingButton;
    public Image[] guideSettingImage;
    public Text[] guideSettingText;
    public Image[] volumeSelectedImage;
    public Image[] guideSettingSelectedImage;
    public Button returnButton;
    public Image returnImage;
    public Image returnIcon;
    public Image panelBackground;

    void Start()
    {
        //スタート時にfalseにセット
        SwitchOptionSelectPanelDisplay(false);  
        foreach(Image obj in volumeSelectedImage)
        {
            obj.enabled = false;
        }
        foreach(Image obj in guideSettingSelectedImage)
        {
            obj.enabled = false;
        } 
    }

    //オプションボタンが押された場合はtrueに
    //戻るボタンが押された場合はfalseに
    public void SwitchOptionSelectPanelDisplay(bool switchStatus)
    {
        panelBackground.enabled = switchStatus;
        returnButton.enabled = switchStatus;
        returnImage.enabled = switchStatus;
        returnIcon.enabled = switchStatus;
        foreach(Button obj in volumeButton)
        {
            obj.enabled = switchStatus;
        }
        
        foreach(Image obj in volumeImage)
        {
            obj.enabled = switchStatus;
        }

        foreach(Text obj in volumeText)
        {
            obj.enabled = switchStatus;
        }

        foreach(Button obj in guideSettingButton)
        {
            obj.enabled = switchStatus;
        }
        
        foreach(Image obj in guideSettingImage)
        {
            obj.enabled = switchStatus;
        }

        foreach(Text obj in guideSettingText)
        {
            obj.enabled = switchStatus;
        }
    }

    //設定されているオプション値の表示切替
    //全オプション地に対してあらかじめ表示用の画像を設定しており
    //実際の設定値を受け取って、その画像だけを表示させる
    //デフォルトで表示される背景画像等とは引数が異なるため、この表示処理のみ分離
    public void DisplaySelectedOptionValue(int volumeValue, int guideValue, bool switchstatus)
    {
        volumeSelectedImage[volumeValue].enabled = switchstatus;
        guideSettingSelectedImage[guideValue].enabled = switchstatus;
    }

    //設定値が変更された場合の表示の切替処理
    public void SetSelectedVolumeImage(int volumeValue)
    {
        //一旦設定値を示すボタンを全て非表示 -> 新たな設定値で再表示
        foreach(Image obj in volumeSelectedImage)
        {
            obj.enabled = false;
        }
        volumeSelectedImage[volumeValue].enabled = true;
    }

    public void SetSelectedGuideSettingImage(int guideSettingValue)
    {
        //一旦設定値を示すボタンを全て非表示 -> 新たな設定値で再表示
        foreach(Image obj in guideSettingSelectedImage)
        {
            obj.enabled = false;
        }
        guideSettingSelectedImage[guideSettingValue].enabled = true;  
    }

}
