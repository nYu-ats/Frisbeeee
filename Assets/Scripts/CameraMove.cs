using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 cameraPosition;
    private Quaternion cameraRotation;
    private float positionZ;
    [SerializeField] float stayTime;
    private bool proceedFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CameraPositionSet(stayTime));
    }

    // Update is called once per frame
    void Update()
    {
        //ステージ1のみmoveSpeedを5に設定
        if(GameController.ReturnDiskStatus())
        {
            if(GameController.stageNumber == 1)
            {
                moveSpeed = 5.0f;
            }
            else
            {
                moveSpeed = 10.0f;
            }
        }

        if(GameController.gamePause)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }

        if(GameController.restartFlag)
        {
            proceedFlag = false;
            StartCoroutine(CameraPositionSet(stayTime));
            GameController.restartFlag = false;
        }
        
        if(proceedFlag)
        {
            positionZ += moveSpeed * Time.deltaTime;
            this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, positionZ);
        }
    }

    IEnumerator CameraPositionSet(float stayTime)
    {
        cameraPosition = this.transform.position;
        cameraRotation = this.transform.rotation;
        positionZ = this.transform.position.z;
        yield return new WaitForSeconds(stayTime);
        proceedFlag = true;
    }
}
