using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Disk : MonoBehaviour
{
    public (float x, float y) direction;
    public float addForce;
    public float decreaseForceX;
    public float decreaseForceY;
    public float ToruqueX;
    static float addForceX;
    static float addForceY;
    static float addForceZ;
    static AudioSource audioPlayer;
    public AudioClip audioClipBall;
    public AudioClip audioClipPoll;

    public GameObject effectParticlePoll;
    public GameObject effectParticleBallRed;
    public GameObject effectParticleBallBlue;
    public GameObject effectParticleBallWhite;
    public Vector3 effectRotation;
    public GameObject effectCollisionFlash;
    static Vector3 collisionPosition;

    void Start()
    {
        //初期で加える力を射出方向に分解
        addForceX = addForce * Mathf.Cos(direction.y * (Mathf.PI / 180.0f)) * Mathf.Sin(direction.x * (Mathf.PI / 180.0f));
        addForceY = addForce * Mathf.Sin(direction.y * (Mathf.PI / 180.0f));
        addForceZ = addForce * Mathf.Cos(direction.y * (Mathf.PI / 180.0f)) * Mathf.Cos(direction.x * (Mathf.PI / 180.0f));
        DirFrisbee(direction.x, direction.y);
        RotateFrisbee(direction.x);
        BehaviourFrisbee(new Vector3(addForceX, addForceY, addForceZ));
        audioPlayer = this.gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        //xの向きに応じて加速度の向きを反転
        if(direction.x >=0)
        {
            BehaviourFrisbee(new Vector3(decreaseForceX, decreaseForceY, 0));
        }
        else
        {
            BehaviourFrisbee(new Vector3(-decreaseForceX, decreaseForceY, 0));
        }
    }

    //フリスビーを回転させるメソッド
    public void RotateFrisbee(float x)
    {
        if(x >= 0)
        {
            this.GetComponent<Rigidbody>().AddTorque(0, 0, ToruqueX, ForceMode.Acceleration);
        }
        else
        {
            this.GetComponent<Rigidbody>().AddTorque(0, 0, -ToruqueX, ForceMode.Acceleration);
        }
    }

    //フリスビーに力を加えるメソッド
    public void BehaviourFrisbee(Vector3 dir)
    {
        this.GetComponent<Rigidbody>().AddForce(dir);
    }

    //フリスビーを傾けるメソッド。
    public void DirFrisbee(float x, float z)
    {
        if(x >= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -z);
        }
        
    }

    void OnTriggerEnter(Collider collision)
    {
        collisionPosition = this.transform.position;
        if(collision.gameObject.tag == "RedBall")
        {
            audioPlayer.PlayOneShot(audioClipBall);
            Instantiate(effectCollisionFlash, collisionPosition, Quaternion.Euler(effectRotation));
            Instantiate(effectParticleBallRed, collisionPosition, Quaternion.Euler(effectRotation));
        }
        else if(collision.gameObject.tag == "BlueBall")
        {
            audioPlayer.PlayOneShot(audioClipBall);
            Instantiate(effectCollisionFlash, collisionPosition, Quaternion.Euler(effectRotation));
            Instantiate(effectParticleBallBlue, collisionPosition, Quaternion.Euler(effectRotation));
        }
        else if(collision.gameObject.tag == "WhiteBall")
        {
            audioPlayer.PlayOneShot(audioClipBall);
            Instantiate(effectCollisionFlash, collisionPosition, Quaternion.Euler(effectRotation));
            Instantiate(effectParticleBallWhite, collisionPosition, Quaternion.Euler(effectRotation));
        }
        else if((collision.gameObject.tag == "BluePoll") | (collision.gameObject.tag == "RedPoll") | (collision.gameObject.tag == "WhitePoll"))
        {
            audioPlayer.PlayOneShot(audioClipPoll);
            Instantiate(effectCollisionFlash, collisionPosition, Quaternion.Euler(effectRotation));
            Instantiate(effectParticlePoll, collisionPosition, Quaternion.Euler(effectRotation));
        }
    }
}

