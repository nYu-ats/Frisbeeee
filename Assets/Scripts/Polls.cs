using System.Collections;
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
    [SerializeField] Material cuttedMaterial;
    public bool isLive = true;
    public float globalPositionZ;
    [SerializeField] GameObject diskIncreasePopUp;
    [SerializeField] GameObject blueIncreasePopUp;
    [SerializeField] GameObject redIncreasePopUp;
    private GameObject uiCanvas;


    // Start is called before the first frame update
    void Start()
    {
        victim = this.gameObject;
        capMaterial = victim.GetComponent<Renderer>().material;
        //切断後のオブジェクトに力を加えられるかのテスト
        if(victim.name == "right side")
        {
           victim.GetComponent<Rigidbody>().AddTorque(-30, 0, 0, ForceMode.Impulse); 
        }
        else if(victim.name == "left side")
        {
           victim.GetComponent<Rigidbody>().AddTorque(30, 0, 0, ForceMode.Impulse); 
        }
        globalPositionZ = this.gameObject.transform.root.position.z;
        uiCanvas = GameObject.Find("Canvas");
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
            BLINDED_AM_ME.MeshCut.Cut(victim, anchorpoint, normalDirection , capMaterial, cuttedMaterial, globalPositionZ);
            Destroy(victim);
            if(this.gameObject.tag == "WhitePoll")
            {
                GameObject popUp = Instantiate(diskIncreasePopUp);
                popUp.transform.SetParent(uiCanvas.transform, false);
            }
            else if(this.gameObject.tag == "RedPoll")
            {
                GameObject popUp = Instantiate(redIncreasePopUp);
                popUp.transform.SetParent(uiCanvas.transform, false);
            }
            else if(this.gameObject.tag == "BluePoll")
            {
                GameObject popUp = Instantiate(blueIncreasePopUp);
                popUp.transform.SetParent(uiCanvas.transform, false);
            }
        }
    }


}