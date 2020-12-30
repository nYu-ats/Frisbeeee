using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
オブジェクトを回転させるスクリプト
中心となるオブジェクト+ポール4本を回転させる
空の親オブジェクトにアタッチする
上から、中心ブロック->top->right->bottom->leftの順で子オブジェクトを設定する
*/
public class RotationObject : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform[] childObjects;
    //中心となるブロックの初期角度調整用の値
    [SerializeField] float adjustBlockRotation;
    private float time;
    private float radius;
    // Start is called before the first frame update
    void Start()
    {
        //タグで回転軸を判断
        if(this.gameObject.tag == "RotateZ")
        {
            //Z軸回転の場合、Y方向のtopと中心ブロックの距離を回転半径とする
            radius = childObjects[1].transform.position.y - childObjects[0].transform.position.y;
        }
        else if(this.gameObject.tag == "RotateY")
        {
            //Y軸回転の場合、Z方向のtopと中心ブロックの距離を回転半径とする
            radius = (childObjects[1].transform.position.z - childObjects[0].transform.position.z) * Mathf.Sqrt(2);
        }
        
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //タグで回転軸を判断
        if(this.gameObject.tag == "RotateZ")
        {
            //中心ブロックの回転処理
            childObjects[0].rotation =  Quaternion.Euler(0.0f, 0.0f, - rotationSpeed * time);
            /*
            以下各ポールの回転処理
            ポールがが破壊された場合は、処理をスキップ
            */
            try
            {
                //topの回転処理
                childObjects[1].position = new Vector3(childObjects[0].position.x + radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[0].position.y + radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[1].position.z);
                childObjects[1].rotation = Quaternion.Euler(0.0f, 0.0f, - rotationSpeed * time);
            }
            catch{}
            try
            {
                //rightの回転処理
                childObjects[2].position = new Vector3(childObjects[0].position.x + radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[0].position.y - radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[2].position.z);
                childObjects[2].rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f - rotationSpeed * time);
            }
            catch{}
            try
            {
                //bottomの回転処理
                childObjects[3].position = new Vector3(childObjects[0].position.x - radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[0].position.y - radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[3].position.z);
                childObjects[3].rotation = Quaternion.Euler(0.0f, 0.0f, - rotationSpeed * time);
            }
            catch{}
            try
            {
                //leftの回転処理
                childObjects[4].position = new Vector3(childObjects[0].position.x - radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[0].position.y + radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[4].position.z);
                childObjects[4].rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f - rotationSpeed * time);
            }
            catch{}
            time += Time.deltaTime;
        }

        if(this.gameObject.tag == "RotateY")
        {
            childObjects[0].rotation =  Quaternion.Euler(0.0f, adjustBlockRotation + rotationSpeed * time, 0.0f);
            try
            {
                //topの回転処理
                childObjects[1].position = new Vector3(childObjects[0].position.x + radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[1].position.y, childObjects[0].position.z + radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f));
            }
            catch{}
            try
            {
                //rightの回転処理
                childObjects[2].position = new Vector3(childObjects[0].position.x + radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[2].position.y, childObjects[0].position.z - radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f));
            }
            catch{}
            try
            {
                //bottomの回転処理
                childObjects[3].position = new Vector3(childObjects[0].position.x - radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[3].position.y, childObjects[0].position.z - radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f));
            }
            catch{}
            try
            {
                //leftの回転処理
                childObjects[4].position = new Vector3(childObjects[0].position.x - radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[4].position.y, childObjects[0].position.z + radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f));
            }
            catch{}
            time += Time.deltaTime;
        }

    }
}
