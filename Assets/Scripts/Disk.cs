using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Disk : MonoBehaviour
{
    public (float x, float y) direction;
    public float addForce;
    public float decreaseForceX;
    public float decreaseForceY;
    public float ToruqueX;
    static float addForceX;
    static float addForceY;
    static float addForceZ;

    //フリスビーに力を加えるメソッド
    public void BehaviourFrisbee(Vector3 dir)
    {
        this.GetComponent<Rigidbody>().AddForce(dir);
    }

    //フリスビーを回転させるメソッド
    public void RotateFrisbee(float x)
    {
        if(x > 0)
        {
            this.GetComponent<Rigidbody>().AddTorque(ToruqueX, 0, 0, ForceMode.Force);
        }
        else
        {
            this.GetComponent<Rigidbody>().AddTorque(-ToruqueX, 0, 0, ForceMode.Force);
        }
    }
/*
    //フリスビーを傾けるメソッド。editer上で確認したが、勝手にx軸で回転させてくれてるっぽい？
    public void DirFrisbee(float y)
    {
        transform.rotation = Quaternion.Euler(y, 0, 0);
    }
*/
    void Start()
    {
        //初期で加える力を射出方向に分解
        addForceX = addForce * Mathf.Cos(direction.y * (Mathf.PI / 180.0f)) * Mathf.Sin(direction.x * (Mathf.PI / 180.0f));
        addForceY = addForce * Mathf.Sin(direction.y * (Mathf.PI / 180.0f));
        addForceZ = addForce * Mathf.Cos(direction.y * (Mathf.PI / 180.0f)) * Mathf.Cos(direction.x * (Mathf.PI / 180.0f));
        RotateFrisbee(direction.x);
        BehaviourFrisbee(new Vector3(addForceX, addForceY, addForceZ));
    }

    void Update()
    {
        //xの向きに応じて加速度の向きを反転
        if(direction.x >=0)
        {
            BehaviourFrisbee(new Vector3(decreaseForceX, decreaseForceY, 0));
        }
        else
        {
            BehaviourFrisbee(new Vector3(-decreaseForceX, decreaseForceY, 0));
        }
    }
}

