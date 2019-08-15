using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_LightMode : MonoBehaviour
{
    private bool flag = true;
    public LightController theLight;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (flag && collision.gameObject.tag == "Player")
        {
            flag = false;
            theLight.On_LightMode();
            StartCoroutine(WaitTime());
        }
    }

    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(10f);
        theLight.Off_LightMode();
    }
}
