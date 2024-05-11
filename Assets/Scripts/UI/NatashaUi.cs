using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatashaUi : UIBase
{
    [SerializeField] TMPro.TextMeshProUGUI speachTextObject;

    
    public void changeNatashaText(string text)
    {
        speachTextObject.text = text;
    }

    public void sayRandom()
    {
        if (Time.timeScale == 0) return;
        string query = "SELECT * FROM CityPhrases WHERE Id_City = 1";
        SQLiteDbConnection connector = new SQLiteDbConnection();
        List<Dictionary<string, object>> result = connector.executeGetQuery(query);

        string text = result[Random.Range(0, result.Count)]["Text"].ToString();

        changeNatashaText(text);
        openUI();
    }

}
