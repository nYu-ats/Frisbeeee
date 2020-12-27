using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    public static int stageNumber;
    public static bool gamePause = false;
    public int loadStage; //ホーム画面からスタートステージをセットするための変数
    public ColorBar colorBarUI;
    public GameOver gameOverUI;
    public GameClear gameClearUI;
    public static bool cameraStopFlag = false;
 
    void Start()
    {
        stageNumber = loadStage; //ホーム画面で指定したステージからスタートするようにセット
        playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, stageLength[stageNumber - 1]);
        stageProgress.ResestProgressBarStatus();
    }

    void Update()
    {
        StageProgressManagement();
        CheckGameOver();
        CheckGameClear();
        ItemUsingTimeControl();
    }

    /*
    ゲームオーバー・ゲームクリアのチェック
    */
    private void CheckGameOver()
    {
        //ディスク残弾が0もしくはカラーバーが青or赤に振り切ったらゲームオーバー
        if(diskCount <= 0 | colorBarPosition > 10 | colorBarPosition < -10)
        {
            //カメラの進行を止めて、進行速度が0になったらゲームオーバーの表示を行う
            //規定時間表示後、ホーム画面に遷移
            cameraStopFlag = true;
            if(CameraMove.ReturnCameraSpeed() <= 0.0f)
            {
                gameOverUI.DisplayGameOverMessage();
                InitializeGameStatus();
                SetReachedStage();
                Invoke("ReturnToHome", 3.0f);
            }
        }
    }

    private void CheckGameClear()
    {
        //プレイヤーの位置が最終ステージの位置を超えたらゲームクリア
        if(playerCamera.transform.position.z >= stageLength[3])
        {
            //カメラの進行を止めて、進行速度が0になったらゲームクリアの表示を行う
            //規定時間表示後。ホーム画面に遷移
            cameraStopFlag = true;
            if(CameraMove.ReturnCameraSpeed() <= 0.0f)
            {
                gameClearUI.DisplayGameClearEffect(playerCamera.transform.position);
                InitializeGameStatus();
                SetReachedStage();
                Invoke("ReturnToHome", 5.0f);
            }
        }
    }

    //ゲームオーバーもしくはゲームクリア時のInvokeにメソッド名を文字列で渡す必要があるため
    public void ReturnToHome()
    {
        SceneManager.LoadScene("Home");
    }

    //カラーバーのステータス更新
    public static float colorBarPosition = 0.0f;
    public void UpdateColorBarValue(float colorBarChangeValue)
    {
        colorBarPosition += colorBarChangeValue;
    }

    public float GetColorBarPosition()
    {
        return colorBarPosition;
    }

    //ディスク残弾の更新
    public static int diskCount = 20;
    public void UpdateDiskCount(int diskCountChangeValue)
    {
        diskCount += diskCountChangeValue;
    }
    public int ReturnDiskCount()
    {
        return diskCount;
    }

    public bool ReturnCameraStopFlag()
    {
        return cameraStopFlag;
    }

    

    public StageProgressBar stageProgress;
    public static float[] stageLength = new float[4]{0.0f, 3073.0f, 9923.0f, 14101.0f};

    public float[] ReturnStageConstitution()
    {
        return stageLength;
    }

    private void StageProgressManagement()
    {
        if(stageNumber == 1)
        {
            if(playerCamera.transform.position.z <= stageLength[stageNumber])
            {
                stageProgress.UpdateStageProgressBar(playerCamera.transform.position.z, stageNumber);
            }
            else
            {
                stageProgress.ResestProgressBarStatus();
                stageNumber += 1;
            }
        }

        if(stageNumber == 2)
        {
            if(playerCamera.transform.position.z <= stageLength[stageNumber])
            {
                stageProgress.UpdateStageProgressBar(playerCamera.transform.position.z, stageNumber);
            }
            else
            {
                stageProgress.ResestProgressBarStatus();
                stageNumber += 1;
            }
        }

        if(stageNumber == 3)
        {
            if(playerCamera.transform.position.z <= stageLength[stageNumber])
            {
                stageProgress.UpdateStageProgressBar(playerCamera.transform.position.z, stageNumber);
            }
        }
    }


    public bool straightItem = false;
    public bool colorStopItem = false;
    public bool diskInfinityItem = false;
    public bool straightUsing = false;
    public bool infinitytUsing = false;
    public static bool colorStopUsing = false;
    private static float straightTime = 0.0f;
    private static float infinityTime = 0.0f;
    private static float colorStopTime = 0.0f;

    public void SetHaveItemStatus(string item, bool status)
    {
        if(item == "Straight")
        {
            straightItem = status;
        }
        else if(item == "Infinity")
        {
            diskInfinityItem = status;
        }
        else if(item == "ColorStop")
        {
            colorStopItem = status;
        }
    }

    public bool GetHaveItemStatus(string item)
    {
        if(item == "Straight")
        {
            return straightItem;
        }
        else if(item == "Infinity")
        {
            return diskInfinityItem;
        }
        else if(item == "ColorStop")
        {
            return colorStopItem;
        }
        else
        {
            return false;
        }
    }

    public void SetItemUseStatus(string item, bool status, float itemUseTime)
    {
        if(item == "Straight")
        {
            straightUsing = status;
            straightTime = itemUseTime;
        }
        else if(item == "Infinity")
        {
            infinitytUsing = status;
            infinityTime = itemUseTime;
        }
        else if(item == "ColorStop")
        {
            colorStopUsing = status;
            colorStopTime = itemUseTime;
        }
    }


    public bool ReturnItemUsingStatus(string item)
    {
        if(item == "Straight")
        {
            return straightUsing;
        }
        else if(item == "Infinity")
        {
            return infinitytUsing;
        }
        else if(item == "ColorStop")
        {
            return colorStopUsing;
        }
        else
        {
            return false;
        }

    }

    void ItemUsingTimeControl()
    {
        if(straightUsing)
        {
            if(straightTime > 0.0f)
            {
                straightTime -= Time.deltaTime;
            }
            else
            {
                straightTime = 0.0f;
                straightUsing = false;
            }
        }

        if(infinitytUsing)
        {
            if(infinityTime > 0.0f)
            {
                infinityTime -= Time.deltaTime;
            }
            else
            {
                infinityTime = 0.0f;
                infinitytUsing = false;
            }
        }

        if(colorStopUsing)
        {
            if(colorStopTime > 0.0f)
            {
                colorStopTime -= Time.deltaTime;
            }
            else
            {
                colorStopTime = 0.0f;
                colorStopUsing = false;
            }
        }
    }


    public bool ReturnPauseStatus()
    {
        return gamePause;
    }

    public void SetGamePauseStatus(bool status)
    {
        gamePause = status;
    }



    //ゲームの状態を操作するので、初期化処理自体はGameControllerに実装
    //"ホームへ戻る"や"ステージはじめからリスタート"ボタンの処理をする過程でこのメソッドを呼び出すようにする
    public void InitializeGameStatus()
    {
        diskCount = 20;
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


    public float GetStageStartPosition()
    {
        return stageLength[stageNumber - 1];
    }

    public int GetStageNumber()
    {
        return stageNumber;
    }

    public static bool restartFlag; //ステージはじめからリスタート時に、カメラの移動
    public bool ReturnRestartStatus()
    {
        return restartFlag;
    }

    public static void SetRestartFlag(bool status)
    {
        restartFlag = status;
    }
}
