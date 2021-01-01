using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    public GameOver gameOverUI;
    public GameClear gameClearUI;

    public int loadStage; //ホーム画面から指定のあったスタートステージを格納するための変数

    private int stageNumber;
    //滞在ステージ番号のプロパティ
    public int StageNumber
    {
        get{return this.stageNumber;}
    }

    private bool gamePause = false; //ゲームポーズ状態を管理するフラグ
    //プロパティ
    public bool GamePause
    {
        set{this.gamePause = value;}
        get{return gamePause;}
    }

    //カラーバーのステータス更新
    //Uiの更新は行わず、値の更新のみ行う
    private float colorBarPosition = 0.0f;
    
    //カラーバーのプロパティ
    public float ColorBarPosition
    {
        set{this.colorBarPosition += value;}
        get{return colorBarPosition;}
    }

    //ディスク残弾の更新
    //UIの更新は行わず、値の更新のみ行う
    private int diskCount = 30;

    //ディスク残弾のプロパティ
    public int DiskCount
    {
        set{this.diskCount += value;}
        get{return this.diskCount;}
    }
 
    void Start()
    {
        stageNumber = loadStage; //ホーム画面で指定したステージからスタートするようにセット
        SwitchActiveStage(stageNumber);
        playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, stageLength[stageNumber - 1]);
    }

    void Update()
    {
        StageProgressManagement();
        CheckGameOver();
        ItemUsingTimeControl();
    }
    

    /*
    ゲームオーバー・ゲームクリアのチェック
    */
    //ゲームクリアもしくはゲームオーバーの状態を管理するフラグ
    private bool gameStopFlag = false;
    //プロパティ
    public bool GameStopFlag
    {
        get{return this.gameStopFlag;}
    }

    private void CheckGameOver()
    {
        //ディスク残弾が0もしくはカラーバーが青or赤に振り切ったらゲームオーバー
        if(diskCount <= 0 | colorBarPosition > 10 | colorBarPosition < -10)
        {
            //カメラの進行を止めて、進行速度が0になったらゲームオーバーの表示を行う
            //規定時間表示後、ホーム画面に遷移
            gameStopFlag = true;
            if(playerCamera.GetComponent<CameraMove>().MoveSpeed <= 0.0f)
            {
                gameOverUI.DisplayGameOverMessage();
                InitializeGameStatus();
                SetReachedStage();
                Invoke("ReturnToHome", 3.0f);
            }
        }
    }

    private void GameClear()
    {
        //カメラの進行を止めて、進行速度が0になったらゲームクリアの表示を行う
        //規定時間表示後。ホーム画面に遷移
        gameStopFlag = true;
        if(playerCamera.GetComponent<CameraMove>().MoveSpeed <= 0.0f)
        {
            gameClearUI.DisplayGameClearEffect(playerCamera.transform.position);
            InitializeGameStatus();
            SetReachedStage();
            Invoke("ReturnToHome", 5.0f);
        }
    }

    //ゲームオーバーもしくはゲームクリア時のInvokeにメソッド名を文字列で渡す必要があるため
    //ホームへ戻るボタンが押された際にもこちらを使用する
    public void ReturnToHome()
    {
        SceneManager.LoadScene("Home");
    }


    /*
    ステージ進行状況を更新する処理
    */
    [SerializeField] GameObject[] eachStageObject;

    private float[] stageLength = new float[4]{0.0f, 3073.0f, 9923.0f, 14101.0f};
    //ステージ長のプロパティ
    public float[] StageLength
    {
        set{}
        get{return this.stageLength;}
    }

    //ゲーム進行に合わせて滞在ステージ番号の更新とステージオブジェクトのActivateを切り替える
    private void StageProgressManagement()
    {
        if(stageNumber == 1)
        {
            if(playerCamera.transform.position.z > stageLength[stageNumber])
            {
                stageNumber += 1;
                SwitchActiveStage(stageNumber);
            }
        }

        if(stageNumber == 2)
        {
            if(playerCamera.transform.position.z > stageLength[stageNumber])
            {
                stageNumber += 1;
                SwitchActiveStage(stageNumber);
            }
        }

        if(stageNumber == 3)
        {
            if(playerCamera.transform.position.z > stageLength[stageNumber])
            {
                //プレイヤーの位置が最終ステージの位置を超えたらゲームクリア
                GameClear();
            }
        }
    }

    private void SwitchActiveStage(int isStage)
    {
        //滞在ステージと1つ次のステージ併せて2ステージ分をActivateに、その他はDisactivate
        //滞在ステージ = 最終ステージの場合は、滞在ステージのみをActivateにする
        int isStageIndex = isStage - 1; //わかりやすくするためステージ番号 -> ステージ配列のインデックスに変換
        eachStageObject[isStageIndex].SetActive(true);
        isStageIndex += 1; //次ステージのインデックス
        if(isStageIndex / eachStageObject.Length < 1)
        {
            eachStageObject[isStageIndex].SetActive(true);
        }
        else
        {
            isStageIndex -= eachStageObject.Length;
            eachStageObject[isStageIndex].SetActive(false);
        }
        //下記処理で不要ステージをDisactivateにする
        //2ステージ分はすでに処理済み(Activate)なので、その分ループ回数を2減らす
        for(int i = 0; i < eachStageObject.Length -2; i ++)
        {
            isStageIndex += 1;
            if(isStageIndex / eachStageObject.Length < 1)
            {
                eachStageObject[isStageIndex].SetActive(false);
            }
            else
            {
                isStageIndex -= eachStageObject.Length;
                eachStageObject[isStageIndex].SetActive(false);
            }
        }
    }


    /*
    アイテムの管理に関する処理
    */
    //アイテム所持しているか否かを示すディクショナリ
    private Dictionary<string, bool> haveItem = new Dictionary<string, bool>()
    {
        {"Straight", false},
        {"Infinity", false},
        {"ColorStop", false},
    };
    //アイテム所持ステータスのアクセサ
    public void SetHaveItemStatus(string item, bool status)
    {
        haveItem[item] = status;
    }
    public bool GetHaveItemStatus(string item)
    {
        return haveItem[item];
    }

    //アイテム使用中か否かを示すディクショナリ
    private Dictionary<string, bool> itemUsing = new Dictionary<string, bool>()
    {
        {"Straight", false},
        {"Infinity", false},
        {"ColorStop", false},
    };
    //アイテム使用ステータスのアクセサ
    public void SetItemUseStatus(string item, bool statuse)
    {
        itemUsing[item] = statuse;
    }
    public bool GetItemUseStatus(string item)
    {
        return itemUsing[item];
    }

    //アイテム使用可能な時間の設定
    private Dictionary<string, float> itemEffectTime = new Dictionary<string, float>()
    {
        {"Straight", 0.0f},
        {"Infinity", 0.0f},
        {"ColorStop", 0.0f}, 
    };
    //アイテム使用時間のアクセサ
    public void SetItemUseTime(string item, float time)
    {
        itemEffectTime[item] = time;
    }

    //各アイテムの仕様状態をチェックし、使用中であれば使用可能時間を減算していく
    void ItemUsingTimeControl()
    {
        foreach(KeyValuePair<string, bool> itemStatusPair in itemUsing)
        {
            if(itemStatusPair.Value)
            {
                if(itemEffectTime[itemStatusPair.Key] > 0.0f)
                {
                    itemEffectTime[itemStatusPair.Key] -= Time.deltaTime;
                }
                else
                {
                    itemEffectTime[itemStatusPair.Key] -= Time.deltaTime;
                    itemUsing[itemStatusPair.Key] = false;  
                }
            }
        }
    }


    //ゲームの状態を操作するので、初期化処理自体はGameControllerに実装
    //"ホームへ戻る"や"ステージはじめからリスタート"ボタンの処理をする過程でこのメソッドを呼び出すようにする
    public void InitializeGameStatus()
    {
        diskCount = 30;
        //カラーバーのポジションを初期値にセット、UIをアップデートする
        colorBarPosition = 0.0f;
    }

    //"ホームへ戻る"ボタンでも使用する処理だが、ゲーム状態に関わる処理で、ゲームオーバーもしくはゲームクリア時にも必要な操作なため
    //GameController側へ実装
    public void SetReachedStage()
    {
        if(PlayerPrefs.HasKey("Stage"))
        {
            if(PlayerPrefs.GetInt("Stage") < stageNumber)
            {
                PlayerPrefs.SetInt("Stage", stageNumber);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Stage", stageNumber);
        }
    }

    public static bool restartFlag;
    public bool ReturnRestartStatus()
    {
        return restartFlag;
    }

    public static void SetRestartFlag(bool status)
    {
        restartFlag = status;
    }


}
