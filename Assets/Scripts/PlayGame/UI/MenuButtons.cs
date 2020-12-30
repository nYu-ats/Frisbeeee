using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
メニューパネルの3つのボタンいずれも、クリック時にDisableMenuPanelが必要なので
このクラスにクリック処理をまとめて記述
*/
public class MenuButtons : MonoBehaviour
{
    [SerializeField] MenuPanelButton menuPanelButton;
    [SerializeField] GameController gameController;
    [SerializeField] Transform playerCamera;

    //ゲーム再開ボタンに割り当て
    public void DisableMenuPanel()
    {
        gameController.GamePause = false;
        menuPanelButton.SwitchMenuPanelDisplay(false);
    }

    //ステージはじめからリスタートボタンに割り当て
    public void RestartStageStart()
    {
        float[] eachStageArray = gameController.StageLength;
        float restartStagePositionZ = eachStageArray[gameController.StageNumber - 1];
        playerCamera.GetComponent<CameraMove>().RestartFlag = true;
        gameController.InitializeGameStatus();
        playerCamera.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, restartStagePositionZ);
        DisableMenuPanel();
    }

    //ホームに戻るボタンに割り当て
    public void ReturnToHome()
    {
        int stageNumber = gameController.StageNumber;
        DisableMenuPanel();
        gameController.InitializeGameStatus();
        gameController.SetReachedStage();
        gameController.ReturnToHome();;
    }

}
