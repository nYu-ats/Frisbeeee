using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
2本のポール+ブロックで構成させれる障害物
支えとなっていた2本のポールが両方破壊されると、ブロックが重力で落下
*/
public class BlockWithTwoPolls : MonoBehaviour
{
    //オブジェクトに含まれるブロックの数
    [SerializeField] int blockCount;
    //ブロック落下時の回転速度
    [SerializeField] float rotationSpeed;

    void Update()
    {
        //子オブジェクトがブロックのみとなる = 支えがなくなった
        if(this.gameObject.transform.childCount == blockCount)
        {
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = false;
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = true;
            //ブロック落下時に少しだけ回転させる
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().AddTorque(rotationSpeed * Time.deltaTime, 0.0f, 0.0f, ForceMode.Impulse);
        }
    }
}
