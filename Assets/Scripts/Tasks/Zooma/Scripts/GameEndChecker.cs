using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndChecker : MonoBehaviour
{

    private PlayerDataUI playerDataUI;

    private void Start()
    {
        playerDataUI = GameObject.FindAnyObjectByType<PlayerDataUI>();
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "ActiveBalls")
        {
            playerDataUI.looseUI.openCloseUI();

        }
    }
}
