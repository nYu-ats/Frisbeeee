using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBar : MonoBehaviour
{
    [SerializeField] GameController gameController;
    public Image colorBar;
    public Image colorMark;
    public const int colorChangeRate = 20;
    public const float colorMarkChangeRate = 12.5f;

    void Update()
    {
        colorBar.GetComponent<RectTransform>().localPosition = new Vector2(gameController.GetColorBarPosition() * colorChangeRate, 0);
        colorMark.GetComponent<RectTransform>().localPosition = new Vector2(gameController.GetColorBarPosition() * colorMarkChangeRate, 0); 
    }
}
