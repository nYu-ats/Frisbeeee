using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//効果音再生用のスクリプト
public class SoundEffect : MonoBehaviour
{
    //音量の最大値
    private int maxVolume = 2;
    private float time;
    [SerializeField] float destroyTime = 2.0f;

    void Start()
    {
        //音量を設定し、効果音を再生
        this.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Volume") / maxVolume;
        this.gameObject.GetComponent<AudioSource>().Play();
        //消去タイマーセット
        time = 0.0f;
    }

    void Update()
    {
        //一定時間経過後、シーンから自身を消去
        time += Time.deltaTime;
        if(time >= destroyTime)
        {
            Destroy(this.gameObject);
        }
    }
}
