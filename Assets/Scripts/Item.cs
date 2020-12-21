using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
アイテムゲット時の処理
*/
public class Item : MonoBehaviour
{
    public GameController gameController;
    //アイテム獲得時の音
    [SerializeField] GameObject itemGetSound;

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>(); //prefab化されておりインスペクターから設定手出来ないのでスクリプトで紐づける
    }
    void OnTriggerEnter(Collider collision)
    {
        //衝突対象がDiskの場合のみアイテムゲット
        if(collision.gameObject.tag == "Disk")
        {
            //アイテム未所持かどうか判断
            if(!gameController.ReturnItemStatus(this.transform.parent.gameObject.tag))
            {
                this.GetItem(this.transform.parent.gameObject.tag);
            }
        }
    }

    //アイテムゲット時の処理
    private void GetItem(string item)
    {
        //アイテムを所持状態にする
        gameController.GetItem(item);
        //効果音再生
        Instantiate(itemGetSound, this.transform.position, Quaternion.Euler(0, 0, 0));
        //自身を消去
        Destroy(this.transform.parent.gameObject);
    }

}
