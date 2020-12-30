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
    public GameController gameController;

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>(); //prefab化されておりインスペクターから設定手出来ないのでスクリプトで紐づける
        cameraSpeed =  GameObject.FindWithTag("MainCamera").GetComponent<CameraMove>().MoveSpeed;
        //加える力を3軸方向に分解
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

    void FixedUpdate()
    {
        //ディスクを直進させるアイテムを使っている時はカーブさせない
        //アイテム未使用状態の時は一定後からでカーブさせる
        if(gameController.GetItemUseStatus("Straight"))
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
    [SerializeField] GameObject pollHitSound;
    [SerializeField] GameObject wallHitSound;
    private Vector3 collisionPosition;

    void OnTriggerEnter(Collider collision)
    {
        //ポールヒット時のパーティクル、フラッシュ、効果音を再生
        //アイテムもトリガーがtrueになっているため、ポールヒット時かどうかif文で判定する
        if((collision.gameObject.tag == "WhitePoll") | (collision.gameObject.tag == "RedPoll") | (collision.gameObject.tag == "BluePoll"))
        {
            collisionPosition = this.transform.position; //衝突が発生した位置
            Instantiate(pollHitSound, collisionPosition, Quaternion.Euler(0, 0, 0));
            Instantiate(effectCollisionFlash, collisionPosition, Quaternion.Euler(0, 0, 0));
            Instantiate(pollCutParticle, collisionPosition, Quaternion.Euler(0, 0, 0));

        }
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

