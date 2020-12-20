using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuPanel : MonoBehaviour
{
    public Button[] eachMenuButton;
    public Image[] eachMenuImage;
    public Text[] eachMenuText;
    void Start()
    {
       SwitchMenuPanelDisplay(false); 
    }

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
