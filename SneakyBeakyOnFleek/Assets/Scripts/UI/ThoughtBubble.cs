using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubble : MonoBehaviour {
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip appearSound;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private InteractionCombo testCombo;
    [SerializeField] private Image imageContainer;
    [SerializeField] private float timePerImage = 0.5f;
    [SerializeField] private int amountOfCycles = 3;

    private List<Sprite> currentSequence = null;
    private int currentIndex = 0;
    private float nextImageTime = 0;
    private int currentCycle = 1;

    private void Awake() {
        canvasGroup.alpha = 0;
    }

    void Update() {
        UpdateSequence();
    }

    public void ShowSequenceForCombo(InteractionCombo combo) {
        currentSequence = combo.HintImageSequence;
        nextImageTime = Time.time + timePerImage;
        currentIndex = 0;
        currentCycle = 1;
        UpdateImage();
        canvasGroup.alpha = 1;
        audioSource.PlayOneShot(appearSound);
    }

    [ContextMenu("Test bubble")]
    private void TestBubble() {
        ShowSequenceForCombo(testCombo);
    }

    private void UpdateSequence() {
        if (currentSequence == null) { return; }
        if (Time.time >= nextImageTime) {
            GoToNextStepInSequence();
        }
    }

    private void GoToNextStepInSequence() {
        currentIndex++;
        if (currentSequence.Count <= currentIndex) {
            currentIndex = 0;
            currentCycle++;
            if (currentCycle > amountOfCycles) {
                canvasGroup.alpha = 0;
                currentSequence = null;
            }
        }
        UpdateImage();
        nextImageTime = Time.time + timePerImage;
    }

    private void UpdateImage() {
        if (currentSequence == null || currentSequence.Count == 0) { return; }
        if (currentSequence.Count <= currentIndex) { return; }

        imageContainer.sprite = currentSequence[currentIndex];
        audioSource.PlayOneShot(changeSound);
    }
}
