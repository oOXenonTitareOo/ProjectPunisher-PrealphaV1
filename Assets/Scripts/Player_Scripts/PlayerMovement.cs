using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 30f;
    public float jumpForce = 5f;
    public float groundDistance = 0.4f;
    private float currentSpeed;
    public Transform groundCheck;
    public LayerMask groundMask;
    public LayerMask droppedItemMask;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isSprinting = false;
    public AudioClip footStepSFX;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(PlayFootStepSFX());
    }

    void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void OnJump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask | droppedItemMask);
    }

    void OnMovement(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnSprint()
    {
        isSprinting = true;
    }

    void OnSprintRelease()
    {
        isSprinting = false;
    }

    void MovePlayer()
    {
        if(isSprinting)
        {
            currentSpeed = sprintSpeed;
        }
        else if(!isSprinting)
        {
            currentSpeed = moveSpeed;
        }

        Vector3 direction = transform.right * moveInput.x + transform.forward * moveInput.y;
        direction.Normalize();
        rb.linearVelocity = new Vector3(direction.x * currentSpeed, rb.linearVelocity.y, direction.z * currentSpeed);
    }

    IEnumerator PlayFootStepSFX()
    {
        while (true)
        {
            if(rb.linearVelocity.magnitude > 0.1f && isGrounded)
            {
                AudioManager.Instance.PlaySFX(footStepSFX, transform.position);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}