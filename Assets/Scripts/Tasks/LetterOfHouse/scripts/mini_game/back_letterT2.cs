namespace Task2
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    public class back_letterT2 : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (transform.childCount == 0)
            {
                GameObject dropped = eventData.pointerDrag;
                letterT2 letter2 = dropped.GetComponent<letterT2>();
                letter2.parentAfterDrag = transform;
            }

        }
    }
}