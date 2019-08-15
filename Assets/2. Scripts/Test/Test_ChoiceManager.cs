using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ChoiceManager : MonoBehaviour
{

    public Choice choice;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            OrderManager.instance.SetPlayerNotMove();
            StartCoroutine(ChoiceCoroutine());
        }
    }

    private IEnumerator ChoiceCoroutine()
    {
        ChoiceManager.instance.ShowChoice(choice);

        yield return new WaitUntil(() => !ChoiceManager.instance.choicing);
        OrderManager.instance.SetPlayerMove();
        Debug.Log(ChoiceManager.instance.GetResult());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
