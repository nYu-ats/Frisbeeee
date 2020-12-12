using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] GameObject collisionSound;
    [SerializeField] int lifeDecrasePoll;
    [SerializeField] int lifeDecraseWall;
    [SerializeField] Image collisionEffect1;
    [SerializeField] GameObject collisionEffect2;
    [SerializeField] GameObject playerCamera;
    [SerializeField] float displayTime = 1.0f;
    [SerializeField] GameObject diskDecreasePopUp;
    [SerializeField] GameObject uiCanvas;



    private bool collisionPlayFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "RedPoll" | collision.gameObject.tag == "BluePoll" | collision.gameObject.tag == "WhitePoll")
        {
            GameController.diskCount -= lifeDecrasePoll;
            DisplayDecreasePopUp();
        }
        else if(collision.gameObject.tag == "Disk")
        {

        }
        else
        {
            GameController.diskCount -= lifeDecraseWall;
            DisplayDecreasePopUp();
        }
    }

    private void DisplayDecreasePopUp()
    {
        GameObject popUp = Instantiate(diskDecreasePopUp);
        popUp.transform.SetParent(uiCanvas.transform, false);
    }

    void OnTriggerEnter(Collider collision)
    {
        if(!collisionPlayFlag & (collision.gameObject.tag != "Disk"))
        {
            collisionPlayFlag = true;
            collisionEffect1.enabled = true;
            playerCamera.GetComponent<Animator>().SetBool("PlayerCollision", true);
            collisionEffect2.GetComponent<Animator>().SetBool("PlayerCollision", true);
            Instantiate(collisionSound, this.transform.position, Quaternion.Euler(0, 0, 0));
            StartCoroutine(CollisionEffectDisactivate(collisionEffect1, collisionEffect2, playerCamera, displayTime));
        }
    }

    IEnumerator CollisionEffectDisactivate(Image img1, GameObject img2, GameObject cameraObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        img1.enabled = false;
        img2.GetComponent<Animator>().SetBool("PlayerCollision", false);
        cameraObj.GetComponent<Animator>().SetBool("PlayerCollision", false);
        collisionPlayFlag = false;
    }
    
}
