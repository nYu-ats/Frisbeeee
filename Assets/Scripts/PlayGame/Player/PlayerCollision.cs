using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] GameObject collisionSound;
    [SerializeField] int lifeDecrasePoll; //ポールに衝突した時のディスクの減少量
    [SerializeField] int lifeDecraseWall; //壁に衝突した時のディスクの減少量
    [SerializeField] Image collisionEffect;
    [SerializeField] float effectPlayTime = 0.5f;
    [SerializeField] GameObject pollHitPopUp; //ポールへの衝突によるディスクの減少量のポップアップ
    [SerializeField] GameObject wallHitPopUp; //壁への衝突によるディスクの減少量のポップアップ
    [SerializeField] GameObject uiCanvas;
    [SerializeField] GameController gameController;
    
    //衝突時の処理を1だけしかを行輪内容にするためOnTriggerExitを使う
    void OnTriggerExit(Collider collision)
    {
        //タグで衝突対象を判断
        if(collision.gameObject.tag == "RedPoll" | collision.gameObject.tag == "BluePoll" | collision.gameObject.tag == "WhitePoll")
        {
            gameController.DiskCount = -lifeDecrasePoll; //規定個数ディスクを減らす
            PlayCollisionEffect(); //衝突時の効果を再生
            DisplayDecreasePopUp(pollHitPopUp, lifeDecrasePoll); //ポップアップを出す
        }
        else if(collision.gameObject.tag == "Disk" | collision.gameObject.tag == "Disactive")
        {
            //ディスクを発射した際にコライダーに衝突してしまうことがあるので
            //衝突対象がディスクだった場合は処理を無視する
            //加えてカット後のポールの衝突も無視する
        }
        else
        {
            //ポールとディスク以外で衝突しうるオブジェクトは壁系のオブジェクト
            gameController.DiskCount = -lifeDecraseWall;
            PlayCollisionEffect();
            DisplayDecreasePopUp(wallHitPopUp, lifeDecraseWall);
        }
    }

    //ポップアップの表示
    private void DisplayDecreasePopUp(GameObject hit, int decreaseNumber)
    {
        GameObject popUp = Instantiate(hit);
        popUp.transform.Find("Text").GetComponent<Text>().text = "-" + decreaseNumber.ToString(); //Canvasに表示する前に衝突対象に応じた値をポップアップにセットする
        popUp.transform.SetParent(uiCanvas.transform, false);
    }

    //障害物に衝突した時の効果を再生する
    private void PlayCollisionEffect()
    {
        this.transform.parent.GetComponent<Animator>().SetBool("PlayerCollision", true); //カメラの子オブジェクトにプレイヤーとしてのコライダーをセットしている
        collisionEffect.enabled = true;
        Instantiate(collisionSound, this.transform.position, Quaternion.Euler(0, 0, 0));
        StartCoroutine(CollisionEffectDisactivate(collisionEffect, this.transform.parent.gameObject, effectPlayTime));  
    }

    //規定時間経過で衝突効果をfalseに再セット
    IEnumerator CollisionEffectDisactivate(Image img1, GameObject cameraObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        img1.enabled = false;
        cameraObj.GetComponent<Animator>().SetBool("PlayerCollision", false);
    }
    
}
