using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Button itemButton;
    [SerializeField] string thisItemName;
    [SerializeField] GameController gameController;
    [SerializeField] DiskGenerator diskGenerator;
    [SerializeField] Image[] itemUsingImage;
    [SerializeField] const float canItemUseTime = 5.0f;

    void Start()
    {
        itemImage.enabled = false;
        itemButton.enabled = false;
    }

    void Update()
    {
        //アイテム所持状態を確認して、アイテム取得時にば画像をenableにする
        if(gameController.GetHaveItemStatus(thisItemName))
        {
            itemImage.enabled = true;
            itemButton.enabled = true;
        }
    }

    //アイテム消費の処理
    public void ItemConsume()
    {
        diskGenerator.TappCancel(); //アイテムタップ時にディスクが生成されるのを防ぐ
        foreach(Image img in itemUsingImage)
        {
            //アイテム使用中を示すイメージを表示する
            img.enabled = true;
        }
        itemImage.enabled = false;
        itemButton.enabled = false;
        gameController.SetHaveItemStatus(thisItemName, false); //アイテム所持 -> 未所持へフラグを変更
        gameController.SetItemUseStatus(thisItemName, true, canItemUseTime); //アイテムを使用状態にして、使用可能時間をセット
        StartCoroutine(DisactivateItemUsingImage(canItemUseTime));
    }

    //アイテム使用中を示すイメージは、規定時間(アイテム使用可能時間と同等)経過で非表示へ
    IEnumerator DisactivateItemUsingImage(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach(Image img in itemUsingImage)
        {
            img.enabled = false;
        }
    }
}
