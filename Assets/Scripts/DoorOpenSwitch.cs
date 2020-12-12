using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenSwitch : MonoBehaviour
{
    [SerializeField] GameObject switchPushedSound;
    [SerializeField] GameObject doorRight;
    [SerializeField] GameObject doorLeft;
    [SerializeField] GameObject switchObj1;
    [SerializeField] GameObject switchObj2;
    [SerializeField] Material afterPushedMat;
    private float delayTIme = 0.5f;
    void Start()
    {
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
                doorRight.GetComponent<Animator>().SetBool("OpenDoor", true);
                doorLeft.GetComponent<Animator>().SetBool("OpenDoor", true);
                Instantiate(switchPushedSound, this.transform.position, Quaternion.Euler(0, 0, 0));
                this.gameObject.GetComponent<MeshRenderer>().material = afterPushedMat;
                StartCoroutine(DisableSwitch(delayTIme));
            }
        }
    }

    IEnumerator DisableSwitch(float delay)
    {
        yield return new WaitForSeconds(delay);
        switchObj1.tag = "Wall";
        switchObj2.tag = "Wall";
    }
}
