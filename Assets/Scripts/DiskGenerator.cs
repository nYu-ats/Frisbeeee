using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskGenerator : MonoBehaviour
{
    static Vector2 pos_start;
    static Vector2 pos_tmp;
    static Vector2 pos_end;
    public GameObject diskPrefab;
    private Vector3 releasePosition;
    private float mouseDownTime = 0.0f;
    [SerializeField] float mouseDownDuaration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameController.gamePause)
        {
            if(GameController.diskCount > 0)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    pos_start = Input.mousePosition;
                }
                if(Input.GetMouseButton(0))
                {
                    mouseDownTime += Time.deltaTime;
                    pos_tmp = Input.mousePosition;
                }
                if(Input.GetMouseButtonUp(0))
                {
                    //マウスが離されたら射出方向を設定してディスク生成
                    if(mouseDownTime > mouseDownDuaration)
                    {
                        pos_end = Input.mousePosition;
                        releasePosition = GameObject.Find("Main Camera").transform.position;
                        //GameObject disk = Instantiate(diskPrefab, new Vector3(releasePosition.x, releasePosition.y + 0.5f, releasePosition.z), Quaternion.identity) as GameObject;
                        GameObject disk = Instantiate(diskPrefab, releasePosition, Quaternion.identity) as GameObject;
                        disk.GetComponent<Disk>().direction = Orbit.ReleaseDirection(pos_start, pos_end);
                        if(!GameController.infinitytUsing)
                        {
                            GameController.diskCount -= 1;
                        }
                    }
                    mouseDownTime = 0.0f;
                }
            }
        }
        /*
        //マウス入力取得
        if(Input.GetMouseButtonDown(0))
        {
            pos_start = Input.mousePosition;
        }
        if(Input.GetMouseButton(0))
        {
            pos_tmp = Input.mousePosition;
            
        }
        if(Input.GetMouseButtonUp(0))
        {
            //マウスが離されたら射出方向を設定してディスク生成
            pos_end = Input.mousePosition;
            releasePosition = GameObject.Find("Main Camera").transform.position;
            //GameObject disk = Instantiate(diskPrefab, new Vector3(releasePosition.x, releasePosition.y + 0.5f, releasePosition.z), Quaternion.identity) as GameObject;
            GameObject disk = Instantiate(diskPrefab, releasePosition, Quaternion.identity) as GameObject;
            disk.GetComponent<Disk>().direction = Orbit.ReleaseDirection(pos_start, pos_end);
        }
        */
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

        //Debug.Log((x,y));
        return (x, y);
    }
}
