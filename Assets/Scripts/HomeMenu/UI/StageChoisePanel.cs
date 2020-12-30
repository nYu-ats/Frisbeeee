using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageChoisePanel : MonoBehaviour
{
    public Button[] eachStageButton;
    public Image[] eachStageImage;
    public Text[] eachStageText;
    public Button returnButton;
    public Image returnImage;
    public Image returnIcon;
    public Image panelBackground;

    void Start()
    {
        //スタート時にfalseにセット
        SwitchStageChoisePanelDisplay(false);
    }

    //スタートボタンが押された場合はtrueに
    //戻るボタンが押された場合はfalseに
    public void SwitchStageChoisePanelDisplay(bool switchStatus)
    {
        panelBackground.enabled = switchStatus;
        returnButton.enabled = switchStatus;
        returnImage.enabled = switchStatus;
        returnIcon.enabled = switchStatus;
        foreach(Button obj in eachStageButton)
        {
            obj.enabled = switchStatus;
        }
        
        foreach(Image obj in eachStageImage)
        {
            obj.enabled = switchStatus;
        }

        foreach(Text obj in eachStageText)
        {
            obj.enabled = switchStatus;
        }
    }

    //未到達のステージがある場合には、未到達部分を選択できないようにする
    public void DisactivateUnreachStage(int stageIndex, int stageCount)
    {
        for(int i = stageIndex; i < stageCount; i++ )
        {
            eachStageImage[i].color = new Color(255.0f, 255.0f, 255.0f, 0.1f);
            eachStageText[i].color = new Color(255.0f, 255.0f, 255.0f, 0.1f);
            eachStageButton[i].enabled = false;
        }
    }
}
