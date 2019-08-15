using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Weather : MonoBehaviour
{
    private bool flag = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(flag && collision.gameObject.tag == "Player")
        {
            flag = false;
            WeatherManager.instance.On_WeatherEffect();
        }
    }
}
