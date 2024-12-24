using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public enum ClipName
{
    Sword,
    CantCast,
    Dash,
    DemonVanish,
    FireExplosion,
    FireBallRelease,
    Lose,
    Win,
    WaterSpellCast,
    WaterSpellExplosion,
    ZoomInCanCast,
    UIButton
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager Instance { get; set; }
    [SerializeField] private SoundSO _soundSo;
    private Dictionary<ClipName, List<AudioClip>> audioClips;
    private float soundVolume = 1f;
    
    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        audioClips = new Dictionary<ClipName, List<AudioClip>>();
        foreach (SoundSO.AudioClipInformation audioClipInformation in _soundSo.clipsWithMatchingName)
        {
            audioClips.Add(audioClipInformation.clipName,audioClipInformation.clips);
        }
    }


    // make a static function here that check for the instance that if it does exist or not and play sound

    public static void PlaySound(ClipName clipName, Vector2 playPosition)
    {
        if (Instance == null) return;
        Instance.Play(clipName, playPosition);
    }
    
    
    private void Play(ClipName clipName,Vector2 playPosition)
    {
        AudioSource.PlayClipAtPoint(audioClips[clipName][Random.Range(0,audioClips[clipName].Count)],playPosition,soundVolume);
    }


    public static void ChangeVolume()
    {
        if (Instance == null) return;
        Instance.ChangeV();
    }

    public static float GetVolume()
    {
        return Instance == null ? 0f : Instance.GetVolumePrivate();
    }
    
    private void ChangeV()
    {
        soundVolume += 0.1f;
        if (soundVolume >= 1.1f)
        {
            soundVolume = 0f;
        }
    }
    
    private float GetVolumePrivate()
    {
        return soundVolume;
    } 
    
}
