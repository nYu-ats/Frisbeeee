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
    - オプション設定ボタン
    */

    /*
    スタートボタン
    */
    private const int stageCount = 3;     //ステージ数は3つ
    public StageChoisePanel stageChoisePanel;
    [SerializeField] GameObject buttonPushedSound;

    //スタートボタンクリック時の処理
    public void OnStartButtonClicked()
    {
        Instantiate(buttonPushedSound);
        //ステージセレクト画面を表示する
        stageChoisePanel.SwitchStageChoisePanelDisplay(true);
        //未到達ステージがある場合は、選択できないようにする
        if(PlayerPrefs.GetInt("Stage") != stageCount)
        {
            stageChoisePanel.DisactivateUnreachStage(PlayerPrefs.GetInt("Stage"), stageCount);
        }
    }


    /*
    オプションボタン
    */
    public OptionSettingPanel optionSettingPanel;

    //オプションボタンクリック時の処理
    public void OnOptionBuutnClicked()
    {
        Instantiate(buttonPushedSound);
        optionSettingPanel.SwitchOptionSelectPanelDisplay(true);
        //オプション設定値を示すイメージを表示する
        optionSettingPanel.DisplaySelectedOptionValue(PlayerPrefs.GetInt("Volume"), PlayerPrefs.GetInt("Guide"), true);
    }


    /*
    リターンボタン
    リターンボタンクリック時の処理
    どのパネルから押されたかによって非表示化する対象を分岐させる
    */
    public void OnReturnButtonClicked(int flag)
    {
        //ステージチョイス画面から押された場合 : flag -> 1
        //オプション設定画面から押された場合 : flag -> 2
        if(flag == 1)
        {
            stageChoisePanel.SwitchStageChoisePanelDisplay(false);
        }
        else if(flag == 2)
        {
            optionSettingPanel.SwitchOptionSelectPanelDisplay(false);
            optionSettingPanel.DisplaySelectedOptionValue(PlayerPrefs.GetInt("Volume"), PlayerPrefs.GetInt("Guide"), false);
        }
    }


    /*
    ステージセレクトボタン
    */
    private static int startStage = 1;

    //ステージセレクトボタンがクリック時の処理
    public void OnStageButtonclicked(int stageNumber)
    {
        Instantiate(buttonPushedSound);
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

    /*
    オプション設定ボタン
    */
    //音量調節ボタンクリック時の処理
    public void SetVolume(int volume)
    {
        Instantiate(buttonPushedSound);
        PlayerPrefs.SetInt("Volume", volume);
        optionSettingPanel.SetSelectedVolumeImage(volume);
    }

    //ガイド表示切替ボタンクリック時の処理
    public void SetGuide(int onOffFlag)
    {
        Instantiate(buttonPushedSound);
        PlayerPrefs.SetInt("Guide", onOffFlag);
        optionSettingPanel.SetSelectedGuideSettingImage(onOffFlag);
    }
}
