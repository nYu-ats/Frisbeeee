using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    [SerializeField] Image gameClearBackground;
    [SerializeField] Text gameClearText;
    [SerializeField] GameObject gameClearSound;
    private bool soundInstantiateFlag = false;

    public void DisplayGameClearEffect(Vector3 position)
    {
        gameClearBackground.enabled = true;
        gameClearText.enabled = true;
        //1回のみ効果音再生オブジェクトが生成されるようにする
        if(!soundInstantiateFlag)
        {
            Instantiate(gameClearSound, position, Quaternion.Euler(0, 0, 0));
            soundInstantiateFlag = true;
        }
    }

}
