using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Choice
{
    public string question;

    [Tooltip("선택지는 총 4개까지 가능")]
    public string[] answers;
}


public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager instance;

    private GameObject choiceManager;
    private Animator theAnim;

    public Text question_Field;
    public Text[] Answer_Field;

    public GameObject question_Panel;
    public GameObject[] Answer_Panel;

    private string question;
    private List<string> answers;

    private int count; // 총 선택지의 개수
    private int result; // 최종 선택된 선택지(0~3)

    public bool choicing; // 선택지 처리 중임을 나타내는 플래그
    private bool keyInput; // 선택지 출력 마친후에 키 인풋 처리를 위함

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

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
        choiceManager = this.gameObject;
        theAnim = this.GetComponent<Animator>();
        
        choicing = false;
        keyInput = false;

        question = "";
        answers = new List<string>();

        question_Field.text = "";
        question_Panel.SetActive(false);

        for (int i = 0; i < Answer_Panel.Length; i++)
        {
            Answer_Field[i].text = "";
            Answer_Panel[i].SetActive(false);
        }
    }

    public void ShowChoice(Choice _choice)
    {
        choicing = true;
       
        count = _choice.answers.Length;
        result = 0;
        question = _choice.question;
        question_Panel.SetActive(true);

        for (int i = 0; i < count; i++) //선택지 개수에 해당되는 만큼만 패널 활성화하여야 함
        {
            answers.Add(_choice.answers[i]);
            Answer_Panel[i].SetActive(true);
        }
        choiceManager.SetActive(true);

        theAnim.SetBool("Appear", true);
        StartCoroutine(ChoiceCoroutine());
        Selection();
    }

    private IEnumerator ChoiceCoroutine()
    {
        StartCoroutine(TypingQuestion());

        yield return new WaitForSeconds(0.1f);

        switch (count)
        {
            case 1:
                StartCoroutine(TypingAnswer_0());
                break;
            case 2:
                StartCoroutine(TypingAnswer_0());
                StartCoroutine(TypingAnswer_1());
                break;
            case 3:
                StartCoroutine(TypingAnswer_0());
                StartCoroutine(TypingAnswer_1());
                StartCoroutine(TypingAnswer_2());
                break;
            case 4:
                StartCoroutine(TypingAnswer_0());
                StartCoroutine(TypingAnswer_1());
                StartCoroutine(TypingAnswer_2());
                StartCoroutine(TypingAnswer_3());
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(2f);
        keyInput = true;
    }

    private IEnumerator TypingQuestion()
    {
        yield return new WaitForSeconds(0.3f);

        question_Field.text = "";
        for (int i = 0; i < question.Length; i++)
        {
            question_Field.text += question[i];
            yield return waitTime;
        }

    }

    private IEnumerator TypingAnswer_0()
    {
        yield return new WaitForSeconds(0.5f);

        Answer_Field[0].text = "";
        for (int i = 0; i < answers[0].Length; i++)
        {
            Answer_Field[0].text += answers[0][i];
            yield return waitTime;
        }
    }

    private IEnumerator TypingAnswer_1()
    {
        yield return new WaitForSeconds(0.7f);

        Answer_Field[1].text = "";
        for (int i = 0; i < answers[1].Length; i++)
        {
            Answer_Field[1].text += answers[1][i];
            yield return waitTime;
        }
    }

    private IEnumerator TypingAnswer_2()
    {
        yield return new WaitForSeconds(0.9f);

        Answer_Field[2].text = "";
        for (int i = 0; i < answers[2].Length; i++)
        {
            Answer_Field[2].text += answers[2][i];
            yield return waitTime;
        }
    }

    private IEnumerator TypingAnswer_3()
    {
        yield return new WaitForSeconds(1.1f);

        Answer_Field[3].text = "";
        for (int i = 0; i < answers[3].Length; i++)
        {
            Answer_Field[3].text += answers[3][i];
            yield return waitTime;
        }
    }

    private void Selection()  //현재 선택된 answer panel 강조
    {
        Color color = Answer_Panel[0].GetComponent<Image>().color;
        color.a = 0.6f;
        for (int i = 0; i < count; i++)
        {
            Answer_Panel[i].GetComponent<Image>().color = color;
        }
        color.a = 1.0f;
        Answer_Panel[result].GetComponent<Image>().color = color;
    }

    private void ExitChoice() //선택지 끝내고 초기화
    {
        keyInput = false;
        question = "";
        answers.Clear();

        theAnim.SetBool("Appear", false);

        question_Field.text = "";
        question_Panel.SetActive(false);

        for (int i = 0; i < Answer_Panel.Length; i++) 
        {
            Answer_Field[i].text = "";
            Answer_Panel[i].SetActive(false);
        }
        choiceManager.SetActive(false);
        choicing = false;
    }

    public int GetResult() //결과값 반환
    {
        return result;
    }
       

    // Update is called once per frame
    void Update()
    {
        if(keyInput)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ExitChoice();
            }

            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (result == count - 1)
                    result = 0;
                else
                    result++;

                Selection();
            }

            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (result == 0)
                    result = count - 1;
                else
                    result--;
                Selection();
            }
 
        }
    }
}
