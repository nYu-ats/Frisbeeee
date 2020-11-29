using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove1 : MonoBehaviour
{
    [SerializeField] int speed = 30;
    [SerializeField] float moveDistance = 20.0f;
    [SerializeField] float moveDistanceY = 20.0f;
    private float[] centerPositionX;
    private float[] centerPositionY;
    private Transform[] childObj;
    private float time;
    private float[] childTransformX;
    private float[] childTransformY;
    private int haveChild;
    // Start is called before the first frame update
    void Start()
    {
        haveChild = this.gameObject.transform.childCount;
        childObj = new Transform[this.gameObject.transform.childCount];
        centerPositionX = new float[this.gameObject.transform.childCount];
        childTransformX = new float[this.gameObject.transform.childCount];
        centerPositionY = new float[this.gameObject.transform.childCount];
        childTransformY = new float[this.gameObject.transform.childCount];
    
        for(int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if(this.gameObject.tag == "HorizonMove")
            {
                childObj[i] = this.gameObject.transform.GetChild(i);
                childTransformX[i] = childObj[i].position.x;
                if(childTransformX[i] < 0)
                {
                    centerPositionX[i] = childTransformX[i] + moveDistance;
                }
                else
                {
                    centerPositionX[i] = childTransformX[i] - moveDistance;   
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

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        for(int i = 0; i < haveChild; i++)
        {
            if(this.gameObject.tag == "HorizonMove")
            {
                if(childTransformX[0] <= 0)
                {
                    try
                    {
                        childObj[i].position = new Vector3(centerPositionX[i] - moveDistance *(Mathf.Cos(speed * time * (Mathf.PI / 180.0f))), childObj[i].position.y, childObj[i].position.z);
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
                        childObj[i].position = new Vector3(centerPositionX[i] + moveDistance *(Mathf.Cos(speed * time * (Mathf.PI / 180.0f))), childObj[i].position.y, childObj[i].position.z);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            else if(this.gameObject.tag == "VerticaleMove")
            {
                if(childTransformY[i] <= 0)
                {
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
