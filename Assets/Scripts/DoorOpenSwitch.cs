using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
各ステージの仕切りとして設置している扉のスイッチが押されたときの処理
*/
public class DoorOpenSwitch : MonoBehaviour
{
    [SerializeField] GameObject switchPushedSound;
    //左右のドアを別々で格納
    [SerializeField] GameObject doorRight;
    [SerializeField] GameObject doorLeft;
    //スイッチオブジェクト本体
    [SerializeField] GameObject switchObj1;
    //スイッチオブジェクトの台座となっているブロック
    [SerializeField] GameObject switchObj2;
    //スイッチが押された後に設定するマテリアル
    [SerializeField] Material afterPushedMat;

    void OnTriggerEnter(Collider collision)
    {
        //衝突対象がディスクだった場合のみ処理を実行する
        if(collision.gameObject.tag == "Disk")
        {
            //左右のドアそれぞれが開くアニメーションを再生
            doorRight.GetComponent<Animator>().SetBool("OpenDoor", true);
            doorLeft.GetComponent<Animator>().SetBool("OpenDoor", true);
            //スイッチを押した音を再生
            Instantiate(switchPushedSound, this.transform.position, Quaternion.Euler(0, 0, 0));
            //1度スイッチが押されたらマテリアルを変える
            this.gameObject.GetComponent<MeshRenderer>().material = afterPushedMat;
            //1度スイッチが押されたらタグを書き換えて無効化する
            switchObj1.tag = "Wall";
            switchObj2.tag = "Wall";
        }
    }
}
