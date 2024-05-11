using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : UIBase
{

    [SerializeField] private TMPro.TextMeshProUGUI m_TextMeshPro;

    public void onpenVictoryUI(int ernedCoins)
    {
        Game.changePlayerCoins(ernedCoins);
        m_TextMeshPro.text = ernedCoins.ToString();
        base.openUI();
    }

}
