using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMove : MonoBehaviour
{
    private Vector3 cameraPosition;
    [SerializeField] float adjustVolume = 10.0f;
    private int maxVolume = 2;
    [SerializeField] float distanceY = 100.0f;
    [SerializeField] float distanceZ = -100.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Volume") / maxVolume / adjustVolume;
        cameraPosition = GameObject.FindWithTag("MainCamera").transform.position;
        this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y + distanceY, cameraPosition.z + distanceZ);
    }

    // Update is called once per frame
    void Update()
    {
        cameraPosition = GameObject.FindWithTag("MainCamera").transform.position;
        this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y + distanceY, cameraPosition.z + distanceZ); 
    }
}
