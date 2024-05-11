using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LifesEndsUI : UIBase
{
    public void backToCity()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            Game.closeAllUI();
        }
        else
        {
            Game.closeAllUI();
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        }
    }
}
