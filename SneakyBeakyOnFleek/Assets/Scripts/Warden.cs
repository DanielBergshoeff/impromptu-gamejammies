using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warden : SerializedMonoBehaviour {

    [Header("References")]
    [SerializeField] private WardenCheckable checkable = default;
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private Door door = default;

    [Space][Header("Slow checkup settings")]
    [SerializeField] private float slowCheckupStepTime = 1;
    [SerializeField] private int slowCheckupStepAmount = 15;
    [SerializeField] private float slowCheckupWaitTime = 2;

    [Space][Header("Footstep shared settings")]
    [SerializeField] private int walkawayFootstepAmount = 7;
    [SerializeField] private float walkawayStepTime = 1;
    [SerializeField] private AnimationCurve footstepVolumeCurve = default;
    [SerializeField] private RandomAudioClip footStepSound = default;

    [Space] [Header("Other settings")]
    [SerializeField] private AudioClip failureReactionSound = default;

    private bool isInCheckup = false;
    private bool sawCheckableOutsideOfBed = false;

    private void Start() {
        
    }

    private void Update() {
        //if (Input.GetKeyDown(KeyCode.Alpha1)) {
        //    HandleSlowCheckup();
        //}
    }

    private void HandleSlowCheckup() {
        if (isInCheckup) { return; }
        StartCoroutine(SlowCheckupRoutine());
    }

    private IEnumerator SlowCheckupRoutine() {
        isInCheckup = true;

        yield return SlowFootstepsBuildup();
        yield return door.OpenSlow();
        yield return CheckForFailure(slowCheckupWaitTime);
        if (sawCheckableOutsideOfBed) {
            yield return FailureReaction();
            GameEvents.OnPlayerSpotted?.Invoke();
            yield break;
        }
        yield return door.Close();
        yield return WalkAway();

        isInCheckup = false;
    }

    private IEnumerator FailureReaction() {
        PlaySound(failureReactionSound, 1);
        yield return new WaitForSeconds(failureReactionSound.length);
    }

    // play footsteps slowly, building up in volume according to curve
    private IEnumerator SlowFootstepsBuildup() {
        int footstepCount = 0;
        while (footstepCount < slowCheckupStepAmount) {
            float footstepVolume = footstepVolumeCurve.Evaluate(Mathf.Clamp01(footstepCount / (float)slowCheckupStepAmount));
            PlaySound(footStepSound, footstepVolume);
            footstepCount++;
            yield return new WaitForSeconds(slowCheckupStepTime);
        }
    }

    private IEnumerator CheckForFailure(float checkTime) {
        float endTime = Time.time + checkTime;
        while (Time.time < endTime) {
            if (checkable.IsInBed == false) {
                sawCheckableOutsideOfBed = true;
                yield break;
            }
            yield return null;
        }
    }

    // play footsteps slowly, building up in volume according to curve
    private IEnumerator WalkAway() {
        int footstepsToGo = walkawayFootstepAmount;
        while (footstepsToGo > 0) {
            float footstepVolume = footstepVolumeCurve.Evaluate(Mathf.Clamp01(footstepsToGo / (float)walkawayFootstepAmount));
            PlaySound(footStepSound, footstepVolume);
            footstepsToGo--;
            yield return new WaitForSeconds(walkawayStepTime);
        }
    }

    private void PlaySound(AudioClip sound, float volume) {
        audioSource.PlayOneShot(sound, volume);
    }
}
