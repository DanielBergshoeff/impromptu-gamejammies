using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailureScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup failureOverlay = default;
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private AudioClip failureSound = default;
    [SerializeField] private Button retryButton = default;

    private void Awake() {
        failureOverlay.gameObject.SetActive(false);
        failureOverlay.alpha = 0;
        GameEvents.OnWardenSpottedPlayer += HandlePlayerSpotted;
        retryButton.onClick.AddListener(HandleRetryButtonClick);
    }

    private void HandlePlayerSpotted() {
        failureOverlay.gameObject.SetActive(true);
        failureOverlay.DOFade(1, 0.5f);
        if(failureSound != null) {
            audioSource.PlayOneShot(failureSound);
        }
    }

    private void HandleRetryButtonClick() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
