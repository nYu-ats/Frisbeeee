using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    private int playerStartPosition;
    public static int stageNumber;
    public bool straightItem = false;
    public bool colorStopItem = false;
    public bool diskInfinityItem = false;
    public bool straightUsing = false;
    public bool infinitytUsing = false;
    public static bool colorStopUsing = false;
    private float straightTime = 0.0f;
    private float infinityTime = 0.0f;
    private float colorStopTime = 0.0f;
    public static bool gamePause = false;
    public int loadStage;
    public static bool restartFlag;
    public ColorBar colorBarUI;
    public DiskCount diskCountUI;
    public GameOver gameOverUI;
    public GameClear gameClearUI;
    public bool cameraStopFlag = false;
 
    void Start()
    {
        stageNumber = loadStage;
        playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, stageLength[stageNumber - 1]);
        playerStartPosition = (int) playerCamera.transform.position.z;
        stageProgress.ResestProgressBarStatus();
    }

    void Update()
    {
        StageProgressManagement();
        diskCountUI.UpdateDiskCountUI(diskCount);
        colorBarUI.UpdateColorBar(colorBarPosition);
        ItemUsingTimeControl();
        if(diskCount <= 0 | colorBarPosition > 10 | colorBarPosition < -10)
        {
            cameraStopFlag = true;
            if(CameraMove.ReturnCameraSpeed() <= 0.0f)
            {
                gameOverUI.DisplayGameOverMessage();
                Invoke("SceneReturn", 3.0f);
            }
        }

        if(playerCamera.transform.position.z >= stageLength[3])
        {
            cameraStopFlag = true;
            if(CameraMove.ReturnCameraSpeed() <= 0.0f)
            {
                gameClearUI.DisplayGameClearEffect(playerCamera.transform.position);
                Invoke("SceneReturn", 5.0f);
            }
        }
    }

    //カラーバーのステータス更新
    //赤or青のポールが破壊されたときに呼び出される
    public float colorBarPosition = 0.0f;
    public void UpdateColorBarValue(float colorBarChangeValue)
    {
        colorBarPosition += colorBarChangeValue;
    }

    //ディスク残弾の更新
    public static int diskCount = 20;
    public void UpdateDiskCount(int diskCountChangeValue)
    {
        diskCount += diskCountChangeValue;
    }

    public bool ReturnDiskStatus()
    {
        return diskCount > 0;
    }

    public bool ReturnCameraStopFlag()
    {
        return cameraStopFlag;
    }

    public bool ReturnRestartStatus()
    {
        return restartFlag;
    }
    

    public StageProgressBar stageProgress;
    public float[] stageLength = new float[4]{0.0f, 3073.0f, 9923.0f, 14101.0f};

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

    public DiskGenerator diskGenerator;

    public MenuPanelButton menuPanel;
    public void PauseGame()
    {
        gamePause = true;
        diskGenerator.TappCancel();
        menuPanel.SwitchMenuPanelDisplay(true);
    }

    public bool ReturnPauseStatus()
    {
        return gamePause;
    }

    public void RestartGame()
    {
        gamePause = false;
        menuPanel.SwitchMenuPanelDisplay(false);
    }

    public void SceneReturn()
    {
        RestartGame();
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
        InitializeGameStatus();
        SceneManager.LoadScene("Home");
    }

    private void InitializeGameStatus()
    {
        diskCount = 20;
        //カラーバーのポジションを初期値にセット、UIをアップデートする
        colorBarPosition = 0.0f;
        colorBarUI.UpdateColorBar(colorBarPosition);
    }

    public void RestartStage()
    {
        RestartGame();
        restartFlag = true;
        InitializeGameStatus();
        playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, stageLength[stageNumber - 1]);
    }

    public static void ReadyToRestart()
    {
        restartFlag = false;
    }
}
