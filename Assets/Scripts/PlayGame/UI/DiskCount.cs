using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ディスクの残弾を表示する
public class DiskCount : MonoBehaviour
{
    public Text diskCountText;
    [SerializeField] GameController gameController;

    void Update()
    {
        UpdateDiskCountUI();
    }
    public void UpdateDiskCountUI()
    {
        diskCountText.text = "残弾 : " + gameController.DiskCount;   
    }
}
