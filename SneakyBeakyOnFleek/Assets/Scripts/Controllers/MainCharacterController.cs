using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class MainCharacterController : MonoBehaviour
{
	public float MoveSpeed = 3.0f;
	public float JumpStrength = 200f;

	[Space][Header("Interaction")]
	[SerializeField] private float maxInteractionRange = 2;
	[SerializeField] private Transform handTransform = default;
	[SerializeField] private float pickupTweenDuration = 0.1f;

	private Rigidbody myRigidbody;
	private Animator myAnimator;

	private Vector2 moveDir;
	private bool isGrounded = false;
	private float isGroundedRaycastLength = 0.02f;
	private float rotationSpeed = 720f;

	private Interactable holdingInteractable = null;

    // Start is called before the first frame update
    void Start()
    {
		myRigidbody = GetComponent<Rigidbody>();
		myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		Move();
		CheckIfGrounded();
    }

	private void Move()
	{
		myAnimator.SetFloat("MoveSpeed", moveDir.magnitude);
		Vector3 moveDir3D = new Vector3(moveDir.x, 0f, moveDir.y);
		transform.position +=  moveDir3D * Time.deltaTime * MoveSpeed;
		if(moveDir.sqrMagnitude > 0.1f)
		{
			Quaternion targetRotation = Quaternion.LookRotation(moveDir3D);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
		}
	}

	private void CheckIfGrounded()
	{
		if(Physics.Raycast(transform.position + Vector3.up * 0.01f, -Vector3.up, isGroundedRaycastLength))
		{
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}
	}

	public void TryJump(InputAction.CallbackContext callback)
	{
		if(!isGrounded)
		{
			return;
		}

		myAnimator.SetTrigger("Jump");
		myRigidbody.AddForce(Vector3.up * JumpStrength);
	}

	public void SetMoveInput(InputAction.CallbackContext callback)
	{
		moveDir = callback.ReadValue<Vector2>();
	}

	public void TryInteract(InputAction.CallbackContext callback) {
		if (callback.phase != InputActionPhase.Started) { return; }

		if (holdingInteractable != null) {
			DropInteractable();
			return;
        }

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxInteractionRange);
		List<Collider> sortedColliders = new List<Collider>(hitColliders);
		sortedColliders.Sort((a, b) => { return Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position)); });
		foreach (var hitCollider in sortedColliders) {
			Interactable interactable = hitCollider.GetComponent<Interactable>();
			if (interactable != null) {
				if (interactable.CanPickUp) {
					TryPickupInteractable(interactable);
					break;
				}
            }
		}
	}

    private void TryPickupInteractable(Interactable interactable) {
		if (holdingInteractable != null) {
			return;
        }

		holdingInteractable = interactable;
		interactable.transform.SetParent(handTransform, true);
        interactable.transform.DOLocalMove(Vector3.zero, pickupTweenDuration);
        interactable.transform.DOLocalRotate(Vector3.zero, pickupTweenDuration);
        interactable.DisablePhysics();
	}

	private void DropInteractable() {
		holdingInteractable.transform.SetParent(null, true);
		holdingInteractable.EnablePhysics();
		holdingInteractable = null;
	}
}
