using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ok_or_Cancel : MonoBehaviour
{
    public GameObject go;

    public Text OK_Text;
    public GameObject OK_Panel;

    public Text Cancel_Text;
    public GameObject Cancel_Panel;

    public bool activated;
    private bool result;

    public void ShowOOC(string _Ok, string _Cancel)
    {
        OK_Text.text = _Ok;
        Cancel_Text.text = _Cancel;

        OK_Panel.SetActive(false);
        Cancel_Panel.SetActive(true);

        result = true;
        go.SetActive(true);
        activated = true;

    }

    private void ExitOOC()
    {        
        go.SetActive(false);
        activated = false;
    }

    public bool GetResult()
    {
        return result;
    }

    private void Select()
    {
        result = !result;

        if(!result)
        {
            OK_Panel.SetActive(true);
            Cancel_Panel.SetActive(false);
        }
        else
        {
            OK_Panel.SetActive(false);
            Cancel_Panel.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        go.SetActive(false);
        activated = false;
        result = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(activated)
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                Select();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Select();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                ExitOOC();
            }
            else if (Input.GetKeyDown(KeyCode.X)) //즉,취소 선택과 같은 효과
            {
                result = false;
                ExitOOC();
            }
        }
    }
}
