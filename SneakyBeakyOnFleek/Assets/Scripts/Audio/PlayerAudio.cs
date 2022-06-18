using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
	public RandomAudioClip FootfallAudioClips;
	public RandomAudioClip GrabClips;

	private MainCharacterController mainCharacterController;
	private AnimationListener myAnimationListener;
	private AudioSource myAudioSource;
    private float lastGrabSoundTime;

    // Start is called before the first frame update
    void Awake()
    {
		mainCharacterController = GetComponent<MainCharacterController>();
		myAnimationListener = GetComponent<AnimationListener>();
		myAnimationListener.FootfallEvent.AddListener(OnFootFall);
		myAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
		GameEvents.OnComboExecuted += HandleComboExecuted;
		mainCharacterController.OnPickedUpInteractable += HandlePickedUpInteractable;
		mainCharacterController.OnDroppedInteractable += HandleDroppedInteractable;
	}

    private void OnDisable() {
		GameEvents.OnComboExecuted -= HandleComboExecuted;
		mainCharacterController.OnPickedUpInteractable -= HandlePickedUpInteractable;
		mainCharacterController.OnDroppedInteractable += HandleDroppedInteractable;
	}

    private void OnFootFall()
	{
		if(FootfallAudioClips.HasClips) {
			myAudioSource.PlayOneShot(FootfallAudioClips);
		}
	}

	private void HandleComboExecuted(InteractionCombo combo) {
		if (combo.CombineSound != null) {
			myAudioSource.PlayOneShot(combo.CombineSound);
		}
	}

	private void HandlePickedUpInteractable(Interactable interactable) {
		PlayGrabSound();
		if (interactable.Data.GrabSound.HasClips) {
			myAudioSource.PlayOneShot(interactable.Data.GrabSound);
		}
	}

	private void HandleDroppedInteractable(Interactable interactable) {
		PlayGrabSound();
		if (interactable.Data.GrabSound.HasClips) {
			myAudioSource.PlayOneShot(interactable.Data.GrabSound);
		}
	}

    private void PlayGrabSound() {
		if (Time.time - lastGrabSoundTime < 0.2f) { return; }
		myAudioSource.PlayOneShot(GrabClips);
		lastGrabSoundTime = Time.time;
	}
}
