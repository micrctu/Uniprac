using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Text itemName_Text;
    public Text itemCount_Text;
    public Image itemIcon;
    public Image itemPanel;

    public void AddItemToSlot(Item _item)
    {
        itemIcon.sprite = _item.itemIcon;
        itemName_Text.text = _item.itemName;
        if (_item.itemType == Item.ItemType.Use)
        {
            if (_item.itemCount > 0)
                itemCount_Text.text = "x" + _item.itemCount.ToString();
            else
                itemCount_Text.text = "";
        }
        else
            itemCount_Text.text = "";
    }

    public void RemoveItemFromSlot()
    {
        itemIcon.sprite = null;
        itemName_Text.text = "";
        itemCount_Text.text = "";
    }

}
