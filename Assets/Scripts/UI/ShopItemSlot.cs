using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{
    public Image ItemImage;
    public Text ItemName;
    public Text ItemCost;
    public Button BuyButton;

    public void UpdateDisplay(Item _item)
    {
        ItemImage.sprite = _item.Image;
        ItemName.text = _item.Name;
        ItemCost.text = _item.Cost.ToString();
    }
}
