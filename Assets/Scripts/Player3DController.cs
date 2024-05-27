using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3DController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 97.8f;
    [SerializeField] float runSpeed = 200f;
    [SerializeField] float jumpSpeed = 1.9f;
    [SerializeField] float rotateSpeed = 200f;
    [SerializeField] LayerMask groundLayer;
    public Animator animator;
    CharacterController characterController;
    public GameObject healingVFX;

    float moveAmount;
    Vector3 moveDirection;
    float verticalVelocity;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGroundStatus();

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        float speed = (Input.GetKey(KeyCode.LeftShift) && moveAmount > 0) ? runSpeed : walkSpeed;

        Vector3 movement = moveDirection * speed * Time.deltaTime;

        if (isGrounded)
        {
            verticalVelocity = -1f; // Ensures the character stays grounded
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpSpeed;
            }
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        movement.y = verticalVelocity * Time.deltaTime;
        characterController.Move(movement);

        // Adjust moveAmount based on speed
        float adjustedMoveAmount = moveAmount * (speed / walkSpeed);
        animator.SetFloat("Speed", adjustedMoveAmount);

        if (moveAmount != 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }
        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("Death");
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if (healingVFX != null)
        {
            healingVFX.SetActive(false);
        }
        if (healingVFX != null)
        {
            healingVFX.SetActive(true);
        }
    }
}
