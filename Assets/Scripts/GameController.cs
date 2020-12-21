using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    public Image stageProgressBar;
    public Text diskCountText;
    public static int diskCount = 20;
    private int playerStartPosition;
    private float[] stageLength = new float[4]{0.0f, 3073.0f, 9923.0f, 14101.0f};
    public static int stageNumber;
    private int stageProgressBarLength = 1000;
    [SerializeField] Image straightItemImage;
    [SerializeField] Button straightItemButton;
    [SerializeField] Image colorStopItemImage;
    [SerializeField] Button colorStopItemButton;

    [SerializeField] Image diskInfinityItemImage;
    [SerializeField] Button diskInfinityItemButton;

    public static bool straightItem = false;
    public static bool colorStopItem = false;
    public static bool diskInfinityItem = false;
    public static bool straightUsing = false;
    public static bool infinitytUsing = false;
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
    //public static int reachStage;
    public int loadStage;
    public static bool restartFlag;
    private bool displayGuide;
    [SerializeField] float displayTime = 5.0f;
    [SerializeField] Image guideFlickImage;
    [SerializeField] Text guideFlickText;
    [SerializeField] Image guideWhitePollImage;
    [SerializeField] Text guideWhitePollText;
    [SerializeField] Image guideColorPollImage;
    [SerializeField] Text guideColorPollText;
    [SerializeField] Image guideObstacleImage;
    [SerializeField] Text guideObstacleText;
    [SerializeField] Image guideGameOverImage;
    [SerializeField] Text guideGameOverText;
    [SerializeField] Image guideSwitchImage;
    [SerializeField] Text guideSwitchText;
    [SerializeField] GameObject guideSwitchFocus;
    private GameObject focusObject;
    [SerializeField] Image gameOverBackground;
    [SerializeField] Text gameOverText;
    [SerializeField] Image gameClearBackground;
    [SerializeField] Text gameClearText;
    [SerializeField] GameObject gameClearSound;
    private bool gameClearFlag = false;
    public ColorBar colorBar;
 
    void Start()
    {
        stageNumber = loadStage;
        LoadStage();
        playerStartPosition = (int) playerCamera.transform.position.z;
        stageProgressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, 1.5f);

        if(PlayerPrefs.GetInt("Guide") == 1)
        {
            displayGuide = true;
        }
        else
        {
            displayGuide = false;
        }
    }

    void Update()
    {
        diskCountText.text =  "Disk : " + diskCount;
        StageProgress();
        colorBar.UpdateColorBar(colorBarPosition);
        CheckItemGet();
        ItemConsume();
        if(displayGuide)
        {
            DisplayGuide();
        }

        if(diskCount <= 0)
        {
            gameOverBackground.enabled = true;
            if(CameraMove.ReturnCameraSpeed() <= 0.0f)
            {
                gameOverText.enabled = true;
                Invoke("SceneReturn", 3.0f);
            }
        }

        if(playerCamera.transform.position.z >= stageLength[3])
        {
            if(CameraMove.ReturnCameraSpeed() <= 0.0f)
            {
                gameClearText.enabled = true;
                gameClearBackground.enabled = true;
                if(!gameClearFlag)
                {
                    Instantiate(gameClearSound, playerCamera.transform.position, Quaternion.Euler(0, 0, 0));
                    gameClearFlag = true;
                }
                Invoke("SceneReturn", 5.0f);
            }
        }
    }

    public float colorBarPosition = 0.0f;
    public void UpdateColorBarValue(float colorBarChangeValue)
    {
        colorBarPosition += colorBarChangeValue;
    }

    public static bool ReturnDiskStatus()
    {
        return diskCount > 0;
    }

    public static bool ReturnRestartStatus()
    {
        return restartFlag;
    }
    void StageProgress()
    {
        if(stageNumber == 1)
        {
            if((playerCamera.transform.position.z - stageLength[stageNumber - 1]) <= stageLength[stageNumber])
            {

                stageProgressBar.GetComponent<RectTransform>().sizeDelta = new Vector2((playerCamera.transform.position.z - stageLength[stageNumber - 1]) / stageLength[stageNumber] * stageProgressBarLength, 1.5f);
            }
            else
            {
                stageProgressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, 1.5f);
                stageNumber += 1;
            }
        }
        if(stageNumber == 2)
        {
            if((playerCamera.transform.position.z - stageLength[stageNumber - 1]) <= stageLength[stageNumber])
            {
                stageProgressBar.GetComponent<RectTransform>().sizeDelta = new Vector2((playerCamera.transform.position.z - stageLength[stageNumber - 1]) / stageLength[stageNumber] * stageProgressBarLength, 1.5f);
            }
            else
            {
                stageProgressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, 1.5f);
                stageNumber += 1;
            }
        }
    }

    public static void GetItem(string item)
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

    public static bool ReturnItemStatus(string item)
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

    public MenuPanel menuPanel;
    public void PauseGame()
    {
        gamePause = true;
        DiskGenerator.TappCancel();
        menuPanel.SwitchMenuPanelDisplay(true);
    }

    public static bool ReturnPauseStatus()
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

    private void DisplayGuide()
    {
        if(Mathf.Floor(playerCamera.transform.position.z) == 100)
        {
            guideFlickImage.enabled = true;
            guideFlickText.enabled = true;
            StartCoroutine(DisactivateGuide(guideFlickImage, guideFlickText, displayTime));
        }

        if(Mathf.Floor(playerCamera.transform.position.z) == 200)
        {
            guideWhitePollImage.enabled = true;
            guideWhitePollText.enabled = true;
            StartCoroutine(DisactivateGuide(guideWhitePollImage, guideWhitePollText, displayTime));
        }

        if(Mathf.Floor(playerCamera.transform.position.z) == 320)
        {
            guideColorPollImage.enabled = true;
            guideColorPollText.enabled = true;
            StartCoroutine(DisactivateGuide(guideColorPollImage, guideColorPollText, displayTime));
        }

        if(Mathf.Floor(playerCamera.transform.position.z) == 520)
        {
            guideObstacleImage.enabled = true;
            guideObstacleText.enabled = true;
            StartCoroutine(DisactivateGuide(guideObstacleImage, guideObstacleText, displayTime));
        }

        if(Mathf.Floor(playerCamera.transform.position.z) == 670)
        {
            guideGameOverImage.enabled = true;
            guideGameOverText.enabled = true;
            StartCoroutine(DisactivateGuide(guideGameOverImage, guideGameOverText, displayTime));
        }

        if(Mathf.Floor(playerCamera.transform.position.z) == 3020)
        {
            guideSwitchImage.enabled = true;
            guideSwitchText.enabled = true;
            StartCoroutine(DisactivateGuide(guideSwitchImage, guideSwitchText, displayTime)); 
        }

        if(Mathf.Floor(playerCamera.transform.position.z) == 3020)
        {
            GameObject check = GameObject.FindWithTag("SwitchFocus");
            if(check == null)
            {
                focusObject = Instantiate(guideSwitchFocus);
                StartCoroutine(DisactivateFocusSwitch(focusObject, displayTime));
            }
        }
    }

    IEnumerator DisactivateGuide(Image image, Text text, float delay)
    {
        yield return new WaitForSeconds(delay);
        image.enabled = false;
        text.enabled = false;
    }

    IEnumerator DisactivateFocusSwitch(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(focusObject);
    }
}
