using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool IsInteractable => isInteractable;
    public InteractableData Data = default;

    [SerializeField] private bool isInteractable = true;
    [SerializeField] private Rigidbody body = default;
    [SerializeField] private new Collider collider = default;

    public void EnablePhysics() {
        body.isKinematic = false;
        collider.enabled = true;
    }

    public void DisableInteraction() {
        isInteractable = false;
    }

    public void DisablePhysics() {
        body.isKinematic = true;
        collider.enabled = false;
    }

    public void EnableInteraction() {
        isInteractable = true;
    }
}
