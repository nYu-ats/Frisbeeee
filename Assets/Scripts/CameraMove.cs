using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
カメラの移動とともにゲームが進行する
ステージに応じて移動速度を変える
*/
public class CameraMove : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 cameraPosition;
    private Quaternion cameraRotation;
    private float positionZ;
    //ゲームシーンロード後、移動を始めるまでの待機時間
    [SerializeField] float stayTime;
    //カメラを移動させるか、待機させるか判断用のフラグ
    private bool proceedFlag = false;

    void Start()
    {
        StartCoroutine(CameraPositionSet(stayTime));
    }

    // Update is called once per frame
    void Update()
    {
        //ステージ1のみmoveSpeedを5に設定
        //ステージ2と3はSpeedを10にする
        if(GameController.ReturnDiskStatus())
        {
            if(GameController.stageNumber == 1)
            {
                moveSpeed = 5.0f;
            }
            else
            {
                moveSpeed = 10.0f;
            }
        }

        //ゲームポーズ中、動きを止める
        if(GameController.ReturnPauseStatus())
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }

        /*
        ゲームリスタートが発生した場合、一旦移動を止めて
        位置を再セット
        */
        if(GameController.ReturnRestartStatus())
        {
            proceedFlag = false;
            StartCoroutine(CameraPositionSet(stayTime));
            GameController.ReadyToRestart();
        }
        
        if(proceedFlag)
        {
            positionZ += moveSpeed * Time.deltaTime;
            this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, positionZ);
        }
    }


    //カメラの位置を設定
    //一定時間待機した後、カメラの移動を開始させる
    IEnumerator CameraPositionSet(float stayTime)
    {
        cameraPosition = this.transform.position;
        cameraRotation = this.transform.rotation;
        positionZ = this.transform.position.z;
        yield return new WaitForSeconds(stayTime);
        proceedFlag = true;
    }
}
