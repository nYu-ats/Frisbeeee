using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskGenerator : MonoBehaviour
{
    static Vector2 tapStartPosition;
    static Vector2 tapTempPosition;
    static Vector2 tapReleasePosition;
    [SerializeField] GameObject diskPrefab;
    private Vector3 diskGeneratePosition;
    [SerializeField] GameObject uiCanvas;
    [SerializeField] GameObject diskDecreasePopUp;
    public static bool diskGenerateFlag;//アイテム消費やゲームポーズボタンを押したときにディスクの生成をしないようにするためのフラグ 

    void Start()
    {
        diskGenerateFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //ディスク残団が0の場合はゲームオーバーなのでディスクは生成しない
        if(GameController.ReturnDiskStatus())
        {
            //ゲームポーズ中はディスクを生成しない
            if(!GameController.ReturnPauseStatus())
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
                    diskGeneratePosition = GameObject.Find("Main Camera").transform.position; //カメラの座標をディスク生成位置とする
                    GameObject disk = Instantiate(diskPrefab, diskGeneratePosition, Quaternion.identity) as GameObject;
                    disk.GetComponent<Disk>().direction = Orbit.ReleaseDirection(tapStartPosition, tapReleasePosition);
                    if(!GameController.infinitytUsing)
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
}

public static class Orbit
{
    static float maxWidth = 500.0f;
    static float maxHeight = 500.0f;
    static float maxRotX = 60.0f;
    static float maxRotY = 45.0f;
    static float x = 0.0f;
    static float y = 0.0f;

    //マウスの移動量から射出方向の回転角度へ変換
    public static (float, float) ReleaseDirection(Vector2 start, Vector2 end)
    {
        float move_x = 0.0f;
        float move_y = 0.0f;
        Vector2 move = end -start;
        if (Mathf.Abs(move.x) <= maxWidth)
        {
            move_x = (float)move.x;    
        }
        else if(move.x <= 0)
        {
            move_x = -maxWidth;
        }
        else
        {
            move_x = maxWidth;
        }

        if (Mathf.Abs(move.y) <= maxHeight)
        {
            move_y = (float)move.y;    
        }
        else if(move.y <= 0)
        {
            move_y = -maxHeight;
        }
        else
        {
            move_y = maxHeight;
        }

         x = maxRotX * move_x / maxWidth;
         y = maxRotY * move_y / maxHeight;

        return (x, y);
    }
}
