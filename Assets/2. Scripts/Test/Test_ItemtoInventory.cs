using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ItemtoInventory : MonoBehaviour
{
    public int itemID;
    public int itemCount;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.Z))
        {
            for (int i = 0; i < ItemManager.instance.itemList.Count; i++)
            {
                if(ItemManager.instance.itemList[i].itemID == itemID)
                {
                    Inventory.instance.AddItemtoInventory(new Item(ItemManager.instance.itemList[i], itemCount));
                    Destroy(this.gameObject);
                    return;
                }
                    
            }
            Debug.Log("아이템리스트에 없는 아이템입니다");
            Destroy(this.gameObject);
            return;
        }
    }
}
