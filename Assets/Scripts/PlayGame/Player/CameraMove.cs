using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//あくまでカメラの移動にかからう処理のみ
//ゲームステータスの変化による位置の移動は各ボタンにより実装する
public class CameraMove : MonoBehaviour
{
    private float moveSpeed;
    //プロパティ
    public float MoveSpeed
    {
        get{return this.moveSpeed;}
    }

    private bool restartFlag = false;
    public bool RestartFlag
    {
        set{this.restartFlag = value;}
        get{return this.restartFlag;}
    }


    private Vector3 cameraPosition;
    private Quaternion cameraRotation;
    private float positionZ;
    private bool proceedFlag = false; //ステージロードもしくは初めからリスタートした後に、実際にカメラが進行するまで数秒間隔をあけるためのフラグ
    [SerializeField] float stayTime; //ゲームシーンロード後、移動を始めるまでの待機時間

    [SerializeField] float decreaseSpeed = 0.5f; //ゲームオーバーorゲームクリア時に、カメラを減速させるための速度

    public GameController gameController;

    void Start()
    {
        StartCoroutine(CameraPositionSet(stayTime));
    }

    // Update is called once per frame
    void Update()
    {
        ChangeCameraSpeed();
        //ゲームポーズ中、動きを止める
        if(gameController.GamePause)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }

        /*
        ステージはじめからリスタートが発生した場合、一旦移動を止めて位置を再セット
        リスタートボタンが押される -> restartFlagがtrueになる -> 数秒間をあけてカメラの移動スタート
        */
        if(restartFlag)
        {
            proceedFlag = false;
            StartCoroutine(CameraPositionSet(stayTime));
            restartFlag = false;
        }
        
        //カメラを移動させる処理
        if(proceedFlag)
        {
            positionZ += moveSpeed * Time.deltaTime;
            this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, positionZ);
        }
    }

    private void ChangeCameraSpeed()
    {
        /*
        カメラの移動速度を調整する処理
        ステージ1 -> moveSpeed : 5
        ステージ2と3はSpeed : 10
        ゲームオーバーorゲームクリアになったら、カメラの動きを止める
        */
        if(!gameController.GameStopFlag)
        {
            if(gameController.StageNumber == 1)
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
