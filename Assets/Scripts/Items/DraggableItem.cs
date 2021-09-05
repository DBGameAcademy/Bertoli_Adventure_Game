using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
    public Item Item;
    public ItemSlot ParentSlot;

    public Image ItemImage;
    Canvas canvas = null;
    RectTransform rectTransform = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        ItemSlot slot = null;

        foreach(RaycastResult result in results)
        {
            slot = result.gameObject.GetComponent<ItemSlot>();
            if (slot != null)
            {
                break;
            }
        }

        if (slot != null)
        {
            if (slot.SlotType == Item.eItemType.None || Item.ItemType == slot.SlotType)
            {
                slot.OnDrop(this);

                transform.SetParent(ParentSlot.transform);
                rectTransform.anchoredPosition = Vector2.zero;

                return;
            }
        }

        transform.SetParent(ParentSlot.transform);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    private void Awake()
    {
        ItemImage = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        ParentSlot = GetComponentInParent<ItemSlot>();
    }

}
