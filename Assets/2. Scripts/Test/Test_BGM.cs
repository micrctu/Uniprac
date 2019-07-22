using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_BGM : MonoBehaviour
{
    private bool flag = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!flag && collision.transform.name == "Player")
        {
            flag = true;
            BGMManager.instance.FadeInBGM(0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.name == "Player")
        {
            BGMManager.instance.FadeOutBGM();
        }
    }
}
