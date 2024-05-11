
using UnityEngine;
using UnityEngine.EventSystems;

public class House_letter : MonoBehaviour, IDropHandler
{
    public TMPro.TextMeshProUGUI textHouse;
    public int idLetter;

    PlayerDataUI mainUI;
    CatSayingUI catSayingUI;
    LifesEndsUI lifesEndsUI;
    Health health;

    private void Start()
    {
        mainUI = GameObject.FindAnyObjectByType<PlayerDataUI>();
        catSayingUI = mainUI.catUI;
        lifesEndsUI = mainUI.lifesUI;
        health = GameObject.FindObjectOfType<Health>(); 
    }


    private void DestroyHealth(string letter, string house)
    {
        Game.changePlayerLifeCount(-1);
        string say = "Буква " + letter + " не првратилась в букву " + house + " попробуй еще раз!";
        catSayingUI.say(say);
    }
    public void OnDrop(PointerEventData eventData)
    { 
        if (Game.getPlayersLifes() <= 0)
        {
            lifesEndsUI.openCloseUI();
            return;
        }


        GameObject dropped = eventData.pointerDrag;
        letter letter = dropped.GetComponent<letter>();
        switch (letter.textLetter.text)
            {
                case "к":
                    if ((textHouse.text == "ч") || (textHouse.text == "ц"))
                    {
                        Destroy(this.gameObject);
                        health.countWin++;
                    }
                    else
                        DestroyHealth(letter.textLetter.text, textHouse.text);
                break;
                case "г":
                if ((textHouse.text == "ж") || (textHouse.text == "з"))
                    {
                        Destroy(this.gameObject);
                        health.countWin++;
                    }
                    else
                        DestroyHealth(letter.textLetter.text, textHouse.text);
                break;
                case "х":
                if ((textHouse.text == "щ") || (textHouse.text == "ш") || (textHouse.text == "с"))
                    {
                        Destroy(this.gameObject);
                        health.countWin++;
                    }
                    else
                        DestroyHealth(letter.textLetter.text, textHouse.text);
                break;
            }
        
    }
}
