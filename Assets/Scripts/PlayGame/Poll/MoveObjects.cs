using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
不空数のGameObjectをまとめて移動させるスクリプト
空の親オブジェクトから子オブジェクトのpositionを書き換える
*/
public class MoveObjects : MonoBehaviour
{
    //移動速度
    [SerializeField] int speed = 30;
    //横移動の場合の移動距離
    [SerializeField] float moveDistanceX = 20.0f;
    //縦移動の場合の移動距離
    [SerializeField] float moveDistanceY = 20.0f;
    //各オブジェクトの移動の中心となる座標を格納する配列
    private float[] centerPositionX;
    private float[] centerPositionY;
    //移動させる子オブジェクトを格納する配列
    private Transform[] childObj;
    private float time;
    //各子オブジェクトの初期位置を格納する配列
    private float[] childTransformX;
    private float[] childTransformY;
    private int haveChild;

    void Start()
    {
        //各配列の要素数を設定
        haveChild = this.gameObject.transform.childCount;
        childObj = new Transform[haveChild];
        centerPositionX = new float[haveChild];
        childTransformX = new float[haveChild];
        centerPositionY = new float[haveChild];
        childTransformY = new float[haveChild];

        //要素を格納していく
        for(int i = 0; i < haveChild; i++)
        {
            //タグで縦移動or横移動を判断
            //横移動の場合はposition.xとmoveDistaneXから移動の中心を計算
            //縦移動の場合はposition.yとmoveDistanceYから移動の中心を計算
            if(this.gameObject.tag == "HorizonMove")
            {
                childObj[i] = this.gameObject.transform.GetChild(i);
                childTransformX[i] = childObj[i].position.x;
                if(childTransformX[i] < 0)
                {
                    centerPositionX[i] = childTransformX[i] + moveDistanceX;
                }
                else
                {
                    centerPositionX[i] = childTransformX[i] - moveDistanceX;   
                }
            }
            else if(this.gameObject.tag == "VerticaleMove")
            {
                childObj[i] = this.gameObject.transform.GetChild(i);
                childTransformY[i] = childObj[i].position.y;
                if(childTransformY[i] < 0)
                {
                    centerPositionY[i] = childTransformY[i] + moveDistanceY;
                }
                else
                {
                    centerPositionY[i] = childTransformY[i] - moveDistanceY;   
                }
            }
        }
        
        time = 0.0f;
    }


    void Update()
    {
        time += Time.deltaTime;
        for(int i = 0; i < haveChild; i++)
        {
            //タグで横移動or縦移動の判断
            //HorizonMove   -> 横移動
            //VerticaleMove -> 縦移動
            if(this.gameObject.tag == "HorizonMove")
            {
                //初期位置に応じて移動のための関数を変える
                //初期座標が負 -> a-b * cosΘ
                //初期座標が正 -> a+b * cosΘ
                if(childTransformX[i] <= 0)
                {
                    //配列の要素が空の場合(オブジェクトが破壊された場合)、処理を無視する
                    try
                    {
                        childObj[i].position = new Vector3(centerPositionX[i] - moveDistanceX *(Mathf.Cos(speed * time * (Mathf.PI / 180.0f))), childObj[i].position.y, childObj[i].position.z);
                    }
                    catch
                    {
                        continue;
                    }
                }
                else if(childTransformX[i] > 0)
                {
                    try
                    {
                        childObj[i].position = new Vector3(centerPositionX[i] + moveDistanceX *(Mathf.Cos(speed * time * (Mathf.PI / 180.0f))), childObj[i].position.y, childObj[i].position.z);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            else if(this.gameObject.tag == "VerticaleMove")
            {
                //初期位置に応じて移動のための関数を変える
                //初期座標が負 -> a-b * cosΘ
                //初期座標が正 -> a+b * cosΘ
                if(childTransformY[i] <= 0)
                {
                    //配列の要素が空の場合(オブジェクトが破壊された場合)、処理を無視する
                    try
                    {
                        childObj[i].position = new Vector3(childObj[i].position.x, centerPositionY[i] - moveDistanceY *(Mathf.Cos(speed * time * (Mathf.PI / 180.0f))) , childObj[i].position.z);
                    }
                    catch
                    {
                        continue;
                    }
                }
                else if(childTransformY[i] > 0)
                {
                    try
                    {
                        childObj[i].position = new Vector3(childObj[i].position.x, centerPositionY[i] + moveDistanceY *(Mathf.Cos(speed * time * (Mathf.PI / 180.0f))), childObj[i].position.z);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }  
        }
    }
}
