using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private Vector3 cameraPosition;
    //BGMの音量が大きすぎるので、小さくするための変数
    [SerializeField] float adjustVolume = 10.0f;
    //音量設定の最大値
    private int maxVolume = 2;
    [SerializeField] float distanceY = 100.0f;
    [SerializeField] float distanceZ = -100.0f;

    // Start is called before the first frame update
    void Start()
    {
        //設定値に応じて音量を調整
        this.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Volume") / maxVolume / adjustVolume;
        //カメラの位置に応じてオーディオの位置を調整
        cameraPosition = GameObject.FindWithTag("MainCamera").transform.position;
        this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y + distanceY, cameraPosition.z + distanceZ);
    }
}
