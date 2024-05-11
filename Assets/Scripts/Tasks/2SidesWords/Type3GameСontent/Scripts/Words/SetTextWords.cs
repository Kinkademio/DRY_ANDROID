using UnityEngine;
using UnityEngine.UI;

public class SetTextWords : MonoBehaviour
{
   public TMPro.TextMeshProUGUI textObject;

    public void setText(string text)
    {
        this.textObject.text = text;
    }
}
