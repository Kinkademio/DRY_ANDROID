using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUIButton : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI taskNumberText;
    private int scheneTaskNumber;

    PlayerDataUI mainUI;
    LifesEndsUI lifesEndsUI;

    private void Start()
    {
        mainUI = GameObject.FindAnyObjectByType<PlayerDataUI>();
        lifesEndsUI = mainUI.lifesUI;
    }

    public void setNumber(int taskNumber)
    {
        taskNumberText.text = taskNumber.ToString();
    }

    public void setScheneNumber(int number)
    {
        scheneTaskNumber = number;
    }

    public void loadTask()
    {
        if(Game.getPlayersLifes() > 0)
        {
            Game.closeAllUI();
            SceneManager.LoadSceneAsync(scheneTaskNumber, LoadSceneMode.Single);
        }
        else
        {
            lifesEndsUI.openCloseUI();
        }

 
    }
}
