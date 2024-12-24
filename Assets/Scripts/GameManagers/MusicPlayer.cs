using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
   
    private static MusicPlayer Instance { get; set; }
    private AudioSource _audioSource;
    private float volume = 1f;
    
    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Instance._audioSource = gameObject.GetComponent<AudioSource>();
        Instance._audioSource.enabled = false;
    }
    
    
    
    
    public static  void ChangeVolume()
    {
        if (Instance == null) return;
        
        Instance.ChangeVolumePrivate();
    }

    public static float GetVolume()
    {
        return Instance == null ? 0f : Instance.GetVolumePrivate();
    }


    public static void ActivateTheMusic()
    {
        if (Instance == null) return;
        Instance.ActivateTheMusicPrivate();
    }

    public static void DisableTheMusic()
    {
        if (Instance == null) return;
        Instance.DisableTheMusicPrivate();
    }
    

    private void ActivateTheMusicPrivate()
    {
        Instance._audioSource.enabled = true;
        Instance._audioSource.Play();
    }

    private void DisableTheMusicPrivate()
    {
        Instance._audioSource.enabled = false;
    }
    
    private float GetVolumePrivate()
    {
        return volume;
    }

    private void ChangeVolumePrivate()
    {
        volume += 0.1f;
        if (volume >= 1.1f)
        {
            volume = 0f;
        }
        
        _audioSource.volume = volume;
    }
    
    
    
}
