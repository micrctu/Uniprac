using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_NumberManager : MonoBehaviour
{
    public string correctNumber;
    [Range(1, 4)]
    public int numbers_Column;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            OrderManager.instance.SetPlayerNotMove();
            StartCoroutine(NumberCoroutine());
        }
    }

    private IEnumerator NumberCoroutine()
    {
        NumberManager.instance.ShowNumber(correctNumber, numbers_Column);

        yield return new WaitUntil(() => !NumberManager.instance.choicing);
        OrderManager.instance.SetPlayerMove();
        Debug.Log(NumberManager.instance.GetResult());
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
