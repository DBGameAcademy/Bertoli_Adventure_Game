using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Collection", menuName = "Create Item Collection")]
public class ItemCollection : ScriptableObject
{
    public List<Item> Items = new List<Item>();

    public Item GetItem(Item.eItemType _type)
    {
        List<Item> possibleItems = new List<Item>();

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].ItemType == _type)
            {
                possibleItems.Add(Items[i]);
            }
        }

        if (possibleItems.Count == 0)
        {
            Debug.LogError("No Monsters of Type " + _type);
        }

        Item item = possibleItems[Random.Range(0, possibleItems.Count)];

        return item;
    }

    public Item GetRandomItem()
    {
        return Items[Random.Range(0, Items.Count)];
    }
}
