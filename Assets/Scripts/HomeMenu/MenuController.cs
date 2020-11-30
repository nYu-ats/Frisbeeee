using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] Button[] stageButton;
    [SerializeField] Image[] stageImage;
    [SerializeField] Text[] stageText;
    [SerializeField] Button[] volumeButton;
    [SerializeField] Image[] volumeImage;
    [SerializeField] Text[] volumeText;
    [SerializeField] Image[] volumeChecked;
    [SerializeField] Button[] guideButton;
    [SerializeField] Image[] guideImage;
    [SerializeField] Text[] guideText;
    [SerializeField] Image[] guideChecked;
    private int onFlag = 1;
    private int offFlag = 0;

    private static int startStage;

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("Stage"))
        {
            PlayerPrefs.SetInt("Stage", 1);
        }

        if(!PlayerPrefs.HasKey("Guide"))
        {
            PlayerPrefs.SetInt("Guide", 1);
        }

        if(!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetInt("Volume", 1);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(PlayerPrefs.GetInt("Volume"));
    }

    public void OnStartButtonClicked()
    {
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
    }

    public void OnReturnButtonClicked(int flag)
    {
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

            DisplayOptionSettings(offFlag);  
        }
    }

    public void OnStageButtonclicked(int stageNumber)
    {
        startStage = stageNumber;
        SceneManager.sceneLoaded += StartPositionSet;
        SceneManager.LoadScene("SampleScene");
    }

    private void StartPositionSet(Scene next, LoadSceneMode mode)
    {
        GameController gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameController>();
        gameManager.loadStage = startStage;
        startStage = 1;
        SceneManager.sceneLoaded -= StartPositionSet;
    }

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

        DisplayOptionSettings(onFlag);
    }

    public void SetVolume(int volume)
    {
        PlayerPrefs.SetInt("Volume", volume);
        foreach(Image obj in volumeChecked)
        {
            obj.enabled = false;
        }
        volumeChecked[PlayerPrefs.GetInt("Volume")].enabled = true;
    }

    public void SetGuide(int onOffFlag)
    {
        PlayerPrefs.SetInt("Guide", onOffFlag);
        foreach(Image obj in guideChecked)
        {
            obj.enabled = false;
        }
        guideChecked[PlayerPrefs.GetInt("Guide")].enabled = true;
    }

    public void DisplayOptionSettings(int flag)
    {
        if(flag == 1)
        {
            volumeChecked[PlayerPrefs.GetInt("Volume")].enabled = true;
            guideChecked[PlayerPrefs.GetInt("Guide")].enabled = true;
        }
        else if(flag == 0)
        {
            volumeChecked[PlayerPrefs.GetInt("Volume")].enabled = false;
            guideChecked[PlayerPrefs.GetInt("Guide")].enabled = false;           
        }
    }
}
