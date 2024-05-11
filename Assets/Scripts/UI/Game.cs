using System;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public enum gameState
    {
        onPause,
        onProcess,
    }

    public static gameState currentGameState = gameState.onProcess;
    public static List<UIBase> openedUI =new List<UIBase>();

    public static void changeGameState(gameState newState)
    {
        switch (newState)
        {
            case gameState.onProcess:
                Time.timeScale = 1.0f;
                break;
            case gameState.onPause:
                Time.timeScale = 0f;
                break;
        }
        currentGameState = newState;
    }

    public static void pauseGame()
    {
        if (currentGameState != gameState.onPause)
        {
            changeGameState(gameState.onPause);
        }
       
    }

    public static void resumeGame()
    {
        if (currentGameState == gameState.onPause && openedUI.Count == 0)
        {
            changeGameState(gameState.onProcess);
        }
       
    }

    public static void changePlayerLifeCount(int countOfLifes)
    {
        SQLiteDbConnection connection = new SQLiteDbConnection();
        string queryGet = "SELECT * FROM User";
        List<Dictionary<string, object>> getUserResult = connection.executeGetQuery(queryGet);
        int currentUserLifes = 0;
        int totalUserLifes = 0;

        currentUserLifes = Convert.ToInt32(getUserResult[0]["Lifes"]);
        totalUserLifes = Convert.ToInt32(getUserResult[0]["LifesTotal"]);

        int newLifeCount = currentUserLifes + countOfLifes;
        if(newLifeCount < 0)
        {
            newLifeCount = 0;
        }

        string query = "UPDATE User SET Lifes = " + newLifeCount + " WHERE Id = 1";
        connection.executeSetQuery(query);

        PlayerDataUI playerUI = GameObject.FindAnyObjectByType<PlayerDataUI>();
        playerUI.initUI();

    }

    public static void changePlayerCoins(int countOfCoins)
    {
        SQLiteDbConnection connection = new SQLiteDbConnection();
        string queryGet = "SELECT * FROM User";
        List<Dictionary<string, object>> getUserResult = connection.executeGetQuery(queryGet);
        int currentUserCoins = 0;
        currentUserCoins = Convert.ToInt32(getUserResult[0]["Coins"]);

        int newCoinsCount = currentUserCoins + countOfCoins;
        if (newCoinsCount < 0)
        {
            newCoinsCount = 0;
        }

        string query = "UPDATE User SET Coins = " + newCoinsCount + " WHERE Id = 1";
        connection.executeSetQuery(query);

        PlayerDataUI playerUI = GameObject.FindAnyObjectByType<PlayerDataUI>();
        playerUI.initUI();
    }


    public static int getPlayersLifes()
    {
        SQLiteDbConnection connection = new SQLiteDbConnection();
        string queryGet = "SELECT * FROM User";
        List<Dictionary<string, object>> getUserResult = connection.executeGetQuery(queryGet);
        return Convert.ToInt32(getUserResult[0]["Lifes"]);
    }

    

    public static void closeAllUI()
    {
        UIBase[] allUI = GameObject.FindObjectsByType<UIBase>(FindObjectsSortMode.None);

        foreach (UIBase ui in allUI)
        {
            ui.closeUI();
        }
    }


}
