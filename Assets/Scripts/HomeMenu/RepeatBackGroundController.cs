using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackGroundController : MonoBehaviour
{
    //ステージに配置するブロックの数
    [SerializeField] GameObject repeatBlock;

    //ブロックを並べる間隔(ブロック長)
    [SerializeField] float generateInterval;
    
    //ブロックを並べる初期位置
    private float startPosition = 0.0f;

    //何個ブロック並べるか
    private int repeatCount = 2;
    void Start()
    {
        //初期位置z=0で1ブロック長おきにブロック生成
        float createPosition = startPosition;
        for(int i = 0; i < repeatCount; i++)
        {
            Instantiate(repeatBlock, new Vector3(0.0f, 0.0f, createPosition), Quaternion.identity);
            createPosition += generateInterval;
        }
    }

    void Update()
    {
        //シーン中のブロック数が規定個数を下回ったら、新しいブロック生成
        GameObject[] currentGameObjects = GameObject.FindGameObjectsWithTag("Wall");
        if(currentGameObjects.Length < repeatCount)
        {
            Instantiate(repeatBlock, new Vector3(0.0f, 0.0f, generateInterval), Quaternion.identity);
        }
    }
}
