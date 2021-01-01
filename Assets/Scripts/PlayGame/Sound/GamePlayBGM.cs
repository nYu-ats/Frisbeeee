using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲームBGM再生用のスクリプト
public class GamePlayBGM : MonoBehaviour
{
    private Vector3 cameraPosition;
    //音量設定の最大値
    private int maxVolume = 2;
    [SerializeField] float distanceY;
    [SerializeField] float distanceZ;

    void Start()
    {
        //ゲームスタート時に音量を設定
        this.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Volume") / maxVolume;
    }

    void Update()
    {
        //カメラと一定距離をとりながら追従する
        cameraPosition = GameObject.FindWithTag("MainCamera").transform.position;
        this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y + distanceY, cameraPosition.z + distanceZ); 
    }
}
