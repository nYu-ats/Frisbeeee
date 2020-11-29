using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Camera playerCamera;
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
    private float[] stageLength = new float[4]{0.0f, 3071.0f, 9921.0f, 14101.0f};
    private int stageNumber;
    private int stageProgressBarLength = 1000;
    [SerializeField] Image straightItemImage;
    [SerializeField] Image colorStopItemImage;
    [SerializeField] Image diskInfinityItemImage;
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
    public static int reachStage;
    public int loadStage;
    // Start is called before the first frame update
    void Start()
    {
        stageNumber = 1;
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
        CheckPlayerStage();
    }

    /*void CalcScore()
    {
        float score = ((int) playerCamera.transform.position.z) - playerStartPosition;
        scoreText.text = score + "m"; 
    }
    */

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
            //straightItemImage.color = new Color(straightItemImage.color.r, straightItemImage.color.g, straightItemImage.color.b, 255);
        }
        else if(diskInfinityItem)
        {
            diskInfinityItemImage.enabled = true;
            //diskInfinityItemImage.color = new Color(diskInfinityItemImage.color.r, diskInfinityItemImage.color.g, diskInfinityItemImage.color.b, 255);
        }
        else if(colorStopItem)
        {
            colorStopItemImage.enabled = true;
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

    public void SceneReturn()
    {
        if(PlayerPrefs.HasKey("Stage"))
        {
            if(PlayerPrefs.GetInt("Stage") < reachStage)
            {
                PlayerPrefs.SetInt("Stage", reachStage);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Stage", reachStage);
        }
        SceneManager.LoadScene("Home");
    }

    public void LoatStage()
    {
        Vector3 tmpPosition = GameObject.FindWithTag("MainCamera").transform.position;
        GameObject.FindWithTag("MainCamera").transform.position = new Vector3(tmpPosition.x, tmpPosition.y, stageLength[reachStage]);
    }
}
