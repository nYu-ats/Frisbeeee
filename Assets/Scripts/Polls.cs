﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BLINDED_AM_ME;

public class Polls : MonoBehaviour
{
    private GameObject victim;
    private Vector3 normalDirection;
    private Vector3 anchorpoint;
    private Material capMaterial;
    private Vector3 normalDirectionStd = new Vector3(0.0f, 1.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        victim = this.gameObject;
        capMaterial = victim.GetComponent<Renderer>().material;
        //切断後のオブジェクトに力を加えられるかのテスト
        if(victim.name == "right side")
        {
            victim.GetComponent<Rigidbody>().AddForce(1.0f, 0.0f, 0.0f, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Disk")
        {
            anchorpoint = collision.ClosestPointOnBounds(this.transform.position);
            normalDirection = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z) * normalDirectionStd;
            GetComponent<CapsuleCollider>().enabled = false;
            BLINDED_AM_ME.MeshCut.Cut(victim, anchorpoint, normalDirection , capMaterial);
            Destroy(victim);
        }
    }


}