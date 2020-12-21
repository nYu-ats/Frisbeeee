using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiskCount : MonoBehaviour
{
    public Text diskCountText;

    public void UpdateDiskCountUI(int diskCount)
    {
        diskCountText.text = "残弾 : " + diskCount;   
    }
}
