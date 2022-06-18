using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTone : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceTone = default;
    [SerializeField] private AudioSource audioSourcePassby = default;
    [SerializeField] private AudioClip roomtoneLoopSound = default;
    [SerializeField] private RandomAudioClip passbySound = default;
    [SerializeField] private int minPassbyWaitTime = 10;
    [SerializeField] private int maxPassbyWaitTime = 30;

    private float nextPassbyTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSourceTone.clip = roomtoneLoopSound;
        audioSourceTone.Play();
        SetNextPassbyTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextPassbyTime && audioSourcePassby.isPlaying == false) {
            audioSourcePassby.clip = passbySound;
            audioSourcePassby.pitch = UnityEngine.Random.Range(0.6f, 1.2f);
            audioSourcePassby.volume = UnityEngine.Random.Range(0.5f, 1);
            audioSourcePassby.Play();
            SetNextPassbyTime();
        }
    }

    private void OnDestroy() {
        audioSourceTone.Stop();
    }

    private void SetNextPassbyTime() {
        nextPassbyTime = Time.time + UnityEngine.Random.Range(minPassbyWaitTime, maxPassbyWaitTime);
    }
}
