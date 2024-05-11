using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public void openCloseUI()
    {

        if (gameObject.activeSelf)
        {
            this.closeUI();
        }
        else
        {
            this.openUI();
        }
    }

    public void closeUI()
    {
        Game.openedUI.Remove(this);
        gameObject.SetActive(false);
        Game.resumeGame();
    }

    public void openUI()
    {
        Game.openedUI.Add(this);
        gameObject.SetActive(true);
        Game.pauseGame();
    }

  
}
