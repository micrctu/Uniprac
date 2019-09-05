using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
        
    public GameObject[] tab_Panels;
    public Text desc_Field;
    public string[] tab_Desc;
    public GameObject slot_Parent;

    private InventorySlot[] inventorySlots;
    private List<Item> inventoryItemList; //플레이어가 소지한 아이템 리스트
    private List<Item> inventoryTabList; //선택한 탭(소모품, 장비 등)에 따라 다르게 보여질 아이템 리스트
    private Ok_or_Cancel theOOC;

    private int currentTab;
    private int currentSlot;
    private int page;
    private const int MAX_SLOTS_IN_PAGE = 12;

    public GameObject go; //인벤토리 활성화/비활성화

    private bool activated; //전체 인벤토리 활성화 여부
    private bool tabActivated; //탭영역 활성화 여부
    private bool itemSlotActivated; //슬롯영역 활성화 여부
    public bool keyInputEnabled; //다른 작업중이라 인벤토리 키인풋 막아야할때

    private WaitForSeconds waitTime = new WaitForSeconds(0.02f);

    private void Awake()
    {
        #region SingleTon
        if (instance == null)
        {
            instance = this;
    //        DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        #endregion Singleton
    }

    // Start is called before the first frame update
    void Start()
    {
        go.SetActive(false);

        currentTab = 0;
        currentSlot = 0;
        page = 1;

        activated = false;
        tabActivated = false;
        itemSlotActivated = false;
        keyInputEnabled = true;

        inventorySlots = slot_Parent.GetComponentsInChildren<InventorySlot>();
        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        theOOC = FindObjectOfType<Ok_or_Cancel>();
    }

    private void StartInventory()
    {
        OrderManager.instance.SetPlayerNotMove();

        go.SetActive(true);
        currentTab = 0;
        currentSlot = 0;
        page = 1;
        tabActivated = true;
        itemSlotActivated = false;

        ClearSlots();
        SelectedTabEffect();
    }

    private void ShowSlots(int startSlot = 0)
    {
        ClearSlots();

        StopAllCoroutines();  //코루틴을 통한 탭부분 반짝임을 멈추고 해당 탭 선택한 것 처럼 이펙트
        Color color = tab_Panels[currentTab].GetComponent<Image>().color;
        color.a = 0.25f;
        tab_Panels[currentTab].GetComponent<Image>().color = color;

        switch (currentTab)
        {
            case 0:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (inventoryItemList[i].itemType == Item.ItemType.Use)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 1:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (inventoryItemList[i].itemType == Item.ItemType.Equip)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 2:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (inventoryItemList[i].itemType == Item.ItemType.Quest)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
            case 3:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (inventoryItemList[i].itemType == Item.ItemType.ETC)
                        inventoryTabList.Add(inventoryItemList[i]);
                }
                break;
        }

        for (int i = (page - 1) * MAX_SLOTS_IN_PAGE; i < inventoryTabList.Count; i++)
        {
            if (i < MAX_SLOTS_IN_PAGE)
                currentSlot = i;
            else
                currentSlot = i % ((page - 1) * MAX_SLOTS_IN_PAGE);

            inventorySlots[currentSlot].gameObject.SetActive(true);
            inventorySlots[currentSlot].AddItemToSlot(inventoryTabList[i]);

            if (currentSlot == MAX_SLOTS_IN_PAGE - 1)
                break;
        }

        currentSlot = startSlot; // 시작 슬롯, 즉 페이지 이동시 자연스러운 시작슬롯 위치 지정을 위해
        if (inventoryTabList.Count > 0)
            SelectedSlotEffect();
        else
        {
            desc_Field.text = "해당 아이템이 없습니다";
            tabActivated = true;
            itemSlotActivated = false;
        }
    }

    private void SelectedSlotEffect()
    {
        StopAllCoroutines();

        Color color = inventorySlots[0].itemPanel.color;
        color.a = 0f;

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].itemPanel.color = color;
        }
        desc_Field.text = inventoryTabList[currentSlot].itemDesc;

        StartCoroutine(SelectedSlotEffectCoroutine());
    }

    private IEnumerator SelectedSlotEffectCoroutine()
    {
        Color color = inventorySlots[currentSlot].itemPanel.color;

        while (itemSlotActivated)
        {
            while (color.a < 0.5f)
            {
                color.a += 0.04f;
                inventorySlots[currentSlot].itemPanel.color = color;
                yield return waitTime;
            }

            while (color.a > 0f)
            {
                color.a -= 0.04f;
                inventorySlots[currentSlot].itemPanel.color = color;
                yield return waitTime;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void ClearSlots()
    {
        currentSlot = 0;
        inventoryTabList.Clear();

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].RemoveItemFromSlot();
            inventorySlots[i].gameObject.SetActive(false);
        }
    }

    private void SelectedTabEffect()
    {
        StopAllCoroutines();

        Color color = tab_Panels[0].GetComponent<Image>().color;
        color.a = 0f;

        for (int i = 0; i < tab_Panels.Length; i++)
        {
            tab_Panels[i].GetComponent<Image>().color = color;
        }
        desc_Field.text = tab_Desc[currentTab];

        StartCoroutine(SelectedTabEffectCoroutine());
    }

    private IEnumerator SelectedTabEffectCoroutine()
    {
        Color color = tab_Panels[currentTab].GetComponent<Image>().color;

        while(tabActivated)
        {
            while (color.a < 0.5f)
            {
                color.a += 0.04f;
                tab_Panels[currentTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }

            while (color.a > 0f)
            {
                color.a -= 0.04f;
                tab_Panels[currentTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }   
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void ExitInventory()
    {
        StopAllCoroutines();

        currentTab = 0;
        currentSlot = 0;
        page = 1;
        tabActivated = false;
        itemSlotActivated = false;
        
        go.SetActive(false);
        OrderManager.instance.SetPlayerMove();
    }

    public void AddItemtoInventory(Item _item)
    {
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            if(inventoryItemList[i].itemID == _item.itemID)
            {
                if (inventoryItemList[i].itemType == Item.ItemType.Use)
                {
                    inventoryItemList[i].itemCount += _item.itemCount;
                    return;
                }
                else
                    break;
            }
        }
        inventoryItemList.Add(_item);
    }

    private IEnumerator OOC_Coroutine(string _Ok, string _Cancel)
    {
        keyInputEnabled = false;
        theOOC.ShowOOC(_Ok, _Cancel);

        yield return new WaitUntil(() => !theOOC.activated);

        if(theOOC.GetResult())
        {
            Debug.Log("사용 선택");
            ItemManager.instance.UseItem(inventoryTabList[(page - 1) * MAX_SLOTS_IN_PAGE + currentSlot].itemID);
        }
        else
        {
            Debug.Log("취소 선택");
        }

        keyInputEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(keyInputEnabled)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                activated = !activated;
                if (!activated)
                {
                    ExitInventory();
                }
                else
                {
                    StartInventory();
                }
            }
            else if (tabActivated)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (currentTab == 0)
                        currentTab = tab_Panels.Length - 1;
                    else
                        currentTab--;

                    SelectedTabEffect();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (currentTab == tab_Panels.Length - 1)
                        currentTab = 0;
                    else
                        currentTab++;

                    SelectedTabEffect();
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    tabActivated = false;
                    itemSlotActivated = true;

                    ShowSlots();
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    activated = false;
                    ExitInventory();
                }
            }
            else if (itemSlotActivated)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (currentSlot == 0)
                    {
                        if (page > 1)
                        {
                            page--;
                            ShowSlots(MAX_SLOTS_IN_PAGE - 1);
                        }
                    }
                    else
                    {
                        currentSlot--;
                        SelectedSlotEffect();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (currentSlot == MAX_SLOTS_IN_PAGE - 1)
                    {
                        if (inventoryTabList.Count > page * MAX_SLOTS_IN_PAGE)
                        {
                            page++;
                            ShowSlots(0);
                        }
                    }
                    else
                    {
                        if (currentSlot + ((page - 1) * MAX_SLOTS_IN_PAGE) == inventoryTabList.Count - 1)
                        { }
                        else
                            currentSlot++;
                        SelectedSlotEffect();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (currentSlot == 0)
                    {
                        if (page > 1)
                        {
                            page--;
                            ShowSlots(MAX_SLOTS_IN_PAGE - 2);
                        }

                    }
                    else if (currentSlot == 1)
                    {
                        if (page > 1)
                        {
                            page--;
                            ShowSlots(MAX_SLOTS_IN_PAGE - 1);
                        }
                    }
                    else
                    {
                        currentSlot = currentSlot - 2;
                        SelectedSlotEffect();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (currentSlot == MAX_SLOTS_IN_PAGE - 1)
                    {
                        if (inventoryTabList.Count > page * MAX_SLOTS_IN_PAGE)
                        {
                            if (inventoryTabList.Count > (page * MAX_SLOTS_IN_PAGE) + 1)
                            {
                                page++;
                                ShowSlots(1);
                            }
                            else
                            {
                                page++;
                                ShowSlots(0);
                            }
                        }
                    }
                    else if (currentSlot == MAX_SLOTS_IN_PAGE - 2)
                    {
                        if (inventoryTabList.Count > page * MAX_SLOTS_IN_PAGE)
                        {
                            page++;
                            ShowSlots(0);
                        }
                    }
                    else if (currentSlot == (inventoryTabList.Count - 1) - ((page - 1) * MAX_SLOTS_IN_PAGE))
                    { }
                    else if (currentSlot == (inventoryTabList.Count - 1) - ((page - 1) * MAX_SLOTS_IN_PAGE) - 1)
                    { }
                    else
                    {
                        currentSlot = currentSlot + 2;
                        SelectedSlotEffect();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (currentTab == 0)
                        StartCoroutine(OOC_Coroutine("사용", "취소"));
                    else if (currentTab == 1)
                        StartCoroutine(OOC_Coroutine("장비", "취소"));
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    tabActivated = true;
                    itemSlotActivated = false;

                    page = 1;
                    ClearSlots();
                    SelectedTabEffect();
                }
            }
        }
        
    }
}
