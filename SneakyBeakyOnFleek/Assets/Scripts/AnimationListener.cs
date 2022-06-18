using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationListener : MonoBehaviour
{
	[HideInInspector] public UnityEvent FootfallEvent;


	private void Awake()
	{
		FootfallEvent = new UnityEvent();
	}

	public void OnFootFall()
	{
		FootfallEvent.Invoke();
	}
}
