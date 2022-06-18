using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warden : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private Door door;

    [Space][Header("Slow checkup settings")]
    [SerializeField] private float slowCheckupStepTime = 1;
    [SerializeField] private int slowCheckupStepAmount = 15;

    [Space][Header("Footstep shared settings")]
    [SerializeField] private int walkawayFootstepAmount = 7;
    [SerializeField] private float walkawayStepTime = 1;
    [SerializeField] private AnimationCurve footstepVolumeCurve;
    [SerializeField] private RandomAudioClip footStep;

    private bool isInCheckup = false;

    private void Start() {
        
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            HandleSlowCheckup();
        }
    }

    private void HandleSlowCheckup() {
        if (isInCheckup) { return; }
        StartCoroutine(SlowCheckupRoutine());
    }

    private IEnumerator SlowCheckupRoutine() {
        isInCheckup = true;

        yield return SlowFootstepsBuildup();
        yield return door.OpenSlow();
        yield return CheckForFailure();
        yield return door.Close();
        yield return WalkAway();

        isInCheckup = false;
    }

    // play footsteps slowly, building up in volume according to curve
    private IEnumerator SlowFootstepsBuildup() {
        int footstepCount = 0;
        while (footstepCount < slowCheckupStepAmount) {
            float footstepVolume = footstepVolumeCurve.Evaluate(Mathf.Clamp01(footstepCount / (float)slowCheckupStepAmount));
            PlaySound(footStep, footstepVolume);
            footstepCount++;
            yield return new WaitForSeconds(slowCheckupStepTime);
        }
    }

    private IEnumerator CheckForFailure() {
        yield return new WaitForSeconds(2);
    }

    // play footsteps slowly, building up in volume according to curve
    private IEnumerator WalkAway() {
        int footstepsToGo = walkawayFootstepAmount;
        while (footstepsToGo > 0) {
            float footstepVolume = footstepVolumeCurve.Evaluate(Mathf.Clamp01(footstepsToGo / (float)walkawayFootstepAmount));
            PlaySound(footStep, footstepVolume);
            footstepsToGo--;
            yield return new WaitForSeconds(walkawayStepTime);
        }
    }

    private void PlaySound(AudioClip sound, float volume) {
        source.PlayOneShot(sound, volume);
    }
}
