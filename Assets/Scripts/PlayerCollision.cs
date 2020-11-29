using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] int lifeDecrasePoll;
    [SerializeField] int lifeDecraseWall;
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
        }
        else
        {
            GameController.diskCount -= lifeDecraseWall;
        }
    }
}
