using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public GameObject destroyGame;
    public Transform parent;
    private PlayerDataUI playerDataUI;
    bool conditionMet = false; // Флаг для хранения информации о выполнении условия
    [SerializeField] button_menu button_menu;
    public int countWin = 0;


    private void Start()
    {
        playerDataUI = GameObject.FindAnyObjectByType<PlayerDataUI>();
    }
    void Update()
    {
        // Проверяем, выполнено ли условие и флаг еще не установлен
        if (countWin == button_menu.winnigNumber && !conditionMet)
        {
            Destroy(destroyGame);


            int TaskID = SceneManager.GetActiveScene().buildIndex;
            DateTime EndTime = DateTime.Now;
            SQLiteDbConnection Conn = new SQLiteDbConnection();
            Conn.executeSetQuery("UPDATE Task SET EndTime ='" + EndTime.ToLongTimeString() + "' WHERE Id=" + TaskID);

            string query = "SELECT * FROM Task WHERE Id =" + TaskID;
            List<Dictionary<string, object>> result = Conn.executeGetQuery(query);

            DateTime StartTime = DateTime.Parse(result[0]["StartTime"].ToString());
            TimeSpan score = EndTime - StartTime;
            int totalScore = score.Seconds + score.Minutes * 60;

            Conn.executeSetQuery("UPDATE Task SET Result ='" + totalScore + "' WHERE Id=" + TaskID);

            int Result = Convert.ToInt32(result[0]["PaySize"]);
            if (totalScore >= 90)
            { Result -= (Result * totalScore / 90) / 100; }

            playerDataUI.victoryUI.onpenVictoryUI(Result);

            conditionMet = true; // Устанавливаем флаг после выполнения условия
        }
    }
}
