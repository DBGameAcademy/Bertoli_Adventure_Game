using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : MonoBehaviour
{
    public List<ShopItemSlot> BuySlots = new List<ShopItemSlot>();
    public ItemSlot SellSlot;
    public Text SellText;

    private void OnEnable()
    {
        SetupItems();

        UIController.Instance.PlayerInventoryUI.gameObject.SetActive(true);
    }

    void SetupItems()
    {
        int itemCount = Random.Range(0, 5);
        List<Item> items = new List<Item>();

        for (int i = 0; i < itemCount; i++)
        {
            items.Add(ItemController.Instance.Items.GetRandomItem());
        }

        for (int i = 0; i < BuySlots.Count; i++)
        {
            BuySlots[i].gameObject.SetActive(i < items.Count);
            if (i < items.Count)
            {
                int index = i;
                BuySlots[i].UpdateDisplay(items[i]);
                BuySlots[i].BuyButton.onClick.RemoveAllListeners();
                BuySlots[i].BuyButton.onClick.AddListener(delegate {
                    BuyButtonClicked(items[index]);
                });
            }
        }
    }

    public void SellItemAdded()
    {
        Item sellItem = SellSlot.Draggable.Item;
        SellText.text = "G" + sellItem.Cost.ToString();
        SellSlot.Draggable.gameObject.SetActive(true);
    }

    public void SellButtonClicked()
    {
        if (SellSlot.Draggable.Item != null)
        {
            GameController.Instance.Player.Gold += SellSlot.Draggable.Item.Cost;
            SellSlot.Draggable.Item = null;
            SellSlot.Draggable.gameObject.SetActive(false);
            SellText.text = "G0";
        }
    }

    void BuyButtonClicked(Item _item)
    {
        if (GameController.Instance.Player.Gold >= _item.Cost)
        {
            if (UIController.Instance.PlayerInventoryUI.HasSpace())
            {
                UIController.Instance.PlayerInventoryUI.AddItemToInventory(_item);
                GameController.Instance.Player.Gold -= _item.Cost;
            }
        }
    }

    private void OnDisable()
    {
        UIController.Instance.PlayerInventoryUI.gameObject.SetActive(false);
    }
}
