using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenSwitch : MonoBehaviour
{
    private GameObject[] triangleDoors = new GameObject[4];
    void Start()
    {
        if(this.gameObject.tag == "Switch1")
        {
            triangleDoors[0] = this.gameObject.transform.parent.gameObject.transform.GetChild(0).gameObject;
            triangleDoors[1] = this.gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject;
            triangleDoors[2] = this.gameObject.transform.parent.gameObject.transform.GetChild(2).gameObject;
            triangleDoors[3] = this.gameObject.transform.parent.gameObject.transform.GetChild(3).gameObject;
        }
        else if(this.gameObject.tag == "Switch2")
        {
            triangleDoors[0] = this.gameObject.transform.parent.gameObject.transform.GetChild(0).gameObject;
            triangleDoors[1] = this.gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Disk")
        {
            if(this.gameObject.tag == "Switch1")
            {
                triangleDoors[0].GetComponent<Animator>().SetBool("OpenDoor", true);
                triangleDoors[1].GetComponent<Animator>().SetBool("OpenDoor", true);
                triangleDoors[2].GetComponent<Animator>().SetBool("OpenDoor", true);
                triangleDoors[3].GetComponent<Animator>().SetBool("OpenDoor", true);
            }
            else if(this.gameObject.tag == "Switch1")
            {
                triangleDoors[0].GetComponent<Animator>().SetBool("OpenDoor", true);
                triangleDoors[1].GetComponent<Animator>().SetBool("OpenDoor", true);     
            }
        }
    }
}
