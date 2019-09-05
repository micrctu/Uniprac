using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public List<Item> itemList { get; private set; }

    private PlayerManager thePlayer;
    private PlayerStats theStat;

    private void Awake()
    {
        #region SingleTon
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        #endregion Singleton
    }

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theStat = thePlayer.gameObject.GetComponent<PlayerStats>();

        itemList = new List<Item>();
        AddItemToList(new Item(10001, "빨간 포션", "체력 50 회복", Item.ItemType.Use));
        AddItemToList(new Item(10002, "파란 포션", "마력 15 회복", Item.ItemType.Use));
        AddItemToList(new Item(10003, "농축 빨간 포션", "체력 350 회복", Item.ItemType.Use));
        AddItemToList(new Item(10004, "농축 파란 포션", "마력 80 회복", Item.ItemType.Use));
        AddItemToList(new Item(11001, "랜덤 상자", "랜덤 포션 획득(꽝 가능)", Item.ItemType.Use));
        AddItemToList(new Item(20001, "짧은 검", "기본적인 용사의 검", Item.ItemType.Equip, 3));
        AddItemToList(new Item(21001, "사파이어 반지", "hp가 지속 회복되는 반지", Item.ItemType.Equip, 0, 0, 1));
        AddItemToList(new Item(30001, "고대 유물의 조각1", "반으로 쪼개진 고대 유물1", Item.ItemType.Quest));
        AddItemToList(new Item(30002, "고대 유물의 조각2", "반으로 쪼개진 고대 유물2", Item.ItemType.Quest));
        AddItemToList(new Item(30003, "고대 유물", "고대 유적에 잠들어 있던 유물", Item.ItemType.Quest));
    }

    public void UseItem(int _itemID)
    {
        switch(_itemID)
        {
            case 10001:
                theStat.Recover_Hp(50);
                break;
            case 10002:
                theStat.Recover_Mp(15);
                break;
            case 10003:
                theStat.Recover_Hp(350);
                break;
            case 10004:
                theStat.Recover_Mp(80);
                break;
            default:
                Debug.Log("아직 미구현");
                break;
        }
    }

    public void AddItemToList(Item _item)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemID == _item.itemID)
                return;
        }
        itemList.Add(_item);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
