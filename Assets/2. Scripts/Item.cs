using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int itemID;
    public string itemName;
    public string itemDesc;
    public int itemCount;
    public Sprite itemIcon;
    public ItemType itemType;

    public int atk;
    public int def;
    public int recover_hp;
    public int recover_mp;

    public enum ItemType
    {
        Use,
        Equip,
        Quest,
        ETC
    }

    public Item(int _itemID, string _itemName, string _itemDesc, ItemType _itemType,
        int _atk = 0, int _def = 0, int _recover_hp = 0, int _recover_mp = 0, int _itemCount = 1)//생성자
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDesc = _itemDesc;
        itemType = _itemType;
        itemCount = _itemCount;

        atk = _atk;
        def = _def;
        recover_hp = _recover_hp;
        recover_mp = _recover_mp;

        itemIcon = Resources.Load("ItemIcon/" + itemID.ToString(),typeof(Sprite)) as Sprite;
        //Resources 폴더 내 해당 경로에 있는 asset 불러오기, typeof(sprite) 즉, sprite 타입으로 가져오고 형변환
        //이는 아이콘 그림 파일이름을 미리 itemID로 바꿔놨기에 가능
    }

    public Item(Item _item, int _itemCount = 1)//생성자
    {
        itemID = _item.itemID;
        itemName = _item.itemName;
        itemDesc = _item.itemDesc;
        itemType = _item.itemType;
        itemCount = _itemCount;

        atk = _item.atk;
        def = _item.def;
        recover_hp = _item.recover_hp;
        recover_mp = _item.recover_mp;

        itemIcon = Resources.Load("ItemIcon/" + itemID.ToString(), typeof(Sprite)) as Sprite;
        //Resources 폴더 내 해당 경로에 있는 asset 불러오기, typeof(sprite) 즉, sprite 타입으로 가져오고 형변환
        //이는 아이콘 그림 파일이름을 미리 itemID로 바꿔놨기에 가능
    }
}
