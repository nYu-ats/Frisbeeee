using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    private int playerStartPosition;
    private float totalStageLength = 14101.0f;
    public static int stageNumber;
    [SerializeField] Image straightItemImage;
    [SerializeField] Button straightItemButton;
    [SerializeField] Image colorStopItemImage;
    [SerializeField] Button colorStopItemButton;

    [SerializeField] Image diskInfinityItemImage;
    [SerializeField] Button diskInfinityItemButton;

    public bool straightItem = false;
    public bool colorStopItem = false;
    public bool diskInfinityItem = false;
    public bool straightUsing = false;
    public bool infinitytUsing = false;
    public static bool colorStopUsing = false;
    [SerializeField] float itemDuaration = 5.0f;
    private float straightTime = 0.0f;
    private float infinityTime = 0.0f;
    private float colorStopTime = 0.0f;
    [SerializeField] Image infinityUsingImage1;
    [SerializeField] Image infinityUsingImage2;
    [SerializeField] Image colorStopImage1;
    [SerializeField] Image colorStopImage2;
    public static bool gamePause = false;
    public int loadStage;
    public static bool restartFlag;
    public ColorBar colorBar;
    public DiskCount diskCountUI;
    public GameOver gameOverUI;
    public GameClear gameClearUI;
    public bool cameraStopFlag = false;
 
    void Start()
    {
        stageNumber = loadStage;
        LoadStage();
        playerStartPosition = (int) playerCamera.transform.position.z;
        stageProgress.ResestProgressBarStatus();
    }

    void Update()
    {
        diskCountUI.UpdateDiskCountUI(diskCount);
        StageManagement();
        colorBar.UpdateColorBar(colorBarPosition);
        CheckItemGet();
        ItemConsume();
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

    private void StageManagement()
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

    public void GetItem(string item)
    {
        if(item == "Straight")
        {
            straightItem = true;
        }
        else if(item == "Infinity")
        {
            diskInfinityItem = true;
        }
        else if(item == "ColorStop")
        {
            colorStopItem = true;
        }
    }

    public bool ReturnItemStatus(string item)
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
    void CheckItemGet()
    {
        if(straightItem)
        {
            straightItemImage.enabled = true;
            straightItemButton.enabled = true;
        }
        else if(diskInfinityItem)
        {
            diskInfinityItemImage.enabled = true;
            diskInfinityItemButton.enabled = true;
        }
        else if(colorStopItem)
        {
            colorStopItemImage.enabled = true;
            colorStopItemButton.enabled = true;
        }
    }

    void ItemConsume()
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
                infinityUsingImage1.enabled = false;
                infinityUsingImage2.enabled = false;
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
                colorStopImage1.enabled = false;
                colorStopImage2.enabled = false;
            }
        }
    }

    public void StraightItemUse()
    {
        if(straightItem)
        {
            DiskGenerator.TappCancel();
            straightItem = false;
            straightItemImage.enabled = false;
            straightUsing = true;
            straightTime = itemDuaration;
        }
    }

    public void InfinityItemUse()
    {
        DiskGenerator.TappCancel();
        diskInfinityItem = false;
        diskInfinityItemImage.enabled = false;
        infinityUsingImage1.enabled = true;
        infinityUsingImage2.enabled = true;
        infinitytUsing = true;
        infinityTime = itemDuaration;
    }

    public void ColorStopItemUse()
    {
        DiskGenerator.TappCancel();
        colorStopItem = false;
        colorStopItemImage.enabled = false;
        colorStopImage1.enabled = true;
        colorStopImage2.enabled = true;
        colorStopUsing = true;
        colorStopTime = itemDuaration;
    }

    public MenuPanelButton menuPanel;
    public void PauseGame()
    {
        gamePause = true;
        DiskGenerator.TappCancel();
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
        colorBar.UpdateColorBar(colorBarPosition);
    }

    public void LoadStage()
    {
        playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, stageLength[stageNumber - 1]);
    }

    public void RestartStage()
    {
        RestartGame();
        restartFlag = true;
        InitializeGameStatus();
        LoadStage();
    }

    public static void ReadyToRestart()
    {
        restartFlag = false;
    }
}
