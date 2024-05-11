namespace Task2
{
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class menuT2 : MonoBehaviour
    {
        private CatSayingUI catSayingUI;
        [SerializeField] button_menuT2 button_menuT2;
        private void Start()
        {
            catSayingUI = GameObject.FindAnyObjectByType<PlayerDataUI>().catUI;
        }

        void Update()
        {
            if (button_menuT2.cloze_flag == true)
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
}