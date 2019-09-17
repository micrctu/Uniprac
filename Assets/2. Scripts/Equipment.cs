using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public static Equipment instance;

    public GameObject go;
    public GameObject[] slots;
    public GameObject selectedSlotEffect;
    public Ok_or_Cancel theOOC;

    public Text[] texts;
    private const int ATK = 0, DEF = 1, HRE = 6, MRE = 7;

    private List<Item> equipmentList;
    private const int WEAPON = 0, SHIELD = 1, AMULET = 2, LEFT_RING = 3, RIGHT_RING = 4, HELMET = 5,
                      ARMOR = 6, LEFT_GLOVE = 7, RIGHT_GLOVE = 8, BELT = 9, LEFT_BOOTS = 10, RIGHT_BOOTS = 11;

    private int added_Atk, added_Def, added_Hre, added_Mre;

    private int selectedSlot;

    public bool keyInputEnabled;
    public bool activated;

    private PlayerStats theStats;

    private void Awake()
    {
        #region SingleTon
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);
        #endregion Singleton
    }

    // Start is called before the first frame update
    void Start()
    {
        theStats = PlayerManager.instance.gameObject.GetComponent<PlayerStats>();

        equipmentList = new List<Item>();
        for (int i = 0; i < slots.Length; i++)
        {
            equipmentList.Add(new Item(0, "빈 아이템", "빈 아이템", Item.ItemType.ETC));
        }

        added_Atk = 0;
        added_Def = 0;
        added_Hre = 0;
        added_Mre = 0;

        selectedSlot = 0;
        keyInputEnabled = true;
        activated = false;
    }

    private void ClearSlots()
    {
        Color color = slots[0].GetComponent<Image>().color;
        color.a = 0f;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].GetComponent<Image>().color = color;
            slots[i].GetComponent<Image>().sprite = null;
        }
    }

    public void EquipItem(Item _item)
    {
        string itemNo = _item.itemID.ToString().Substring(0, 3);
        switch(itemNo)
        {
            case "200":
                if(equipmentList[WEAPON].itemID != 0)
                {
                    UnEquipEffect(equipmentList[WEAPON]);
                    ItemtoInventory(equipmentList[WEAPON]); 
                }
                equipmentList[WEAPON] = _item;
                EquipEffect(equipmentList[WEAPON]);
                break;
            case "210":
                if (equipmentList[LEFT_RING].itemID == 0)
                {
                    equipmentList[LEFT_RING] = _item;
                    EquipEffect(equipmentList[LEFT_RING]);
                }
                else if (equipmentList[RIGHT_RING].itemID == 0)
                {
                    equipmentList[RIGHT_RING] = _item;
                    EquipEffect(equipmentList[RIGHT_RING]);
                }
                else // left ring, right ring 둘다 끼고 있는 경우 left ring 자리 교체
                {
                    UnEquipEffect(equipmentList[LEFT_RING]);
                    ItemtoInventory(equipmentList[LEFT_RING]);
                    equipmentList[LEFT_RING] = _item;
                    EquipEffect(equipmentList[LEFT_RING]);
                }                    
                break;
            default:
                Debug.Log("아직 미구현");
                break;
        }
        ShowText();
    }

    private void EquipEffect(Item _item)
    {
        added_Atk += _item.atk;
        added_Def += _item.def;
        added_Hre += _item.recover_hp;
        added_Mre += _item.recover_mp;

        theStats.player_Atk += _item.atk;
        theStats.player_Def += _item.def;
        theStats.recover_Hp += _item.recover_hp;
        theStats.recover_Mp += _item.recover_mp;
    }

    private void UnEquipEffect(Item _item)
    {
        added_Atk -= _item.atk;
        added_Def -= _item.def;
        added_Hre -= _item.recover_hp;
        added_Mre -= _item.recover_mp;

        theStats.player_Atk -= _item.atk;
        theStats.player_Def -= _item.def;
        theStats.recover_Hp -= _item.recover_hp;
        theStats.recover_Mp -= _item.recover_mp;
    }

    private void ItemtoInventory(Item _item)
    {
        Inventory.instance.AddItemtoInventory(_item);
    }

    private void UnEquipItem(Item _item)
    {
        UnEquipEffect(_item);
        ItemtoInventory(_item);
        ShowText();

        equipmentList[selectedSlot] = new Item(0, "빈 아이템", "빈 아이템", Item.ItemType.ETC);//장비해제 후 빈아이템으로 초기화
    }

    private void LoadSlots()
    {
        Color color = slots[0].GetComponent<Image>().color;
        color.a = 1f;

        for (int i = 0; i < slots.Length; i++)
        {
            if(equipmentList[i].itemID != 0)
            {
                slots[i].GetComponent<Image>().color = color;
                slots[i].GetComponent<Image>().sprite = equipmentList[i].itemIcon;
            }
        }
        SelectedSlotEffect();
    }

    private void SelectedSlotEffect()
    {
        selectedSlotEffect.transform.position = slots[selectedSlot].transform.position;
    }

    private void ShowText()
    {
        if (added_Atk != 0)
            texts[ATK].text = theStats.player_Atk.ToString() + " (+" + added_Atk.ToString() + ")";
        else
            texts[ATK].text = theStats.player_Atk.ToString();

        if (added_Def != 0)
            texts[DEF].text = theStats.player_Def.ToString() + " (+" + added_Def.ToString() + ")";
        else
            texts[DEF].text = theStats.player_Def.ToString();

        if (added_Hre != 0)
            texts[HRE].text = theStats.recover_Hp.ToString() + " (+" + added_Hre.ToString() + ")";
        else
            texts[HRE].text = theStats.recover_Hp.ToString();

        if (added_Mre != 0)
            texts[MRE].text = theStats.recover_Mp.ToString() + " (+" + added_Mre.ToString() + ")";
        else
            texts[MRE].text = theStats.recover_Mp.ToString();
    }
    
    private IEnumerator OOC_Coroutine(string _up, string _down)
    {
        yield return new WaitForSeconds(0.01f); //z키 인풋 겹쳐서 실행되는 것 방지를 위함

        keyInputEnabled = false;
        theOOC.ShowOOC(_up, _down);
        yield return new WaitUntil(() => !theOOC.activated);

        if(theOOC.GetResult())
        {
            UnEquipItem(equipmentList[selectedSlot]);
            ClearSlots();
            LoadSlots();
        }
        keyInputEnabled = true;
    }

    private void ExitEquipment()
    {
        selectedSlot = 0;
        SelectedSlotEffect();
        go.SetActive(false);

        OrderManager.instance.SetPlayerMove();
        Inventory.instance.keyInputEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(keyInputEnabled)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                activated = !activated;
                if(activated)
                {
                    OrderManager.instance.SetPlayerNotMove();
                    Inventory.instance.keyInputEnabled = false;

                    go.SetActive(true);
                    ClearSlots();
                    LoadSlots();
                    ShowText();
                }
                else
                {
                    ExitEquipment();                   
                }
            }
            else if(activated)
            {
                if(Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (selectedSlot == 4)
                        selectedSlot = 0;
                    else if (selectedSlot == slots.Length - 1)
                        selectedSlot = 5;
                    else
                        selectedSlot++;
                    SelectedSlotEffect();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (selectedSlot == 0)
                        selectedSlot = 4;
                    else if (selectedSlot == 5)
                        selectedSlot = slots.Length - 1;
                    else
                        selectedSlot--;
                    SelectedSlotEffect();
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (selectedSlot <= 4)
                        selectedSlot = 5;
                   
                    SelectedSlotEffect();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (selectedSlot >= 5)
                        selectedSlot = 0;

                    SelectedSlotEffect();
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    if(equipmentList[selectedSlot].itemID != 0)
                    {
                        StartCoroutine(OOC_Coroutine("해제", "취소"));                       
                    }
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    activated = false;
                    ExitEquipment();
                }
            }

        }
        
    }
}
