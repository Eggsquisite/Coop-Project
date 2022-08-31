using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private enum PlayerState {
        Idle,
        Moving,
        Attacking,
        Stunned,
        Dying,
    }

    private enum PlayerType {
        Swordmaster,
        Gunslinger
    }
    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Character Specific Values")]
    [SerializeField] private PlayerType type;
    private PlayerState state = PlayerState.Idle;

    [Header("Movement Values")]
    [SerializeField] private float playerSpeed;
    [SerializeField] [Range(0, 1)]
    private float verticalSpeedMultiplier;
    [SerializeField] [Range(0, 1)]
    private float horizontalSpeedMultiplier;
    private Vector2 movement;

    [Header("Jump Values")]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float fallSpeed;
    private bool isJumping;

    [Header("Dash Values")]
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        movement = context.ReadValue<Vector2>().normalized;
    }

    public void OnJump(InputAction.CallbackContext context) {
        isJumping = context.action.triggered;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If player is dead, stop all actions (excluding death)
        if (state == PlayerState.Dying)
            return;

        // If any player type is stunned, prevent movement
        if (state != PlayerState.Stunned)
            Move();
    }

    private void Move() {
        // Swordmaster cannot move while attacking
        if (type == PlayerType.Swordmaster && state == PlayerState.Attacking)
            return;

        rb.MovePosition(rb.position + 
                new Vector2(movement.x * horizontalSpeedMultiplier, movement.y * verticalSpeedMultiplier) * playerSpeed * Time.deltaTime);
    }
}
