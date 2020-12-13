using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
カメラの移動とともにゲームが進行する
ステージに応じて移動速度を変える
*/
public class CameraMove : MonoBehaviour
{
    public static float moveSpeed;
    private Vector3 cameraPosition;
    private Quaternion cameraRotation;
    private float positionZ;
    //ゲームシーンロード後、移動を始めるまでの待機時間
    [SerializeField] float stayTime;
    //カメラを移動させるか、待機させるか判断用のフラグ
    private bool proceedFlag = false;
    //カメラを減速させる速度
    private float decreaseSpeed = 0.5f;

    void Start()
    {
        StartCoroutine(CameraPositionSet(stayTime));
    }

    // Update is called once per frame
    void Update()
    {
        /*
        カメラの移動速度を調整する処理
        ステージ1 -> moveSpeed : 5
        ステージ2と3はSpeed : 10
        ディスクが0(ゲームオーバー)になったら、カメラの動きを止める
        */
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
        else
        {
            if(moveSpeed > 0)
            {
                moveSpeed -= decreaseSpeed;
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
        
        //カメラを移動させる処理
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

    //外部へカメラの速度を通知する
    public static float ReturnCameraSpeed()
    {
        return moveSpeed;
    }
}
