using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{

    public SpriteRenderer black;
    public SpriteRenderer white;

    private float speed;
    private Color tmp;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f); 

    public void FadeOut(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(_speed));
    }

    private IEnumerator FadeOutCoroutine(float speed)
    {
        tmp = black.color;
        while(tmp.a < 1.0f)
        {
            tmp.a += speed;
            if (tmp.a > 1.0f)
                tmp.a = 1.0f;
            black.color = tmp;
            yield return waitTime;
        }
 //       Debug.Log("test3");
    }

    public void FadeIn(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(_speed));
    }

    private IEnumerator FadeInCoroutine(float speed)
    {
        tmp = black.color;
        while (tmp.a > 0.0f)
        {
            tmp.a -= speed;
            if (tmp.a < 0.0f)
                tmp.a = 0.0f;
            black.color = tmp;
            yield return waitTime;
        }
    }

    public void FlashOut(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashOutCoroutine(_speed));
    }

    private IEnumerator FlashOutCoroutine(float speed)
    {
        tmp = white.color;
        while (tmp.a < 1.0f)
        {
            tmp.a += speed;
            if (tmp.a > 1.0f)
                tmp.a = 1.0f;
            white.color = tmp;
            yield return waitTime;
        }
    }

    public void FlashIn(float _speed = 0.02f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashInCoroutine(_speed));
    }

    private IEnumerator FlashInCoroutine(float speed)
    {
        tmp = white.color;
        while (tmp.a > 0.0f)
        {
            tmp.a -= speed;
            if (tmp.a < 0.0f)
                tmp.a = 0.0f;
            white.color = tmp;
            yield return waitTime;
        }
    }

}
