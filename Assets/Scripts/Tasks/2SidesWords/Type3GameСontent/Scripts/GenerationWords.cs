using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerationWords : MonoBehaviour
{
    public Transform pointSpawn;
    public Transform parentWinWindow;
    public GameObject windowWinPrefab;
    public GameObject gameDestroy;
    public GameObject wordPrefab;
    private PlayerDataUI playerDataUI;
    [HideInInspector] public int countCreatWords = 0;
    [HideInInspector] public int countWord = 0;
    [HideInInspector] public int[] usedWords;
    [HideInInspector] public string[] wordsArray;
    bool conditionMet = false;
    public bool endGame = false;

    PlayerDataUI mainUI;
    private void Start()
    {
        mainUI = GameObject.FindAnyObjectByType<PlayerDataUI>();
        playerDataUI = GameObject.FindAnyObjectByType<PlayerDataUI>();
    }
    private string RandomWord(string[] word)
    {
        int randomWordNumber = UnityEngine.Random.Range(0, wordsArray.Length);
        return word[randomWordNumber];
    }
    private string[] DeleterLetter(string word)
    {
        string newWord = "";
        if (word.Count(c => c == 'и') > 0)
        {
            int index = word.IndexOf('и');
            newWord = word.Substring(0, index) + ".." + word.Substring(index + 1);
            return new string[] { newWord, "и" };
        }
        if (word.Count(c => c == 'ы') > 0)
        {
            int index = word.IndexOf('ы');
            newWord = word.Substring(0, index) + ".." + word.Substring(index + 1);
            return new string[] { newWord, "ы" };
        }
        return new string[] { newWord, "и" };
    }

    public void GenerationWordsI()
    {
        if (countWord < wordsArray.Length)
        {
        string[] WordTag = DeleterLetter(RandomWord(wordsArray));
        GameObject newWord = Instantiate(wordPrefab, pointSpawn.transform.position, Quaternion.identity, pointSpawn);
        SetTextWords newWordClass = newWord.GetComponent<SetTextWords>();
        newWordClass.setText(WordTag[0]);
        newWord.tag = WordTag[1];
        }
        else
        {
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
        }
    }

    void Update()
    {
        if (!(countCreatWords == countWord))
        {
            GenerationWordsI();
            countCreatWords++;
        }
        else
        {
            conditionMet = true;
        }

        if (conditionMet)
        {
            return;
        }
    }
}
