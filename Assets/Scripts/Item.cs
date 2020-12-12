using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter()
    {
        if(this.transform.parent.gameObject.tag == "Straight" & !GameController.straightItem)
        {
            GameController.straightItem = true;
            Destroy(this.transform.parent.gameObject);
        }
        else if(this.transform.parent.gameObject.tag == "Infinity" & !GameController.diskInfinityItem)
        {
            Debug.Log("ok");
            GameController.diskInfinityItem = true;
            Destroy(this.transform.parent.gameObject);
        }
        else if(this.transform.parent.gameObject.tag == "ColorStop" & !GameController.colorStopItem)
        {
            GameController.colorStopItem = true;
            Destroy(this.transform.parent.gameObject);
        }
    }

}
