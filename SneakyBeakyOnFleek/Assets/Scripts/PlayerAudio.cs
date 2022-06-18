using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
	public List<AudioClip> FootfallAudioClips;

	private AnimationListener myAnimationListener;
	private AudioSource myAudioSource;

	// Start is called before the first frame update
	void Start()
    {
		myAnimationListener = GetComponent<AnimationListener>();
		myAnimationListener.FootfallEvent.AddListener(OnFootFall);
		myAudioSource = GetComponent<AudioSource>();
    }

	private void OnFootFall()
	{
		if(FootfallAudioClips.Count == 0)
		{
			return;
		}

		int rndNumber = Random.Range(0, FootfallAudioClips.Count);
		myAudioSource.PlayOneShot(FootfallAudioClips[rndNumber]);
	}
}
