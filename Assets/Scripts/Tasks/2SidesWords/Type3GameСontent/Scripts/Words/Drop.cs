using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Drop : MonoBehaviour, IDropHandler
{
    public GenerationWords generationWords;
    public TMPro.TextMeshProUGUI textDropSlot;
    PlayerDataUI mainUI;
    CatSayingUI catSayingUI;

    private void Start()
    {
        mainUI = GameObject.FindAnyObjectByType<PlayerDataUI>();
        catSayingUI = mainUI.catUI;

        DateTime StartTime = DateTime.Now;
        SQLiteDbConnection Conn = new SQLiteDbConnection();
        int TaskID = SceneManager.GetActiveScene().buildIndex;
        Conn.executeSetQuery("UPDATE Task SET StartTime ='" + StartTime.ToLongTimeString() + "' WHERE Id=" + TaskID);
        Debug.Log(StartTime);
    }
    private void DestroyHealth(string textwords, string textDropSlot)
    {
        Game.changePlayerLifeCount(-1);
        string say = "Слово " + textwords + " не пишется с буквой " + textDropSlot +" !";
        catSayingUI.say(say);
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        WordsDrag word = dropped.GetComponent<WordsDrag>();
        if (word.tag == this.gameObject.tag)
        {
            Destroy(dropped);
            generationWords.countWord++;
            Debug.Log("Правильно");
        }
        else
        {
            DestroyHealth(word.textWords.text, textDropSlot.text);
            
            Debug.Log("Неправильно");
        }
    }
}
