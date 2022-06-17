using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip openSlow;
    [SerializeField] private AudioClip close;

    public IEnumerator OpenSlow() {
        source.PlayOneShot(openSlow, 0.75f);
        yield return new WaitForSeconds(5);
    }

    public IEnumerator OpenFast() {
        yield return null;
    }

    public IEnumerator Close() {
        source.PlayOneShot(close, 0.75f);
        yield return new WaitForSeconds(3);
    }
}
