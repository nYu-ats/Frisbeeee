using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
衝突が発生した時の発光エフェクトを消去するスクリプト
*/
public class DeleteCollisionFlash : MonoBehaviour
{
    [SerializeField] float lifeTime;
    private float timeCount = 0.0f;
    void Update()
    {
        timeCount += Time.deltaTime;
        if(timeCount >= lifeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
