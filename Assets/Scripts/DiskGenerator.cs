using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskGenerator : MonoBehaviour
{
    static Vector2 pos_start;
    static Vector2 pos_tmp;
    static Vector2 pos_end;
    public GameObject diskPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            pos_end = Input.mousePosition;
            GameObject disk = Instantiate(diskPrefab) as GameObject;
            disk.GetComponent<Disk>().direction = Orbit.ReleaseDirection(pos_start, pos_end);
        }


    }
}

public static class Orbit
{
    static float maxWidth = 100.0f;
    static float maxHeight = 100.0f;
    static float maxRot = 60.0f;
    static float x = 0.0f;
    static float y = 0.0f;

    //マウスの移動量から射出方向の回転角度へ変換
    public static (float, float) ReleaseDirection(Vector2 start, Vector2 end)
    {
        Vector2 move = end -start;
        float move_x = (float)move.x;
        float move_y = (float)move.y;
        if((-maxWidth <= move_x) && (move_x <= maxWidth))
        {
            x = maxRot * move_x / maxWidth;
        } 
        if((-maxHeight <= move_y) && (move_y <= maxHeight))
        {
            y = maxRot * move_y / maxHeight;
        }
        return (x, y);
    }
}
