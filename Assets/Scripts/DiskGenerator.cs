using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
画面へのインプットを読み込んで
ディスクを生成するスクリプト
*/

public class DiskGenerator : MonoBehaviour
{
    private Vector2 tapStartPosition;
    private Vector2 tapTempPosition;
    private Vector2 tapReleasePosition;
    [SerializeField] GameObject diskPrefab;
    private Vector3 diskGeneratePosition;
    [SerializeField] GameObject uiCanvas;
    [SerializeField] GameObject diskDecreasePopUp;
    [SerializeField] float adjustDiskGeneratePositionZ = 1.0f;
    public static bool diskGenerateFlag;//アイテム消費やゲームポーズボタンを押したときにディスクの生成をしないようにするためのフラグ 

    public GameController gameController;
    void Start()
    {
        diskGenerateFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //ディスク残団が0の場合はゲームオーバーなのでディスクは生成しない
        if(gameController.ReturnDiskStatus())
        {
            //ゲームポーズ中はディスクを生成しない
            if(!gameController.ReturnPauseStatus())
            {
                if(Input.GetMouseButtonDown(0))
                {
                    tapStartPosition = Input.mousePosition; //画面がタップされた位置をディスクの射出方向を決める起点とする
                    //画面がタップされた時にディスクが生成できる状態にする
                    //ディスク生成フラグのtrue/falseの切替タイミングの制御がしやすいため、ここでtrueをセットするようにする
                    diskGenerateFlag = true; 
                }

                if(Input.GetMouseButton(0))
                {
                    //画面フリックの長さと方向を求めるため、
                    //画面がタップ後に指が離された座標を更新する
                    tapTempPosition = Input.mousePosition; 
                }

                //ディスク生成可能状態の時のみ処理を行う
                if(Input.GetMouseButtonUp(0) & diskGenerateFlag)
                {
                    tapReleasePosition = Input.mousePosition;
                    diskGeneratePosition = GameObject.Find("Main Camera").transform.position;
                    diskGeneratePosition = new Vector3(diskGeneratePosition.x, diskGeneratePosition.y, diskGeneratePosition.z + adjustDiskGeneratePositionZ); //Player Colliderとの衝突を避けるためカメラの一定距離前方でディスクを生成
                    GameObject disk = Instantiate(diskPrefab, diskGeneratePosition, Quaternion.identity) as GameObject;
                    disk.GetComponent<Disk>().direction = ReleaseDirection(tapStartPosition, tapReleasePosition); //生成したディスクの射出角度をセットする
                    
                    //ディスク減少を止めるアイテムを使用していない場合のみ、ディスク生成後にディスクの数を減少させる
                    if(!gameController.ReturnItemUsingStatus("Infinity"))
                    {
                        GameController.diskCount -= 1;
                        GameObject popUp = Instantiate(diskDecreasePopUp);
                        popUp.transform.SetParent(uiCanvas.transform, false);
                    }
                }
            }
        }
    }

    //アイテム消費やゲームポーズボタンを押したときにディスク生成フラグをfalseにセットできるようにする
    public static void TappCancel()
    {
        diskGenerateFlag = false;
    }

    [SerializeField] float maxWidth = 500.0f; //フリックのx方向の幅の最大値
    [SerializeField] float maxHeight = 500.0f; //フリックのy方向の幅の最大値
    [SerializeField] float maxAngleX = 60.0f; //ディスク射出時のx方向の角度の最大値
    [SerializeField] float maxAngleY = 45.0f; //ディスク生成時のy方向の角度の最大値

    //フリック時の(x, y)の方向と距離からディスクの射出角度を計算する
    public (float, float) ReleaseDirection(Vector2 start, Vector2 end)
    {
        float x = 0.0f;
        float y = 0.0f;
        float moveX = 0.0f;
        float moveY = 0.0f;
        Vector2 move = end -start;
        if (Mathf.Abs(move.x) <= maxWidth)
        {
            //x方向のフリック距離がフリック幅最大値より小さければ、x方向のフリック距離としてセット
            moveX = (float)move.x;    
        }
        else if(move.x <= 0)
        {
            //x方向のフリック距離がフリック幅最大値より大きく、xの負の方向へフリックされていた場合
            moveX = -maxWidth;
        }
        else
        {
            //x方向のフリック距離がフリック幅最大値より大きく、xの正の方向へフリックされていた場合
            moveX = maxWidth;
        }

        //x方向同様に、y方向のフリック距離/方向によってx方向のフリック距離をセット
        if (Mathf.Abs(move.y) <= maxHeight)
        {
            moveY = (float)move.y;    
        }
        else if(move.y <= 0)
        {
            moveY = -maxHeight;
        }
        else
        {
            moveY = maxHeight;
        }

        //フリック距離の比に応じてディスクの射出角度を計算する
         x = maxAngleX * moveX / maxWidth;
         y = maxAngleY * moveY / maxHeight;

        return (x, y);
    }
}