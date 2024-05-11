namespace Task2
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class House_letterT2 : MonoBehaviour, IDropHandler
    {
        public TMPro.TextMeshProUGUI textHouse;
        public int idLetter;

        PlayerDataUI mainUI;
        CatSayingUI catSayingUI;
        LifesEndsUI lifesEndsUI;
        HealthT2 healthT2;

        private void Start()
        {
            mainUI = GameObject.FindAnyObjectByType<PlayerDataUI>();
            catSayingUI = mainUI.catUI;
            lifesEndsUI = mainUI.lifesUI;
            healthT2 = GameObject.FindObjectOfType<HealthT2>();
        }


        private void DestroyHealth(string letter, string house)
        {
            Game.changePlayerLifeCount(-1);
            string say = "Буква " + letter + " не является прототипом буквы " + house + " попробуй еще раз!";
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
            letterT2 letter = dropped.GetComponent<letterT2>();
            switch (textHouse.text)
            {
                case "к":
                    if ((letter.textLetter.text == "ч") || (letter.textLetter.text == "ц"))
                    {
                        Destroy(dropped);
                        healthT2.countWin++;
                    }
                    else
                        DestroyHealth(letter.textLetter.text, textHouse.text);
                    break;
                case "г":
                    if ((letter.textLetter.text == "ж") || (letter.textLetter.text == "з"))
                    {
                        Destroy(dropped);
                        healthT2.countWin++;
                    }
                    else
                        DestroyHealth(letter.textLetter.text, textHouse.text);
                    break;
                case "х":
                    if ((letter.textLetter.text == "щ") || (letter.textLetter.text == "ш") || (letter.textLetter.text == "с"))
                    {
                        Destroy(dropped);
                        healthT2.countWin++;
                    }
                    else
                        DestroyHealth(letter.textLetter.text, textHouse.text);
                    break;
            }

        }
    }
}
