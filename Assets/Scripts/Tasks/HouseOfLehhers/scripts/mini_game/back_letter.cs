using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class back_letter : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            letter letter2 = dropped.GetComponent<letter>();
            letter2.parentAfterDrag = transform;
        }

    }
}
