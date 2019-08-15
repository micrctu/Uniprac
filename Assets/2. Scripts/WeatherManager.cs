using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager instance;

    public ParticleSystem theParticle;
    public string rainSound;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            //           DontDestroyOnLoad(this.gameObject); 본 게임오브젝트는 캔버스의 자식 오브젝트로, 의미 없음
        }
        else
            Destroy(this.gameObject);
        #endregion Singleton

    }

    public void On_WeatherEffect()
    {
        AudioManager.instance.SetVolume(rainSound, 0.3f);
        AudioManager.instance.Play(rainSound);

        StartCoroutine(WeatherEffectCoroutine());
 //       Debug.Log("test");
 //       theParticle.Play();
    }

    private IEnumerator WeatherEffectCoroutine()
    {
        theParticle.maxParticles = 1;
        theParticle.Play();

        for (int i = 1; i < 20; i++)
        {
            if(i < 10)
               theParticle.maxParticles = i;
            else if(i < 15)
              theParticle.maxParticles = i * 2;
            else
              theParticle.maxParticles = i * 4;

            yield return new WaitForSeconds(0.5f); 
        }
    }

    public void Off_WeatherEffect()
    {
        AudioManager.instance.Stop(rainSound);
        theParticle.Stop();
    }
}
