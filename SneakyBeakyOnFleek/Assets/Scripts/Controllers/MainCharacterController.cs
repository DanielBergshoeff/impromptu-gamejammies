using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class MainCharacterController : MonoBehaviour
{
	public float MoveSpeed = 3.0f;
	public float JumpStrength = 200f;

	private Rigidbody myRigidbody;
	private Animator myAnimator;

	private Vector2 moveDir;
	private bool isGrounded = false;
	private float isGroundedRaycastLength = 0.02f;
	private float rotationSpeed = 720f;

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
}
