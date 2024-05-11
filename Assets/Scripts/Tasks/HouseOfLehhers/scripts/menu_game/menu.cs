    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    private CatSayingUI catSayingUI;
    [SerializeField] button_menu button_menu;
    private void Start()
    {
        catSayingUI = GameObject.FindAnyObjectByType<PlayerDataUI>().catUI;
    }
    void Update()
    {
        if (button_menu.cloze_flag == true)
        {
            Destroy(this.gameObject);

            catSayingUI.sayAbouteTask();

            DateTime StartTime = DateTime.Now;
            SQLiteDbConnection Conn = new SQLiteDbConnection();
            int TaskID = SceneManager.GetActiveScene().buildIndex;
            Conn.executeSetQuery("UPDATE Task SET StartTime ='" + StartTime.ToLongTimeString() + "' WHERE Id=" + TaskID);
            Debug.Log(StartTime);
        }
    }
}
