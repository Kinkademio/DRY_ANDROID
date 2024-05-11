namespace Task2
{
    using UnityEngine;
    using UnityEngine.UI;

    public class HouseT2 : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI textObject;

        public void setText(string text)
        {
            this.textObject.text = text;
        }
    }
}