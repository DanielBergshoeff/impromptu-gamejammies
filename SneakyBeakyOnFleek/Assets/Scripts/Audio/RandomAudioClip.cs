using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomAudioClip
{
    public bool HasClips {
        get {
            return clips != null && clips.Count > 0;
        }
    }
    [SerializeField] private List<AudioClip> clips = new List<AudioClip>();


    public static implicit operator AudioClip(RandomAudioClip randomAudioClip) {
        if (randomAudioClip.clips.Count == 0) {
            return null;
        }
        return randomAudioClip.clips[Random.Range(0, randomAudioClip.clips.Count)];
    }
}
