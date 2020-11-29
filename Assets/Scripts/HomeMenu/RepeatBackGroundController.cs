using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackGroundController : MonoBehaviour
{
    [SerializeField] GameObject repeatBlock;
    [SerializeField] float blockLength;
    private float startPosition = 0.0f;
    private int repeatCount = 3;
    // Start is called before the first frame update
    void Start()
    {
        float createPosition = startPosition;
        for(int i = 0; i < repeatCount; i++)
        {
            Instantiate(repeatBlock, new Vector3(0.0f, 0.0f, createPosition), Quaternion.identity);
            createPosition += blockLength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] currentGameObjects = GameObject.FindGameObjectsWithTag("Wall");
        if(currentGameObjects.Length < repeatCount)
        {
            Instantiate(repeatBlock, new Vector3(0.0f, 0.0f, blockLength), Quaternion.identity);
        }
    }
}
