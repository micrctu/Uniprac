using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    public float volume;
    public bool loop;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
    }

    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void SetVolume(float _volume)
    {
        volume = _volume;
        source.volume = volume;
    }

    public void SetLoop(bool _loop)
    {
        loop = _loop;
        source.loop = loop;
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        #endregion Singleton

    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < sounds.Length; i++) //각각의 sound 클래스마다 AudioSource를 지정해서 해당 AudioSource를 통해 사운드 재생
        {
            GameObject soundObject = new GameObject(" " + i + ". 사운드 이름 = " + sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }
    }

    public void Play(string soundName)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if(sounds[i].name == soundName)
            {
                sounds[i].Play();
                break;
            }
        }
    }

    public void Stop(string soundName)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == soundName)
            {
                sounds[i].Stop();
                break;
            }
        }
    }

    public void SetVolume(string soundName, float vol)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == soundName)
            {
                sounds[i].SetVolume(vol);
                break;
            }
        }
    }

    public void SetLoop(string soundName, bool loop)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == soundName)
            {
                sounds[i].SetLoop(loop);
                break;
            }
        }
    }


}
