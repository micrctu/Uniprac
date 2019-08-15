using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Order : MonoBehaviour
{
    public string NPC_name;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            OrderManager.instance.PreLoadMovingObjects();
            StartCoroutine(NPCmoveCoroutine(NPC_name));
        }
    }

    private IEnumerator NPCmoveCoroutine(string name)
    {
        OrderManager.instance.SetPlayerNotMove();
        for (int i = 0; i < 3; i++)
        {
            OrderManager.instance.Move(name, "LEFT");
            yield return new WaitForSeconds(1f);
        }
        OrderManager.instance.SetTransparent(name);
        yield return new WaitForSeconds(2f);
        OrderManager.instance.UnSetTransparent(name);
        OrderManager.instance.SetPlayerMove();
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
