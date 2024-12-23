using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class SoundSO : ScriptableObject
{
    [System.Serializable]
    public class AudioClipInformation
    {
        public List<AudioClip> clips;
        public ClipName clipName;
    }

    public List<AudioClipInformation> clipsWithMatchingName;

}
