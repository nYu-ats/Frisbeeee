using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
アイテムゲット時の処理
*/
public class Item : MonoBehaviour
{
    //アイテム獲得時の音
    [SerializeField] GameObject itemGetSound;
    void OnTriggerEnter(Collider collision)
    {
        //衝突対象がDiskの場合のみアイテムゲット
        if(collision.gameObject.tag == "Disk")
        {
            //アイテム未所持かどうか判断
            if(!GameController.ReturnItemStatus(this.transform.parent.gameObject.tag))
            {
                this.GetItem(this.transform.parent.gameObject.tag);
            }
        }
    }

    //アイテムゲット時の処理
    private void GetItem(string item)
    {
        //アイテムを所持状態にする
        GameController.GetItem(item);
        //効果音再生
        Instantiate(itemGetSound, this.transform.position, Quaternion.Euler(0, 0, 0));
        //自身を消去
        Destroy(this.transform.parent.gameObject);
    }

}
