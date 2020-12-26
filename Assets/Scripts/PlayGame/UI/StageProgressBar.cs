using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageProgressBar : MonoBehaviour
{
    [SerializeField] Image stageProgressBar;
    private int stageProgressBarLength = 1000;
    private float stageProgressBarHeight = 1.5f;
    public GameController gameController;
    private float[] stageLength;

    void Start()
    {
        stageLength = gameController.ReturnStageConstitution();
    }

    public void UpdateStageProgressBar(float playerPosition, int stageNumber)
    {
        stageProgressBar.GetComponent<RectTransform>().sizeDelta = new Vector2((playerPosition - stageLength[stageNumber - 1]) / stageLength[stageNumber] * stageProgressBarLength, stageProgressBarHeight);
    }

    public void ResestProgressBarStatus()
    {
        stageProgressBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, stageProgressBarHeight);     
    }
}
