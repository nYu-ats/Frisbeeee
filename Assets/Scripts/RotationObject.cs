using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotationObject : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform[] childObjects;
    [SerializeField] float adjustBlockRotation;
    private float time;
    private float radius;
    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.tag == "RotateZ")
        {
            radius = childObjects[1].transform.position.y - childObjects[0].transform.position.y;
        }
        else
        {
            radius = (childObjects[1].transform.position.z - childObjects[0].transform.position.z) * Mathf.Sqrt(2);
        }
        
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.tag == "RotateZ")
        {
            childObjects[0].rotation =  Quaternion.Euler(0.0f, 0.0f, - rotationSpeed * time);
            try
            {
                childObjects[1].position = new Vector3(childObjects[0].position.x + radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[0].position.y + radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[1].position.z);
                childObjects[1].rotation = Quaternion.Euler(0.0f, 0.0f, - rotationSpeed * time);
            }
            catch{}
            try
            {
                childObjects[2].position = new Vector3(childObjects[0].position.x + radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[0].position.y - radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[2].position.z);
                childObjects[2].rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f - rotationSpeed * time);
            }
            catch{}
            try
            {
                childObjects[3].position = new Vector3(childObjects[0].position.x - radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[0].position.y - radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[3].position.z);
                childObjects[3].rotation = Quaternion.Euler(0.0f, 0.0f, - rotationSpeed * time);
            }
            catch{}
            try
            {
                childObjects[4].position = new Vector3(childObjects[0].position.x - radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[0].position.y + radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[4].position.z);
                childObjects[4].rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f - rotationSpeed * time);
            }
            catch{}
            time += Time.deltaTime;
        }

        if(this.gameObject.tag == "RotateY")
        {
            childObjects[0].rotation =  Quaternion.Euler(0.0f, adjustBlockRotation + rotationSpeed * time, 0.0f);
            try
            {
                childObjects[1].position = new Vector3(childObjects[0].position.x + radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[1].position.y, childObjects[0].position.z + radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f));
            }
            catch{}
            try
            {
                childObjects[2].position = new Vector3(childObjects[0].position.x + radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[2].position.y, childObjects[0].position.z - radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f));
            }
            catch{}
            try
            {
                childObjects[3].position = new Vector3(childObjects[0].position.x - radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[3].position.y, childObjects[0].position.z - radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f));
            }
            catch{}
            try
            {
                childObjects[4].position = new Vector3(childObjects[0].position.x - radius * Mathf.Cos(rotationSpeed * time * Mathf.PI / 180.0f), childObjects[4].position.y, childObjects[0].position.z + radius * Mathf.Sin(rotationSpeed * time * Mathf.PI / 180.0f));
            }
            catch{}
            time += Time.deltaTime;
        }

    }
}
