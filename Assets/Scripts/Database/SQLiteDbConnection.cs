using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEditor;
using UnityEngine.SocialPlatforms;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using Mono.Data.Sqlite;

public class SQLiteDbConnection: MonoBehaviour
{


    protected string SqliteFileNameOnly = "localdb.db";
    protected string SqliteFileNameWithPath;


    public SQLiteDbConnection()
    {


#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", SqliteFileNameOnly);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, SqliteFileNameOnly);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + SqliteFileNameOnly);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + SqliteFileNameOnly;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + SqliteFileNameOnly;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + SqliteFileNameOnly;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + SqliteFileNameOnly;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + SqliteFileNameOnly;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif

        SqliteFileNameWithPath = "URI = file:" + dbPath;
    }

    /**
     * Метод для организации операций ЧТЕНИЯ SELECT к примеру
     */
    private List<Dictionary<string, object>> GetInformationFromDB(string request)
    {
        List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();


        using (var connection = new SqliteConnection(SqliteFileNameWithPath))
        {
            //Открываем подключение к базе
            connection.Open();

            using (var command = connection.CreateCommand())
            {   //Выполняем команду к базе
                command.CommandText = request;
                IDataReader reader1 = command.ExecuteReader();
                result = GetArrayOfObjectsFromIDataReader(reader1);
   
            }
            connection.Close();
        }
        return result;
    }

    /**
     * Преобразование к удобочитаемым данным
     */
    private List<Dictionary<string, object>> GetArrayOfObjectsFromIDataReader(IDataReader reader)
    {
        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        while (reader.Read())
        {
            Dictionary<string, object> row = new Dictionary<string, object>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);
                object columnValue = reader.GetValue(i);
                row[columnName] = columnValue;
            }
            data.Add(row);
        }
        return data;
    }


    /**
    * Метод для организации операций ЗАПИСИ INSERT UPDATE и тп
    */
    private int SetInformation(string request)
    {
        int operationResult = 0;
        using (var connection = new SqliteConnection(SqliteFileNameWithPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = request;
                operationResult = command.ExecuteNonQuery();
            }
            connection.Close();
        }
        return operationResult;

    }

    /**
     * Выполняет GET запрос к локальной базе данных
     * SELECT
     */
    public List<Dictionary<string, object>> executeGetQuery(string queryText)
    {
        return GetInformationFromDB(queryText);
    }

    /**
    * Выполняет SET запрос к локальной базе данных
    * INSERT, UPDATE и т.п
    */
    public int executeSetQuery(string queryText)
    {
        return SetInformation(queryText);
    }

}
