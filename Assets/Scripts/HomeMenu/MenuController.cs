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
    }

    // Update is called once per frame
    void Update()
    {
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

    public void OnReturnButtonClicked()
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
}
