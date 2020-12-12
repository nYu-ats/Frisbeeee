using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private int maxVolume = 2;
    private float time;
    [SerializeField] float destroyTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        //音量を調整して再生
        this.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Volume") / maxVolume;
        this.gameObject.GetComponent<AudioSource>().Play();
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //既定の時間経過後、自身を消去
        time += Time.deltaTime;
        if(time >= destroyTime)
        {
            Destroy(this.gameObject);
        }
    }
}
