using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatableBlock : MonoBehaviour
{
    //背景の移動速度
    [SerializeField] float speed;
    private float moveDistance;
    void Start()
    {
        //自身の長さを移動距離として設定
        moveDistance = this.gameObject.transform.GetChild(0).transform.localScale.z;
    }

    void Update()
    {
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.transform.position.z - speed * Time.deltaTime);
        
        //既定の距離(1ブロック分)進んだらシーンから消去
        if(this.gameObject.transform.position.z < -moveDistance)
        {
            Destroy(this.gameObject);
        }
    }
}
