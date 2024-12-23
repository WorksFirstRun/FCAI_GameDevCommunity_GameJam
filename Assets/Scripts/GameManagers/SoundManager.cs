using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public enum ClipName
{
    Sword,
    
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


    public static void ChangeVolume(float newVolume)
    {
        if (Instance == null) return;
        Instance.ChangeV(newVolume);
    }

    private void ChangeV(float newVolume)
    {
        soundVolume = newVolume;
    }
    
}
