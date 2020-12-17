using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BLINDED_AM_ME;

/*
ポールの挙動を制御するスクリプト
*/

public class Polls : MonoBehaviour
{
    //自身のgemeobjectを格納する変数
    private GameObject victim;
    //ポールの長辺方向の法線を格納する変数
    private Vector3 normalDirection;
    //衝突が発生した座標を格納する変数
    private Vector3 anchorpoint;
    //カット前のマテリアル
    private Material capMaterial;
    private Vector3 normalDirectionStd = new Vector3(0.0f, 1.0f, 0.0f);
    //カット後のマテリアル
    [SerializeField] Material cuttedMaterial;
    //rootオブジェクトのZ座標
    public float rootPositionZ;
    //ポールが破壊されたときの、ディスク数とカラーバーの値の変化のポップアップ
    [SerializeField] GameObject diskIncreasePopUp;
    [SerializeField] GameObject blueIncreasePopUp;
    [SerializeField] GameObject redIncreasePopUp;
    [SerializeField] float lifeTime = 5.0f; //カット後のポールの存在時間
    //ポップアップの親となるCanvas
    private GameObject uiCanvas;


    void Start()
    {
        victim = this.gameObject;
        capMaterial = victim.GetComponent<Renderer>().material;
        rootPositionZ = victim.transform.root.position.z;
        //Canvasを設定
        uiCanvas = GameObject.Find("Canvas");
        /*
        切断後のポールに対する処理
        切断後、オブジェクトを少し回転させる
        */
        if(victim.name == "right side")
        {
           victim.GetComponent<Rigidbody>().AddTorque(-10, 0, 0, ForceMode.Impulse);
           StartCoroutine(DestroyCuttedPoll(this.gameObject, lifeTime));
        }
        else if(victim.name == "left side")
        {
           victim.GetComponent<Rigidbody>().AddTorque(10, 0, 0, ForceMode.Impulse);
           StartCoroutine(DestroyCuttedPoll(this.gameObject, lifeTime));
        }
    }


    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Disk")
        {
            //Diskが衝突した座標を取得
            anchorpoint = collision.ClosestPointOnBounds(this.transform.position);
            //ポールの回転に応じて法線方向を調整
            normalDirection = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z) * normalDirectionStd;
            //colliderを無効にする
            GetComponent<CapsuleCollider>().enabled = false;
            //切断処理
            BLINDED_AM_ME.MeshCut.Cut(victim, anchorpoint, normalDirection , capMaterial, cuttedMaterial, rootPositionZ);
            //元のポールは消去
            Destroy(victim);
            //タグでポップアップの種類を判断
            if(this.gameObject.tag == "WhitePoll")
            {
                DisplayPopUp(diskIncreasePopUp);
            }
            else if(this.gameObject.tag == "RedPoll")
            {
                DisplayPopUp(redIncreasePopUp);
            }
            else if(this.gameObject.tag == "BluePoll")
            {
                DisplayPopUp(blueIncreasePopUp);
            }
        }
    }

    private void DisplayPopUp(GameObject obj)
    {
        GameObject popUp = Instantiate(obj);
        //canvasを親オブジェクトに設定
        popUp.transform.SetParent(uiCanvas.transform, false);
    }

    IEnumerator DestroyCuttedPoll(GameObject cuttedPoll, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(cuttedPoll);
    }

}