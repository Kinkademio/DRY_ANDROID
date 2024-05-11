using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseLevelUI : UIBase
{

    [SerializeField] private GameObject buttonContent;
    [SerializeField] private GameObject taskChoseButtonPrefab;

    private void OnEnable()
    {
        clearTaskList();
        SQLiteDbConnection connector = new SQLiteDbConnection();
        string query = "SELECT * FROM Task";
        List<Dictionary<string, object>> result = connector.executeGetQuery(query);
        if(result.Count > 0)
        {
            int taskCounter = 1;
            foreach(Dictionary<string, object> res in result)
            {
                GameObject newbutton = Instantiate(taskChoseButtonPrefab, buttonContent.transform);
                LevelUIButton levelButton = newbutton.GetComponent<LevelUIButton>();
                levelButton.setNumber(taskCounter);
                levelButton.setScheneNumber(Convert.ToInt32(res["ScheneId"]));
                taskCounter++;
            }
        }
    }

    public void clearTaskList()
    {
        //Очищаем список кнопок
        foreach (Transform child in buttonContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

    }
}
