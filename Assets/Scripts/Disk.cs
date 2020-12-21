using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
プレイヤーが射出するディスクの挙動
*/
public class Disk : MonoBehaviour
{
    [SerializeField] float addForce; //前方へ加える力
    [SerializeField] float decreaseForceX; //x方向のカーブさせる力
    [SerializeField] float decreaseForceY; //Y方向のカーブさせる力
    [SerializeField] float ToruqueX; //徐々にディスクを傾けるための角度
    public (float x, float y) direction; //ディスクを射出する角度
    private float addForceX;
    private float addForceY;
    private float addForceZ;
    private float cameraSpeed;


    void Start()
    {
        cameraSpeed = CameraMove.ReturnCameraSpeed();
        //初期で加える力を射出方向に分解
        addForceX = addForce * Mathf.Cos(direction.y * (Mathf.PI / 180.0f)) * Mathf.Sin(direction.x * (Mathf.PI / 180.0f));
        addForceY = addForce * Mathf.Sin(direction.y * (Mathf.PI / 180.0f));
        addForceZ = addForce * Mathf.Cos(direction.y * (Mathf.PI / 180.0f)) * Mathf.Cos(direction.x * (Mathf.PI / 180.0f));
        addForceZ += cameraSpeed * this.gameObject.GetComponent<Rigidbody>().mass; //カメラの移動速度からZ方向に追加で力を加える
        
        //フリスビーっぽい挙動のための処理
        //フリスビー射出方向に応じて傾けて、Z軸方向に徐々に回転させる
        if(direction.x >= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, direction.y);
            this.GetComponent<Rigidbody>().AddTorque(0, 0, ToruqueX, ForceMode.Acceleration);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -direction.y);
            this.GetComponent<Rigidbody>().AddTorque(0, 0, -ToruqueX, ForceMode.Acceleration);
        }

        //フリスビーを射出する
        this.GetComponent<Rigidbody>().AddForce(new Vector3(addForceX, addForceY, addForceZ));
    }

    void Update()
    {
        //ディスクを直進させるアイテムを使っている時は起動の調整はしない
        //アイテム未使用状態の時は一定後からでカーブさせる
        if(GameController.straightUsing)
        {
            return;
        }
        else
        {
            if(direction.x >=0)
            {
                this.GetComponent<Rigidbody>().AddForce(new Vector3(decreaseForceX, decreaseForceY, 0));
            }
            else
            {
                this.GetComponent<Rigidbody>().AddForce(new Vector3(-decreaseForceX, decreaseForceY, 0));
            }
        }
    }

    [SerializeField] GameObject pollCutParticle;
    [SerializeField] GameObject diskCrushParticle;
    [SerializeField] GameObject effectCollisionFlash;
    [SerializeField] int diskIncreaseNumberPoll;
    [SerializeField] GameObject pollHitSound;
    [SerializeField] GameObject wallHitSound;
    private Vector3 collisionPosition;

    void OnTriggerEnter(Collider collision)
    {
        //衝突が発生した位置で効果音とパーティクルを再生する
        collisionPosition = this.transform.position;
        if(collision.gameObject.tag == "BluePoll")
        {
            if(!GameController.colorStopUsing)
            {
                //青ポールをカットした時はゲージの青を増加
                //GameController.colorBarPosition -= colorIncreaseNumberPoll;
                //GameController.colorMarkPosition -= colorIncreaseNumberPoll;
            }
        }
        else if(collision.gameObject.tag == "RedPoll")
        {
            //赤ポールをカットした時はゲージの赤を増加
            if(!GameController.colorStopUsing)
            {
                //GameController.colorBarPosition += colorIncreaseNumberPoll;
                //GameController.colorMarkPosition += colorIncreaseNumberPoll;
            }
        }
        else if(collision.gameObject.tag == "WhitePoll")
        {
            //白ポールをカットした時はディスクを規定数回復
            //GameController.diskCount += diskIncreaseNumberPoll;
        }
        //ポールヒット時のパーティクル、フラッシュ、効果音を再生
        Instantiate(pollHitSound, collisionPosition, Quaternion.Euler(0, 0, 0));
        Instantiate(effectCollisionFlash, collisionPosition, Quaternion.Euler(0, 0, 0));
        Instantiate(pollCutParticle, collisionPosition, Quaternion.Euler(0, 0, 0));
    }

    //壁にはトリガーはつけていないので、壁に衝突した時の効果はOnCollisionEnterで再生する    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.root.gameObject.tag == "Wall")
        {
            collisionPosition = this.transform.position;
            if(collision.gameObject.tag != "Switch1")
            {
                //ディスクがスイッチに当たった時は専用の効果音を再生
                Instantiate(wallHitSound, collisionPosition, Quaternion.Euler(0, 0, 0));
            }
            Instantiate(diskCrushParticle, collisionPosition, Quaternion.Euler(0, 0, 0));
            Destroy(this.gameObject); //壁衝突時にディスクを破壊する
        } 
    }    
}

