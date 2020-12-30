using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageProgressBar : MonoBehaviour
{
    [SerializeField] Image stageProgressBar;
    [SerializeField] Camera playerCamera;
    [SerializeField] GameController gameController;

    private int stageProgressBarLength = 1000;
    private float stageProgressBarHeight = 1.5f;
    private float[] stageLength;

    void Start()
    {
        stageLength = gameController.StageLength;
    }

    void Update()
    {
        stageProgressBar.GetComponent<RectTransform>().sizeDelta = new Vector2((playerCamera.transform.position.z - stageLength[gameController.StageNumber - 1]) / stageLength[gameController.StageNumber] * stageProgressBarLength, stageProgressBarHeight);
    }
}
