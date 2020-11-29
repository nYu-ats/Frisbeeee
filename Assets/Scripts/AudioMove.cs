using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMove : MonoBehaviour
{
    private Vector3 cameraPosition;
    [SerializeField] float distanceOffset = 50.0f;

    // Start is called before the first frame update
    void Start()
    {
        cameraPosition = GameObject.FindWithTag("MainCamera").transform.position;
        this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, cameraPosition.z + distanceOffset);
    }

    // Update is called once per frame
    void Update()
    {
        cameraPosition = GameObject.FindWithTag("MainCamera").transform.position;
        this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, cameraPosition.z + distanceOffset); 
    }
}
