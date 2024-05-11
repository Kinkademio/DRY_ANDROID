using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI health;
    [SerializeField] private TMPro.TextMeshProUGUI coins;
    [SerializeField] public CatSayingUI catUI;
    [SerializeField] public LifesEndsUI lifesUI;
    [SerializeField] public VictoryUI victoryUI;
    [SerializeField] public LooseUI looseUI;

    private void Start()
    {
        initUI();
        StartCoroutine(lireRegenerateCorutine());
    }
    private void OnDestroy()
    {
        StopCoroutine(lireRegenerateCorutine());
    }

    IEnumerator lireRegenerateCorutine()
    {
        yield return new WaitForSeconds(300); // 300 секунд в 5 минутах
        Game.changePlayerLifeCount(1);
    }

    public void initUI()
    {
        SQLiteDbConnection connector = new SQLiteDbConnection();
        string query = "SELECT * FROM User";
        List<Dictionary<string, object>> result = connector.executeGetQuery(query);

        if (result.Count > 0)
        {
            health.text = result[0]["Lifes"].ToString() + '/' + result[0]["LifesTotal"].ToString();
            coins.text = result[0]["Coins"].ToString();
        }
    }
}
