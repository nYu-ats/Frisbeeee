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
    [SerializeField] float canItemUseTime = 5.0f;

    void Start()
    {
        itemImage.enabled = false;
        itemButton.enabled = false;
    }

    void Update()
    {
        if(gameController.GetHaveItemStatus(thisItemName))
        {
            itemImage.enabled = true;
            itemButton.enabled = true;
        }
    }

    public void ItemConsume()
    {
        diskGenerator.TappCancel();
        foreach(Image img in itemUsingImage)
        {
            img.enabled = true;
        }
        itemImage.enabled = false;
        itemButton.enabled = false;
        gameController.SetHaveItemStatus(thisItemName, false);
        gameController.SetItemUseStatus(thisItemName, true, canItemUseTime);
        StartCoroutine(DisactivateItemUsingImage(canItemUseTime));
    }

    IEnumerator DisactivateItemUsingImage(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach(Image img in itemUsingImage)
        {
            img.enabled = false;
        }
    }
}
