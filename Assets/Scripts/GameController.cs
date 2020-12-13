﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    public Text scoreText;
    public Image stageProgressBar;
    public Text diskCountText;
    public  Image colorBar;
    public  Image colorMark;
    [SerializeField] int colorChangeRate = 20;
    [SerializeField] float colorMarkChangeRate = 12.5f;
    public static float colorBarPosition = 0.0f;
    public static float colorMarkPosition = 0.0f;
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
    [SerializeField] Button[] menuButton;
    [SerializeField] Image[] menuImage;
    [SerializeField] Text[] menuText;
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
    [SerializeField] float speedDecrease = 1.0f;
    [SerializeField] Image gameClearBackground;
    [SerializeField] Text gameClearText;
    [SerializeField] GameObject gameClearSound;
    private bool gameClearFlag = false;
 
    // Start is called before the first frame update
    void Start()
    {
        stageNumber = loadStage;
        LoadStage();
        playerStartPosition = (int) playerCamera.transform.position.z;
        stageProgressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, 1.5f);
        foreach(Button obj in menuButton)
        {
            obj.enabled = false;
        }
        foreach(Image obj in menuImage)
        {
            obj.enabled = false;
        }
        foreach(Text obj in menuText)
        {
            obj.enabled = false;
        }

        if(PlayerPrefs.GetInt("Guide") == 1)
        {
            displayGuide = true;
        }
        else
        {
            displayGuide = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        diskCountText.text =  "Disk : " + diskCount;
        //CalcScore();
        StageProgress();
        ColorBarUpdate();
        ItemGet();
        ItemConsume();
        if(displayGuide)
        {
            DisplayGuide();
        }

        /*if(diskCount <= 0)
        {
            gameOverBackground.enabled = true;
            if(playerCamera.GetComponent<CameraMove>().moveSpeed > 0.0f)
            {
                playerCamera.GetComponent<CameraMove>().moveSpeed -= speedDecrease;
            }
            else
            {
                gameOverText.enabled = true;
                Invoke("SceneReturn", 3.0f);
            }
        }*/

        if(playerCamera.transform.position.z >= stageLength[3])
        {
            if(playerCamera.GetComponent<CameraMove>().moveSpeed > 0.0f)
            {
                playerCamera.GetComponent<CameraMove>().moveSpeed -= speedDecrease;
            }
            else
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
    void ColorBarUpdate()
    {
        colorBar.GetComponent<RectTransform>().localPosition = new Vector2(colorBarPosition * colorChangeRate, 0);
        colorMark.GetComponent<RectTransform>().localPosition = new Vector2(colorMarkPosition * colorMarkChangeRate, 0);
    }

    void ItemGet()
    {
        if(straightItem)
        {
            straightItemImage.enabled = true;
            straightItemButton.enabled = true;
            //straightItemImage.color = new Color(straightItemImage.color.r, straightItemImage.color.g, straightItemImage.color.b, 255);
        }
        else if(diskInfinityItem)
        {
            diskInfinityItemImage.enabled = true;
            diskInfinityItemButton.enabled = true;
            //diskInfinityItemImage.color = new Color(diskInfinityItemImage.color.r, diskInfinityItemImage.color.g, diskInfinityItemImage.color.b, 255);
        }
        else if(colorStopItem)
        {
            colorStopItemImage.enabled = true;
            colorStopItemButton.enabled = true;
            //colorStopItemImage.color = new Color(colorStopItemImage.color.r, colorStopItemImage.color.g, colorStopItemImage.color.b, 255);
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
            straightItem = false;
            straightItemImage.enabled = false;
            //straightItemImage.color = new Color(straightItemImage.color.r, straightItemImage.color.g, straightItemImage.color.b, 50);
            straightUsing = true;
            straightTime = itemDuaration;
        }
    }

    public void InfinityItemUse()
    {
        diskInfinityItem = false;
        diskInfinityItemImage.enabled = false;
        infinityUsingImage1.enabled = true;
        infinityUsingImage2.enabled = true;
        infinitytUsing = true;
        infinityTime = itemDuaration;
    }

    public void ColorStopItemUse()
    {
        colorStopItem = false;
        colorStopItemImage.enabled = false;
        colorStopImage1.enabled = true;
        colorStopImage2.enabled = true;
        colorStopUsing = true;
        colorStopTime = itemDuaration;
    }

    public void PauseGame()
    {
        gamePause = true;
        foreach(Button obj in menuButton)
        {
            obj.enabled = true;
        }
        foreach(Image obj in menuImage)
        {
            obj.enabled = true;
        }
        foreach(Text obj in menuText)
        {
            obj.enabled = true;
        }
    }

    public static bool ReturnPauseStatus()
    {
        return gamePause;
    }

    public void RestartGame()
    {
        gamePause = false;
        foreach(Button obj in menuButton)
        {
            obj.enabled = false;
        }
        foreach(Image obj in menuImage)
        {
            obj.enabled = false;
        }
        foreach(Text obj in menuText)
        {
            obj.enabled = false;
        }
    }

    /*
    public void CheckPlayerStage()
    {
        float isStage = GameObject.FindWithTag("MainCamera").transform.position.z;
        for(int i = 1;  i < stageLength.Length - 1; i++)
        {
            if(isStage > stageLength[i])
            {
                reachStage = i + 1;
            }
        }
    }
    */

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
        colorMarkPosition = 0.0f;
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
