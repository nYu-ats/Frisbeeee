using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
メニューパネルの各ボタンにて、ボタンクリック時にメニューパネルの非表示処理を行うので
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
        gameController.SetGamePauseStatus(false);
        menuPanelButton.SwitchMenuPanelDisplay(false);
    }

    //ステージはじめからリスタートボタンに割り当て
    public void RestartStageStart()
    {
        GameController.SetRestartFlag(true);
        gameController.InitializeGameStatus();
        playerCamera.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, gameController.GetStageStartPosition());
        DisableMenuPanel();
    }

    //ホームに戻るボタンに割り当て
    public void ReturnToHome()
    {
        int stageNumber = gameController.GetStageNumber();
        DisableMenuPanel();
        gameController.InitializeGameStatus();
        gameController.SetReachedStage();
        gameController.ReturnToHome();;
    }

}
