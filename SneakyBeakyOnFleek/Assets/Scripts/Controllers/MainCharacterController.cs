using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class MainCharacterController : MonoBehaviour, WardenCheckable {

	public Action<Interactable> OnPickedUpInteractable = null;
	public Action<Interactable> OnDroppedInteractable = null;

	public bool IsInBed => isInBed;

	public float MoveSpeed = 3.0f;
	public float JumpStrength = 200f;

	[Space][Header("Interaction")]
	[SerializeField] private float maxInteractionRange = 2;
	[SerializeField] private Transform handTransform = default;
	[SerializeField] private float pickupTweenDuration = 0.1f;
	[SerializeField] private List<InteractionCombo> availableCombos = new List<InteractionCombo>();

	[Space][Header("Trigger Marker Data")]
	[SerializeField] private TriggerMarkerData inBedMarker = default;

	private Rigidbody myRigidbody;
	private Animator myAnimator;

	private Vector2 moveDir;
	private bool isGrounded = false;
	private float isGroundedRaycastLength = 0.02f;
	private float rotationSpeed = 720f;

	private Interactable holdingInteractable = null;
	private bool isInBed = false;

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

    private void OnTriggerEnter(Collider other) {
		TriggerMarker triggerMarker = other.GetComponent<TriggerMarker>();
		if (triggerMarker != null) {
			if (triggerMarker.Data == inBedMarker) {
				isInBed = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
		TriggerMarker triggerMarker = other.GetComponent<TriggerMarker>();
		if (triggerMarker != null) {
			if (triggerMarker.Data == inBedMarker) {
				isInBed = false;
			}
		}
	}

    private void Move()
	{
		Vector3 camForward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
		Vector3 camRight = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;

		myAnimator.SetFloat("MoveSpeed", moveDir.magnitude);
		Vector3 moveDir3D = camForward * moveDir.y + camRight * moveDir.x;
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

		Interactable nearestInteractable = GetNearestInteractable();

		if (holdingInteractable != null) {
			if (nearestInteractable != null) {
				if(TryCombineInteractable(nearestInteractable)) {
					return;
                }
			}
			DropInteractable();
			return;
        } 
		else {
			if (nearestInteractable != null) {
				PickupInteractable(nearestInteractable);
			}
		}
	}

    private bool TryCombineInteractable(Interactable nearestInteractable) {
		InteractionCombo combo = availableCombos.Find(x => { return (x.InteractableOne == holdingInteractable.Data && x.InteractableTwo == nearestInteractable.Data) || (x.InteractableOne == nearestInteractable.Data && x.InteractableTwo == holdingInteractable.Data); });
		if (combo != null) {
			Destroy(nearestInteractable.gameObject);
			Destroy(holdingInteractable.gameObject);
			holdingInteractable = null;
			PickupInteractable(Instantiate(combo.Result.Prefab.gameObject, handTransform.position, handTransform.rotation).GetComponent<Interactable>());
			GameEvents.OnComboExecuted?.Invoke(combo);
			return true;
        }
		return false;
    }

    private Interactable GetNearestInteractable() {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxInteractionRange);
		List<Collider> sortedColliders = new List<Collider>(hitColliders);
		sortedColliders.Sort((a, b) => { return Vector3.Distance(transform.position, a.transform.position).CompareTo(Vector3.Distance(transform.position, b.transform.position)); });
		foreach (var hitCollider in sortedColliders) {
			Interactable interactable = hitCollider.GetComponent<Interactable>();
			if (interactable != null) {
				if (interactable.IsInteractable) {
					return interactable;
				}
			}
		}
		return null;
	}

    private void PickupInteractable(Interactable interactable) {
		if (holdingInteractable != null) {
			DropInteractable();
        }

		holdingInteractable = interactable;
		interactable.transform.SetParent(handTransform, true);
        interactable.transform.DOLocalMove(Vector3.zero, pickupTweenDuration);
        interactable.transform.DOLocalRotate(Vector3.zero, pickupTweenDuration);
        interactable.DisablePhysics();
		interactable.DisableInteraction();

		OnPickedUpInteractable?.Invoke(interactable);
	}

	private void DropInteractable() {
		holdingInteractable.transform.SetParent(null, true);
		holdingInteractable.EnablePhysics();
		holdingInteractable.EnableInteraction();

		OnDroppedInteractable?.Invoke(holdingInteractable);

		holdingInteractable = null;
	}
}
