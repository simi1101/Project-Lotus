using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool lerpCrouch = false;
    private bool crouching = false;
    private bool sprinting = false;
    public float speed = 2f;
    public float gravity = -9.8f;
    public float jumpHeight = 0.5f;
    public float crouchTimer = 1;
    //attempting to add animation state controllers here
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
       
    }
    //This will receive the inputs for our InputManager.cs and apply them to our character controller
    public void ProcessMove(Vector3 input)
    {
        Vector3 moveDirection;
        moveDirection.x = input.x;
        moveDirection.y = input.y;
        moveDirection.z = input.z;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        if (input.x < 0 || input.x > 0 || input.z < 0 || input.z > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
            
        controller.Move(playerVelocity * Time.deltaTime);
        Debug.Log(playerVelocity.y);

    }
    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }
    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
        {
            speed = 4;
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
            
        else
        {
            speed = 2;
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
        }
            
    }
}
