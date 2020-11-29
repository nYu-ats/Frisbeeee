using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatableBlock : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float blockLength;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.transform.position.z - speed * Time.deltaTime);
        if(this.gameObject.transform.position.z < -blockLength)
        {
            Destroy(this.gameObject);
        }
    }
}
