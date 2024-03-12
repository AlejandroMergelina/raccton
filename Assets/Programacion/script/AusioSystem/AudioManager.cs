using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource musicSource, fbxSource;

    public static AudioManager Instance;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {

            Destroy(gameObject);

        }
        else
        {

            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
    }

    public void changeMusic(AudioClip clip)
    {

        musicSource.clip = clip;

    }

    public void PlaySound(AudioClip clip)
    {

        fbxSource.PlayOneShot(clip);

    }

    public void StopSound()
    {

        fbxSource.Stop();

    }

    public void SetMusicVolumen(float value)
    {

        musicSource.volume += value;

    }
    public void SetFbxVolumen(float value)
    {

        fbxSource.volume += value;

    }

    public void SetPitch2Fbx(float pitch)
    {

        fbxSource.pitch = pitch;

    }

}
