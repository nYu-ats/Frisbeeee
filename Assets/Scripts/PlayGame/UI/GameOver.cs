using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] Image gameOverBackground;
    [SerializeField] Text gameOverText;

    public void DisplayGameOverMessage()
    {
        gameOverBackground.enabled = true;
        gameOverText.enabled = true;   
    }
}
