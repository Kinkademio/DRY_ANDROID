using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WordsDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler//, IDropHandler
{
    public TMPro.TextMeshProUGUI textWords;
    [HideInInspector]public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        textWords.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        textWords.raycastTarget = true;
    }
}