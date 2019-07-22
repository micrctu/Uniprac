using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    private AudioSource audioSource;
    public AudioClip[] clips;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        #endregion Singleton
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void FadeOutBGM()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
       while(audioSource.volume > 0f)
        {
            audioSource.volume = audioSource.volume - 0.01f;
            yield return waitTime;
        }
        this.Stop();
    }

    public void FadeInBGM(int _trackNumber)
    {
        StopAllCoroutines();
        this.Play(_trackNumber, 0f);
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        while (audioSource.volume < 1f)
        {
            audioSource.volume = audioSource.volume + 0.01f;
            yield return waitTime;
        }
    }

    public void Play(int _trackNumber, float vol = 1.0f)
    {
        this.SetVolume(vol);
        audioSource.clip = clips[_trackNumber];
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Unpause()
    {
        audioSource.UnPause();
    }

    public void SetVolume(float vol)
    {
        if(vol >= 0 && vol <= 1)
            audioSource.volume = vol;
    }
    
}
