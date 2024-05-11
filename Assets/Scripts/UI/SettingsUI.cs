using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SettingsUI : UIBase
{

    [SerializeField] GameObject toMainScheneButton;

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            toMainScheneButton.SetActive(false);
        }
        else
        {
            toMainScheneButton.SetActive(true);
        }
    }
}
