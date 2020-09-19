using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Disk : MonoBehaviour
{
    public (float x, float y) direction = (45.0f, 45.0f);
    public float addForce;
    public float decreaseForceX;
    public float decreaseForceY;
    static float addForceX;
    static float addForceY;
    static float addForceZ;

    public void BehaviourFrisbee(Vector3 dir)
    {
        this.GetComponent<Rigidbody>().AddForce(dir);
    }
    void Start()
    {
        addForceX = addForce * Mathf.Cos(direction.y * (Mathf.PI / 180.0f)) * Mathf.Sin(direction.x * (Mathf.PI / 180.0f));
        addForceY = addForce * Mathf.Sin(direction.y * (Mathf.PI / 180.0f));
        addForceZ = addForce * Mathf.Cos(direction.y * (Mathf.PI / 180.0f)) * Mathf.Cos(direction.x * (Mathf.PI / 180.0f));
        BehaviourFrisbee(new Vector3(addForceX, addForceY, addForceZ));
    }

    void Update()
    {
        if(direction.x >=0)
        {
            BehaviourFrisbee(new Vector3(decreaseForceX, decreaseForceY, 0));
        }
        else
        {
            BehaviourFrisbee(new Vector3(-decreaseForceX, decreaseForceY, 0));
        }
    }
}

