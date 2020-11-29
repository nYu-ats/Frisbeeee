using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectColorChang : MonoBehaviour
{
    [SerializeField] GameObject childObjBlue;
    [SerializeField] GameObject childObjRed;
    [SerializeField] GameObject childObjWhite;
    [SerializeField] string startColor;
    [SerializeField] float distance;
    [SerializeField] float colorChangeSpan;
    private float[] colorChangeRateBlueToRed = new float[3];
    private float[] colorChangeRateRedToWhite = new float[3];
    private float[] colorChangeRateWhiteToBlue  = new float[3];
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        colorChangeRateBlueToRed = new float[3]{(250.0f - 185.0f) / colorChangeSpan / 60, (130.0f - 240.0f) / colorChangeSpan / 60, (80.0f - 240.0f) / colorChangeSpan / 60};
        colorChangeRateRedToWhite = new float[3]{(255.0f - 250.0f) / colorChangeSpan / 60, (255.0f - 130.0f) / colorChangeSpan / 60, (255.0f - 80.0f) / colorChangeSpan / 60};
        colorChangeRateWhiteToBlue = new float[3]{(185.0f - 255.0f) / colorChangeSpan / 60, (240.0f - 255.0f) / colorChangeSpan / 60, (240.0f - 255.0f) / colorChangeSpan / 60}; 
        if(startColor == "Blue")
        {
            GameObject child = Instantiate(childObjBlue, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + distance, this.gameObject.transform.position.z), Quaternion.Euler(0, 0, 0));
            child.transform.parent = this.gameObject.transform;
        }
        else if(startColor == "Red")
        {
            GameObject child = Instantiate(childObjRed, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + distance, this.gameObject.transform.position.z), Quaternion.Euler(0, 0, 0));
            child.transform.parent = this.gameObject.transform;
        }
        else if(startColor == "White")
        {
            GameObject child = Instantiate(childObjWhite, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + distance, this.gameObject.transform.position.z), Quaternion.Euler(0, 0, 0));
            child.transform.parent = this.gameObject.transform;
        }
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
       if(this.gameObject.transform.GetChild(1).gameObject.tag == "BluePoll")
       {
           if(time < colorChangeSpan)
           {
               this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = new Color(
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.r + colorChangeRateBlueToRed[0],
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.g + colorChangeRateBlueToRed[1],
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.b + colorChangeRateBlueToRed[2]
               );
               this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Light>().color = new Color(
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.r + colorChangeRateBlueToRed[0],
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.g + colorChangeRateBlueToRed[1],
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.b + colorChangeRateBlueToRed[2]
               );
               time += Time.deltaTime;
           }
           else
           {
               Destroy(this.gameObject.transform.GetChild(1).gameObject);
               GameObject child = Instantiate(childObjRed, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + distance, this.gameObject.transform.position.z), Quaternion.Euler(0, 0, 0));
               child.transform.parent = this.gameObject.transform;
               time = 0;
           }
       }
       else if(this.gameObject.transform.GetChild(1).gameObject.tag == "RedPoll")
       {
           if(time < colorChangeSpan)
           {
               this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = new Color(
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.r + colorChangeRateRedToWhite[0],
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.g + colorChangeRateRedToWhite[1],
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.b + colorChangeRateRedToWhite[2]
               );           
               this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Light>().color = new Color(
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.r + colorChangeRateRedToWhite[0],
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.g + colorChangeRateRedToWhite[1],
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.b + colorChangeRateRedToWhite[2]
               );
               time += Time.deltaTime;
           }
           else
           {
               Destroy(this.gameObject.transform.GetChild(1).gameObject);
               GameObject child = Instantiate(childObjWhite, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + distance, this.gameObject.transform.position.z), Quaternion.Euler(0, 0, 0));
               child.transform.parent = this.gameObject.transform;
               time = 0;
           }
       }
       else if(this.gameObject.transform.GetChild(1).gameObject.tag == "WhitePoll")
       {
           if(time < colorChangeSpan)
           {
               this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = new Color(
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.r + colorChangeRateWhiteToBlue[0],
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.g + colorChangeRateWhiteToBlue[1],
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.b + colorChangeRateWhiteToBlue[2]
               );
               this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Light>().color = new Color(
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.r + colorChangeRateWhiteToBlue[0],
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.g + colorChangeRateWhiteToBlue[1],
                   this.gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color.b + colorChangeRateWhiteToBlue[2]
               );
               time += Time.deltaTime;
           }
           else
           {
               Destroy(this.gameObject.transform.GetChild(1).gameObject);
               GameObject child = Instantiate(childObjBlue, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + distance, this.gameObject.transform.position.z), Quaternion.Euler(0, 0, 0));
               child.transform.parent = this.gameObject.transform;
               time = 0;
           }

       }
    }
}
