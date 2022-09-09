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
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;

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
}