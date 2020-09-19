using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Disk : MonoBehaviour
{
    public (float x, float y) direction = (30.0f, 30.0f);
    public float velocity = 50.0f;
    static float v_x;
    static float v_y;
    static float v_z;
    static float posX = 0.0f;
    static float posY = 0.0f;
    static float posZ = 0.0f;
    static float a_x = -4.9f;
    static float a_y = -9.8f;
    float t = 0.0f;

    void Start()
    {
        v_x = velocity * Mathf.Cos(direction.y * (Mathf.PI / 180.0f)) * Mathf.Sin(direction.x * (Mathf.PI / 180.0f));
        v_y = velocity * Mathf.Sin(direction.y * (Mathf.PI / 180.0f));
        v_z = velocity * Mathf.Cos(direction.y * (Mathf.PI / 180.0f)) * Mathf.Cos(direction.x * (Mathf.PI / 180.0f));
    }

    void Update()
    {
        t = Time.deltaTime;
        v_x += a_x *t;
        v_y += a_y *t;
        posX = v_x * t + a_x * Mathf.Pow(t, 2.0f) /2.0f + posX;
        posY = v_y * t + a_y * Mathf.Pow(t, 2.0f) /2.0f + posY;
        posZ = v_z * t + posZ;
        this.transform.Translate(posX, posY, posZ);
    }
}

