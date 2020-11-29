using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.tag == "RotateY")
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(0.0f, rotateSpeed * time, 0.0f);
            time += Time.deltaTime;    
        }
        else
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, rotateSpeed * time);
            time += Time.deltaTime;
        }
    }
}
