using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState {
        Idle,
        Walking,
        Jumping,
        Dashing,
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
    private PlayerCombat combat;
    private PlayerAnimations animations;

    [Header("Character Specific Values")]
    [SerializeField] private PlayerType type;
    private PlayerState state = PlayerState.Idle;

    // Gunslinger attack is independent of movement
    private bool isFiring, isAttacking;

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
        combat = GetComponent<PlayerCombat>();
        animations = GetComponent<PlayerAnimations>();
    }

    public void OnMove(InputAction.CallbackContext context) {
        movement = context.ReadValue<Vector2>().normalized;
    }

    public void OnJump(InputAction.CallbackContext context) {
        isJumping = context.action.triggered;
    }

    void Update() {
        UpdatePlayerState();
        UpdatePlayerAnimation();
    }

    void FixedUpdate()
    {
        UpdatePlayerMovement();

        // // If player is dead, stop all actions (excluding death)
        // if (state == PlayerState.Dying)
        //     return;

        // // If any player type is stunned, prevent movement
        // if (state != PlayerState.Stunned) {
        //     Move();
        // }
    }

    private void UpdatePlayerState() {
        switch(type) {
            case PlayerType.Swordmaster:
                if (combat.GetIsAttack()) 
                    SetState(PlayerState.Attacking);
                // If player input is > 0, player is attempting to move
                else if (movement.x != 0 || movement.y != 0)
                    SetState(PlayerState.Walking);
                else if (movement.x == 0 && movement.y == 0)
                    SetState(PlayerState.Idle);
                

                break;

            case PlayerType.Gunslinger:
                // Gunslinger attack is independent from movement
                if (combat.GetIsAttack() && !isFiring)
                    isFiring = true;
                else if (!combat.GetIsAttack() && isFiring)
                    isFiring = false;

                // If player input is > 0, player is attempting to move
                if (movement.x != 0 || movement.y != 0)
                    SetState(PlayerState.Walking);
                else if (movement.x == 0 && movement.y == 0)
                    SetState(PlayerState.Idle);

                break;
        }
    }

    private void UpdatePlayerAnimation() {
        switch (state) {
            case PlayerState.Attacking:
                if (animations.GetAttackReady()) {
                    animations.AttackAnim(false);
                }
                break;
            case PlayerState.Walking:
                animations.WalkAnim();
                break;
            case PlayerState.Idle:
                animations.IdleAnim();
                break;
            default:
                animations.IdleAnim();
                break;
        }
    }

    private void UpdatePlayerMovement() {
        // Switch for player movement
        switch (state) {
            // If player is dead, stop all actions (excluding death)
            case PlayerState.Dying:
                return;
            // If any player type is stunned, prevent movement
            case PlayerState.Stunned:
                return;
            case PlayerState.Walking:
                Move();
                break;
            default:
                break;
        }
    }

    private void Move() {
        // Swordmaster cannot move while attacking
        if (type == PlayerType.Swordmaster && state == PlayerState.Attacking)
            return;

        rb.MovePosition(rb.position + 
                new Vector2(movement.x * horizontalSpeedMultiplier, movement.y * verticalSpeedMultiplier) * playerSpeed * Time.deltaTime);
        
    }

    private void SetState(PlayerState newState) {
        if (state == newState) 
            return;

        state = newState;
        Debug.Log("New state: " + state);
    }
}
