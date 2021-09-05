using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemSlot : MonoBehaviour
{
    public Item.eItemType SlotType;
    public DraggableItem Draggable;

    public UnityEvent OnDropHandler;

    public void OnDrop(DraggableItem _draggedItem)
    {
        Item tempItem = Draggable.Item;
        Draggable.Item = _draggedItem.Item;
        if (tempItem != null)
        {
            _draggedItem.Item = tempItem;
        }
        else
        {
            _draggedItem.Item = null;
        }

        if (SlotType == Item.eItemType.Armour)
        {
            GameController.Instance.Player.EquipedArmour = Draggable.Item as ArmourItem;
        }
        if (SlotType == Item.eItemType.Weapon)
        {
            GameController.Instance.Player.HeldWeapon = Draggable.Item as WeaponItem;
        }

        UpdateItemDisplay();
        _draggedItem.ParentSlot.UpdateItemDisplay();

        OnDropHandler?.Invoke();
    }

    public void UpdateItemDisplay()
    {
        if (Draggable.Item != null)
        {
            Draggable.ItemImage.gameObject.SetActive(true);
            Draggable.ItemImage.sprite = Draggable.Item.Image;
        }
        else
        {
            Draggable.ItemImage.gameObject.SetActive(false);
        }
    }
}
