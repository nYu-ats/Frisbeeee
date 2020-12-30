using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
ディスクの数、カラーバーの値が変わった時のポップアップを消去する処理
*/
public class DeletePopUp : MonoBehaviour
{
    [SerializeField] float deleteTime;
    private float time;

    void Start()
    {
        time = 0.0f;
    }


    void Update()
    {
        time += Time.deltaTime;
        if(time > deleteTime)
        {
            Destroy(this.gameObject);
        }
    }
}
