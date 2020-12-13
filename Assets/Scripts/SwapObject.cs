using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
一定時間置きにポールの色を入れ替えるスクリプト
空の親オブジェクトにアタッチする
*/
public class SwapObject : MonoBehaviour
{
    //赤青白それぞれのprefabを入れておく
    [SerializeField] GameObject childObjBlue;
    [SerializeField] GameObject childObjRed;
    [SerializeField] GameObject childObjWhite;
    private GameObject startObj;
    private GameObject changedObj;
    //初期のポールの色
    [SerializeField] string startColor;
    //入れ替え後のポールの色
    [SerializeField] string changedColor;

    //入れ替えのスパン
    [SerializeField] float changeSpan;
    //子オブジェクトの数
    private int thisChildCount;
    private float time;
    //初期オブジェクト <-> 切替後オブジェクトの切替フラグ
    private bool changeFlag = true;
    //ベースとなるオブジェクトの位置
    private Transform instantiatePosition;
    //ベースとなるオブジェクトからの距離
    [SerializeField] float distance;


    void Start()
    {
        instantiatePosition = this.gameObject.transform.GetChild(0).gameObject.transform;
        //インスペクターからの指定の色に従って初期ポールをセット、生成する
        if(startColor == "Blue")
        {
            startObj = childObjBlue;
            ObjectInstantiate(this.gameObject.tag, startObj);
        }
        else if(startColor == "Red")
        {
            startObj = childObjRed;
            ObjectInstantiate(this.gameObject.tag, startObj);
        }
        else if(startColor == "White")
        {
            startObj = childObjWhite;
            ObjectInstantiate(this.gameObject.tag, startObj);
        }

        //インスペクターからの指定の色に従って切替後のポールをセット
        if(changedColor == "Blue")
        {
            changedObj = childObjBlue;
        }
        else if(changedColor == "Red")
        {
            changedObj = childObjRed;
        }
        else if(changedColor == "White")
        {
            changedObj = childObjWhite;
        }
        time = 0;
        //ポールを含めた子オブジェクトの数をセット
        thisChildCount = this.gameObject.transform.childCount;
    }


    void Update()
    {
        //子オブジェクトの数が変わってない = ポールが破壊されていない時のみ、入れ替えの処理を行う
        if(this.gameObject.transform.childCount == thisChildCount)
        {
            time += Time.deltaTime;
            if(time >= changeSpan)
            {
                //切替フラグがtrue -> 切替後オブジェクトを生成
                //切替フラグがfalse -> 初期オブジェクトを生成
                if(changeFlag == true)
                {
                    //既存オブジェクトを消去
                    Destroy(this.gameObject.transform.GetChild(1).gameObject);
                    //新しいオブジェクトを生成
                    ObjectInstantiate(this.gameObject.tag, changedObj);
                    time = 0;
                    changeFlag = false;
                }
                else if(changeFlag == false)
                {
                    Destroy(this.gameObject.transform.GetChild(1).gameObject);
                    ObjectInstantiate(this.gameObject.tag, startObj);
                    time = 0;
                    changeFlag = true;
            }
        }

        }
}

    //受け取ったオブジェクトを生成する処理
    void ObjectInstantiate(string tag, GameObject obj)
    {
        //タグで縦向きか横向きか判断
        if(this.gameObject.tag == "VerticaleMove")
            {
                //縦向きの場合は、ベースオブジェクトから縦方向の一定距離にオブジェクトを生成
                GameObject childPoll = Instantiate(obj, new Vector3(instantiatePosition.position.x, this.instantiatePosition.position.y + distance, instantiatePosition.position.z), Quaternion.Euler(0, 0, 0));
                childPoll.transform.parent = this.gameObject.transform;
            }
        else if(this.gameObject.tag == "HorizonMove")
            {
                //横向きの場合は、ベースオブジェクトから横方向の一定距離にオブジェクトを生成
                GameObject childPoll = Instantiate(obj, new Vector3(instantiatePosition.position.x + distance, instantiatePosition.position.y, instantiatePosition.position.z), Quaternion.Euler(0, 0, 90));
                childPoll.transform.parent = this.gameObject.transform;
            }
    }
}
