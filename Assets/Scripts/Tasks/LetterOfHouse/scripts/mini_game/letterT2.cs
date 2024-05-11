namespace Task2
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class letterT2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public TMPro.TextMeshProUGUI textLetter;
        public int idletter = 0;
        [HideInInspector] public Transform parentAfterDrag;
        public Image image;

        public void OnBeginDrag(PointerEventData eventData)
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }


    }
}
