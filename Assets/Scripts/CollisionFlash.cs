using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionFlash : MonoBehaviour
{
    public float lifeTime;
    static float timeCount = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        if(timeCount >= lifeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
