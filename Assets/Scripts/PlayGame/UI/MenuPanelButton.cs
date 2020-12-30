using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuPanelButton : MonoBehaviour
{
    [SerializeField] Button[] eachMenuButton;
    [SerializeField] Image[] eachMenuImage;
    [SerializeField] Text[] eachMenuText;
    [SerializeField] GameController gameController;
    [SerializeField] DiskGenerator diskGenerator;    

    void Start()
    {
       SwitchMenuPanelDisplay(false);
    }

    public void PauseGame()
    {
        diskGenerator.TappCancel(); //メニューボタンタップ時にディスクが生成させるのを防ぐ
        gameController.GamePause = true;
        SwitchMenuPanelDisplay(true);
    }

    //メニューパネルの表示非表示を切り替える
    public void SwitchMenuPanelDisplay(bool swtichStatus)
    {
        foreach(Button obj in eachMenuButton)
        {
            obj.enabled = swtichStatus;
        }
        foreach(Image obj in eachMenuImage)
        {
            obj.enabled = swtichStatus;
        }
        foreach(Text obj in eachMenuText)
        {
            obj.enabled = swtichStatus;
        }
    }
}
