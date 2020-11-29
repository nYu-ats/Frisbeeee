using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChange : MonoBehaviour
{
    [SerializeField] GameObject childObjBlue;
    [SerializeField] GameObject childObjRed;
    [SerializeField] GameObject childObjWhite;
    [SerializeField] float changeSpan;
    [SerializeField] string startColor;
    [SerializeField] string changedColor;
    [SerializeField] float distance;
    private int thisChildCount;
    private float time;
    private bool changeFlag = true;
    private GameObject startObj;
    private GameObject changedObj;
    private Transform instantiatePosition;

    // Start is called before the first frame update
    void Start()
    {
        instantiatePosition = this.gameObject.transform.GetChild(0).gameObject.transform;
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
        thisChildCount = this.gameObject.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.transform.childCount == thisChildCount)
        {
            time += Time.deltaTime;
            if(time >= changeSpan)
            {
                if(changeFlag == true)
                {
                    Destroy(this.gameObject.transform.GetChild(1).gameObject);
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

    void ObjectInstantiate(string tag, GameObject obj)
    {
        if(this.gameObject.tag == "VerticaleMove")
            {
                GameObject childPoll = Instantiate(obj, new Vector3(instantiatePosition.position.x, this.instantiatePosition.position.y + distance, instantiatePosition.position.z), Quaternion.Euler(0, 0, 0));
                childPoll.transform.parent = this.gameObject.transform;
            }
        else if(this.gameObject.tag == "HorizonMove")
            {
                GameObject childPoll = Instantiate(obj, new Vector3(instantiatePosition.position.x + distance, instantiatePosition.position.y, instantiatePosition.position.z), Quaternion.Euler(0, 0, 90));
                childPoll.transform.parent = this.gameObject.transform;
            }
    }
}
