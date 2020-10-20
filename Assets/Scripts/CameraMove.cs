using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private Vector3 cameraPosition;
    private Quaternion cameraRotation;
    private float positionZ;
    // Start is called before the first frame update
    void Start()
    {
        cameraPosition = this.transform.position;
        cameraRotation = this.transform.rotation;
        positionZ = this.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    { 
        positionZ += moveSpeed * Time.deltaTime;
        this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, positionZ);
    }
}
