using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWithTwoPolls : MonoBehaviour
{
    [SerializeField] int blockCount;
    [SerializeField] float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.transform.childCount == blockCount)
        {
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = false;
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = true;
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().AddTorque(rotationSpeed * Time.deltaTime, 0.0f, 0.0f, ForceMode.Impulse);
        }
    }
}
