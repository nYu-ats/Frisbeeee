using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BLINDED_AM_ME;

public class Polls : MonoBehaviour
{
    static GameObject victim;
    static Vector3 normalDirection;
    static Vector3 anchorpoint;
    static Material capMaterial;
    GameObject[] cuttedObjects;

    // Start is called before the first frame update
    void Start()
    {
        victim = GameObject.Find("poll_sample");
        capMaterial = victim.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("trigger");
        GetComponent<CapsuleCollider>().enabled = false;
        BLINDED_AM_ME.MeshCut.Cut(victim, new Vector3(-0.5f, 0.0f, 5.0f), new Vector3(0.5f, 0.5f, 0.0f) , capMaterial);
        /*
        normalDirection = collision.contacts[0].normal;
        anchorpoint = collision.contacts[0].point;
        Debug.Log(normalDirection);
        Debug.Log(anchorpoint);
        BLINDED_AM_ME.MeshCut.Cut(victim, anchorpoint, normalDirection , capMaterial);
        
        Debug.Log(cuttedObjects[0]);
        Debug.Log(cuttedObjects[1]);
        foreach(GameObject obj in cuttedObjects)
        {
            Instantiate(obj);
        }
        Destroy(victim);
        */
    }
}
