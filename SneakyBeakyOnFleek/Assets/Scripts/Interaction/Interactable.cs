using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool CanPickUp => canPickUp;

    [SerializeField] private bool canPickUp = true;
    [SerializeField] private Rigidbody body = default;
    [SerializeField] private new Collider collider = default;

    public void EnablePhysics() {
        body.isKinematic = false;
        collider.enabled = true;
    }

    public void DisablePhysics() {
        body.isKinematic = true;
        collider.enabled = false;
    }
}
