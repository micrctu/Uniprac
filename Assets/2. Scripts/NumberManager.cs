using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberManager : MonoBehaviour
{
    public static NumberManager instance;

    private int[] numbers;

    public GameObject[] numberPanels;
    public Text[] numberTexts;

    private int column;
    private string correct_Number;
    private int numbers_Column;
    private bool result;
    private Animator theAnim;

    public bool choicing;
    private bool keyinput;

    private void Awake()
    {
        #region Singleton
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
        theAnim = this.GetComponent<Animator>();

        for (int i = 0; i < numberPanels.Length; i++)
        {
            numberPanels[i].SetActive(false);
        }

        choicing = false;
        keyinput = false;
    }

    public void ShowNumber(string rightNumber, int _Pnumber = 4)
    {
        choicing = true;

        correct_Number = rightNumber;
        numbers_Column = _Pnumber;
        numbers = new int[numbers_Column];

        for (int i = 0; i < numbers_Column; i++)
        {
            numberTexts[i].text = "0";
            numberPanels[i].SetActive(true);
        }
        this.gameObject.SetActive(true);

        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x - (45 * (4 - numbers_Column)),
            this.gameObject.transform.position.y, this.gameObject.transform.position.z);  //자리수에 따른 중앙 정렬

        theAnim.SetBool("Appear", true);
        column = 0;
        result = false;
        Selection();
        keyinput = true;
    }

    private void Selection()
    {
        Color color = numberTexts[0].color;
        color.a = 0.3f;

        for (int i = 0; i < numberTexts.Length; i++)
        {
            numberTexts[i].color = color;
        }
        color.a = 1.0f;
        numberTexts[column].color = color;
    }

    private void SelectionAll()
    {
        Color color = numberTexts[0].color;
        color.a = 1.0f;

        for (int i = 0; i < numberTexts.Length; i++)
        {
            numberTexts[i].color = color;
        }
    }

    private void CheckNumber()
    {
        SelectionAll();
        StartCoroutine(CheckNumberCoroutine());
    }

    private IEnumerator CheckNumberCoroutine()
    {
        yield return new WaitForSeconds(1.0f);

        string tmp = "";

        for (int i = numbers_Column - 1; i >= 0; i--)
        {
            tmp += numberTexts[i].text;
        }
        Debug.Log(tmp);
        if (correct_Number.Equals(tmp))
        {
            result = true;
        }
        else
        {
            result = false;
        }
        StartCoroutine(ExitNumberCoroutine());
    }

    public bool GetResult()
    {
        return result;
    }

    private IEnumerator ExitNumberCoroutine()
    {
        theAnim.SetBool("Appear", false);

        yield return new WaitForSeconds(0.2f); //애니메이션 끝나는 시간 보장

        for (int i = 0; i < numberPanels.Length; i++)
        {
            numberTexts[i].text = "0";
            numberPanels[i].SetActive(false);
        }

        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + (45 * (4 - numbers_Column)),
            this.gameObject.transform.position.y, this.gameObject.transform.position.z);

        this.gameObject.SetActive(false);

        choicing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(keyinput)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                keyinput = false;
                CheckNumber();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (numbers[column] == 9)
                    numbers[column] = 0;
                else
                    numbers[column]++;

                numberTexts[column].text = numbers[column].ToString();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (numbers[column] == 0)
                    numbers[column] = 9;
                else
                    numbers[column]--;

                numberTexts[column].text = numbers[column].ToString();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (column == numbers_Column - 1)
                {
                    column = 0;
                }
                else
                    column++;

                Selection();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (column == 0)
                {
                    column = numbers_Column - 1;
                }
                else
                    column--;

                Selection();
            }
        }
    }
}
