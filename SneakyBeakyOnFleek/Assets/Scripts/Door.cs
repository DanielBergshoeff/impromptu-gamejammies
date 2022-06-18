using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip openSlow;
    [SerializeField] private AudioClip close;

    [Space][Header("Sound")]
    [SerializeField] private Transform hinge = default;
    [SerializeField] private float degreesOpenSlow = 70;

    public IEnumerator OpenSlow() {
        source.PlayOneShot(openSlow, 0.5f);
        hinge.DOLocalRotate(new Vector3(0, -degreesOpenSlow, 0), openSlow.length).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(openSlow.length);
    }

    public IEnumerator OpenFast() {
        yield return null;
    }

    public IEnumerator Close() {
        source.PlayOneShot(close, 0.5f);
        hinge.DOLocalRotate(new Vector3(0, 0, 0), close.length - 0.7f).SetEase(Ease.InQuad);
        yield return new WaitForSeconds(close.length);
    }

    [ContextMenu("TestOpenSlow")]
    private void TestOpenSlow() {
        StartCoroutine(TestOpenSlowRoutine());
    }

    private IEnumerator TestOpenSlowRoutine() {
        yield return OpenSlow();
        yield return new WaitForSeconds(1);
        yield return Close();
    }
}
