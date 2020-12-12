using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    void Start()
    {
        //何ステージまでクリアしているか(初期段階では1)
        if(!PlayerPrefs.HasKey("Stage"))
        {
            PlayerPrefs.SetInt("Stage", 1);
        }

        //ゲーム序盤でガイドを表示させるか/非表示にするか(初期は表示)
        if(!PlayerPrefs.HasKey("Guide"))
        {
            PlayerPrefs.SetInt("Guide", 1);
        }

        //音量調整(0/1/2の3段階で初期は1)
        if(!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetInt("Volume", 1);
        }
    }


    /*
    各ボタンクリック時の処理
    - スタートボタン
    - オプションボタン
    - リターンボタン
    - ステージセレクトボタン
    - オプションセレクトボタン
    */


    //スタートボタン
    [SerializeField] Button[] stageButton;
    [SerializeField] Image[] stageImage;
    [SerializeField] Text[] stageText;
    //現状の全ステージ数は3つ
    private const int stageCount = 3;

    //スタートボタンクリック時の処理
    public void OnStartButtonClicked()
    {
        //ステージセレクト画面を表示する
        foreach(Button obj in stageButton)
        {
            obj.enabled = true;
        }
        
        foreach(Image obj in stageImage)
        {
            obj.enabled = true;
        }

        foreach(Text obj in stageText)
        {
            obj.enabled = true;
        }

        //未到達ステージについては、選択できないようにする
        for(int i = PlayerPrefs.GetInt("Stage"); i < stageCount; i++)
        {
            stageImage[i].color = new Color(255.0f, 255.0f, 255.0f, 0.1f);
            stageText[i].color = new Color(255.0f, 255.0f, 255.0f, 0.1f);
            stageButton[i].enabled = false;
        }
    }


    //オプションボタン
    [SerializeField] Button[] volumeButton;
    [SerializeField] Image[] volumeImage;
    [SerializeField] Text[] volumeText;
    [SerializeField] Button[] guideButton;
    [SerializeField] Image[] guideImage;
    [SerializeField] Text[] guideText;

    //オプションの設定値を示すためのイメージ
    [SerializeField] Image[] volumeChecked;
    [SerializeField] Image[] guideChecked;

    //オプションボタンクリック時の処理
    public void OnOptionBuutnClicked()
    {
        foreach(Button obj in volumeButton)
        {
            obj.enabled = true;
        }
        
        foreach(Image obj in volumeImage)
        {
            obj.enabled = true;
        }

        foreach(Text obj in volumeText)
        {
            obj.enabled = true;
        }

        foreach(Button obj in guideButton)
        {
            obj.enabled = true;
        }
        
        foreach(Image obj in guideImage)
        {
            obj.enabled = true;
        }

        foreach(Text obj in guideText)
        {
            obj.enabled = true;
        }
        //オプション設定値を示すイメージを表示する
        volumeChecked[PlayerPrefs.GetInt("Volume")].enabled = true;
        guideChecked[PlayerPrefs.GetInt("Guide")].enabled = true;
    }


    //リターンボタン
    //リターンボタンクリック時の処理
    //どの画面から押されたかによって非表示化する対象を分岐させる
    public void OnReturnButtonClicked(int flag)
    {
        //ステージセレクト画面から押された場合 : flag1
        //オプション設定画面から押された場合 : flag2
        if(flag == 1)
        {
            foreach(Button obj in stageButton)
            {
                obj.enabled = false;
            }
        
            foreach(Image obj in stageImage)
            {
                obj.enabled = false;
            }

            foreach(Text obj in stageText)
            {
                obj.enabled = false;
            }    
        }
        else if(flag == 2)
        {
            foreach(Button obj in volumeButton)
            {
                obj.enabled = false;
            }
        
            foreach(Image obj in volumeImage)
            {
                obj.enabled = false;
            }

            foreach(Text obj in volumeText)
            {
                obj.enabled = false;
            }

            foreach(Button obj in guideButton)
            {
                obj.enabled = false;
            }
        
            foreach(Image obj in guideImage)
            {
                obj.enabled = false;
            }

            foreach(Text obj in guideText)
            {
                obj.enabled = false;
            }

            //オプション設定値を示すイメージを非表示にする
            volumeChecked[PlayerPrefs.GetInt("Volume")].enabled = false;
            guideChecked[PlayerPrefs.GetInt("Guide")].enabled = false;             
        }
    }


    //ステージセレクトボタン
    private static int startStage = 1;

    //ステージセレクトボタンがクリック時の処理
    public void OnStageButtonclicked(int stageNumber)
    {
        //選択されたステージ番号をセット
        startStage = stageNumber;
        //シーンロード時のイベントハンドラーをセット
        SceneManager.sceneLoaded += StartPositionSet;
        SceneManager.LoadScene("SampleScene");
    }

    //ゲームシーン内の監督スクリプトに対して、ロードして欲しいステージ番号を設定する関数
    private void StartPositionSet(Scene next, LoadSceneMode mode)
    {
        GameController gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameController>();
        gameManager.loadStage = startStage;
        //ステージ番号の設定後は、イベントハンドラーから削除
        SceneManager.sceneLoaded -= StartPositionSet;
    }


    //オプションセレクトボタン
    //音量調節ボタンクリック時の処理
    public void SetVolume(int volume)
    {
        PlayerPrefs.SetInt("Volume", volume);
        //一旦設定値を示すボタンを全て非表示 -> 新たな設定値で再表示
        foreach(Image obj in volumeChecked)
        {
            obj.enabled = false;
        }
        volumeChecked[PlayerPrefs.GetInt("Volume")].enabled = true;
    }

    //ガイド表示切替ボタンクリック時の処理
    public void SetGuide(int onOffFlag)
    {
        PlayerPrefs.SetInt("Guide", onOffFlag);
        //一旦設定値を示すボタンを全て非表示 -> 新たな設定値で再表示
        foreach(Image obj in guideChecked)
        {
            obj.enabled = false;
        }
        guideChecked[PlayerPrefs.GetInt("Guide")].enabled = true;
    }
}
