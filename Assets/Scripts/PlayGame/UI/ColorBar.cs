using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBar : MonoBehaviour
{
    public  Image colorBar;
    public  Image colorMark;
    public int colorChangeRate = 20;
    public float colorMarkChangeRate = 12.5f;
    public float colorBarPosition = 0.0f;

    void Update()
    {
        colorBar.GetComponent<RectTransform>().localPosition = new Vector2(colorBarPosition * colorChangeRate, 0);
        colorMark.GetComponent<RectTransform>().localPosition = new Vector2(colorBarPosition * colorMarkChangeRate, 0); 
    }

    public void UpdateColorBarStatus(float colorChahgeValue)
    {
        colorBarPosition += colorChahgeValue;
    }
}
