using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Movement Values")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] [Range(0, 1)]
    private float verticalSpeedMultiplier;
    [SerializeField] [Range(0, 1)]
    private float horizontalSpeedMultiplier;

    [Header("Jump Values")]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float fallSpeed;
    private bool isJumping;

    [Header("Dash Values")]
    [SerializeField] private float dashSpeed;

    [Header("Roll Values")]
    [SerializeField] private float rollTime;
    [SerializeField] private float rollSpeed;
    private bool isRolling;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Walk(Vector2 movement) {
        rb.MovePosition(rb.position + 
                new Vector2(movement.x * horizontalSpeedMultiplier, movement.y * verticalSpeedMultiplier) * walkSpeed * Time.deltaTime);
    }

    public void Run(Vector2 movement) {
        rb.MovePosition(rb.position + 
                new Vector2(movement.x * horizontalSpeedMultiplier, movement.y * verticalSpeedMultiplier) * runSpeed * Time.deltaTime);
    }

    public void Sprint(Vector2 movement) {
        rb.MovePosition(rb.position + 
                new Vector2(movement.x * horizontalSpeedMultiplier, movement.y * verticalSpeedMultiplier) * sprintSpeed * Time.deltaTime);
    }

    public void Roll(Vector2 movement) {
        if (movement.x == 0 && movement.y == 0)
            rb.MovePosition(rb.position +
                new Vector2((movement.x + 1) * horizontalSpeedMultiplier, 0) * dashSpeed * Time.deltaTime);
        else {              
            rb.MovePosition(rb.position +
                new Vector2(movement.x * horizontalSpeedMultiplier, movement.y * verticalSpeedMultiplier) * dashSpeed * Time.deltaTime);
        }
    }
}
