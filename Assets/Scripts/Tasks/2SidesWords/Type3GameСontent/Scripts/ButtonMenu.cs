using UnityEngine;

public class ButtonMenu : MonoBehaviour
{
    public GameObject menuDestroyPrefab;
    public Transform parentGame;
    public string[] wordsArrayButton;
    public GenerationWords generationWords;
    private CatSayingUI catSayingUI;
    private void Start()
    {
        catSayingUI = GameObject.FindAnyObjectByType<PlayerDataUI>().catUI;
    }
    public void ButClickEasy()
    {

        generationWords.wordsArray = wordsArrayButton[0..16];
        generationWords.GenerationWordsI();
        catSayingUI.sayAbouteTask();
        Destroy(menuDestroyPrefab);
      
    }
    public void ButClickNormal()
    {
        generationWords.wordsArray = wordsArrayButton[0..20];
        generationWords.GenerationWordsI();
       
        catSayingUI.sayAbouteTask();
        Destroy(menuDestroyPrefab);
    }
    public void ButClickHard()
    {
        generationWords.wordsArray = wordsArrayButton[0..24];
        generationWords.GenerationWordsI();
      
        catSayingUI.sayAbouteTask();
        Destroy(menuDestroyPrefab);
    }
}
