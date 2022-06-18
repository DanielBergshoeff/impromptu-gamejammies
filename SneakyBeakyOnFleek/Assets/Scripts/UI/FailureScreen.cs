using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailureScreen : MonoBehaviour
{
    [SerializeField] private GameObject failureOverlay = default;
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip failureSound = default;

    private void Awake() {
        failureOverlay.SetActive(false);
        GameEvents.OnPlayerSpotted += HandlePlayerSpotted;
    }

    private void HandlePlayerSpotted() {
        failureOverlay.SetActive(true);
        if(failureSound != null) {
            audioSource.PlayOneShot(failureSound);
        }
    }
}
