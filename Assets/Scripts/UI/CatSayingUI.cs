using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatSayingUI : UIBase
{

    [SerializeField] TMPro.TextMeshProUGUI speachTextObject;
     
    public void sayAbouteCity(int cityId)
    {
        if (Time.timeScale == 0) return; 
        SQLiteDbConnection connection = new SQLiteDbConnection();
        string query = "SELECT * FROM City WHERE Id = " + cityId;

        List<Dictionary<string, object>> result = connection.executeGetQuery(query);

        if(result.Count > 0)
        {
            changeText(result[0]["History"].ToString());
        }
        openCloseUI();
    }


    public void sayRandom()
    {
        SQLiteDbConnection connection = new SQLiteDbConnection();
        string query = "SELECT * FROM catPhrases";

        List<Dictionary<string, object>> result = connection.executeGetQuery(query);

        if (result.Count > 0)
        {
            int randomId = UnityEngine.Random.Range(0, result.Count);
            changeText(result[randomId]["Text"].ToString());
        }
        openCloseUI();

    }

    public void sayAbouteTask()
    {
        int taskNumber = SceneManager.GetActiveScene().buildIndex;
        if (taskNumber == 0) return;

        SQLiteDbConnection connection = new SQLiteDbConnection();
        string query = "SELECT * FROM Task WHERE Id = "+ taskNumber;

        List<Dictionary<string, object>> result = connection.executeGetQuery(query);

        if (result.Count > 0)
        {
            int randomId = UnityEngine.Random.Range(0, result.Count);
            changeText(result[randomId]["CatPhrase"].ToString());
        }
        openCloseUI();
    }

    public void say(string text)
    {
        changeText(text);
        openCloseUI();
    }

   
    public void changeText(string text)
    {
        speachTextObject.text = text;
    }


}
