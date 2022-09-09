using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState {
        Idle,
        Walking,
        Running,
        Sprinting,
        Jumping,
        Dashing,
        Rolling,
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
    private PlayerMovement move;
    private PlayerAnimations animations;
    
    [Header("Input Actions Variables")]
    private PlayerInputControls inputControls;
    private InputAction movementInput;

    [Header("Character Specific Values")]
    [SerializeField] private PlayerType type;
    private PlayerState state = PlayerState.Idle;

    // Gunslinger attack is independent of movement
    private bool isFiring;
    private Vector2 moveVector;

    [Header("Input Values")]
    [SerializeField] private float runBufferMaxTime;
    private bool mainAttackPressed, secondaryAttackPressed, switchWeapon;
    private bool jumpPressed, runHeld, sprintHeld, rollPressed;
    private Coroutine oneRoutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        move = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
        animations = GetComponent<PlayerAnimations>();
        inputControls = new PlayerInputControls();
    }

    private void OnEnable() {
        movementInput = inputControls.Player.Movement;
        movementInput.Enable();

        inputControls.Player.MainAttack.performed += OnMainAttack;
        inputControls.Player.MainAttack.canceled += OnMainAttack;
        inputControls.Player.MainAttack.Enable();

        inputControls.Player.Jump.performed += OnJump;
        inputControls.Player.Jump.Enable();

        inputControls.Player.Roll.performed += OnRoll;
        inputControls.Player.Roll.canceled += OnRoll;
        inputControls.Player.Roll.Enable();

        inputControls.Player.Run.performed += OnRun;
        inputControls.Player.Run.canceled += CancelRun;
        inputControls.Player.Run.Enable();

        inputControls.Player.Sprint.performed += OnSprint;
        inputControls.Player.Sprint.canceled += CancelSprint;
        inputControls.Player.Sprint.Enable();

        inputControls.Player.SwitchWeapon.performed += OnSwitchWeapon;
        inputControls.Player.SwitchWeapon.Enable();
    }

    private void OnDisable() {
        movementInput.Disable();
        inputControls.Player.MainAttack.Disable();
        inputControls.Player.Roll.Disable();
        inputControls.Player.Jump.Disable();
        inputControls.Player.Run.Disable();
        inputControls.Player.Sprint.Disable();
        inputControls.Player.SwitchWeapon.Disable();
    }

    // EVENTS ////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnMove(InputAction.CallbackContext context) { moveVector = context.ReadValue<Vector2>().normalized; }
    private void OnRun(InputAction.CallbackContext context) { runHeld = true; }
    private void CancelRun(InputAction.CallbackContext context) { runHeld = false; }
    private void OnSprint(InputAction.CallbackContext context) { if (runHeld) sprintHeld = true; }
    private void CancelSprint(InputAction.CallbackContext context) { sprintHeld = false; }
    private void OnMainAttack(InputAction.CallbackContext context) { mainAttackPressed = context.action.triggered; }
    private void OnRoll(InputAction.CallbackContext context) { rollPressed = context.action.triggered; }
    private void OnJump(InputAction.CallbackContext context) { jumpPressed = context.action.triggered; }
    private void OnSwitchWeapon(InputAction.CallbackContext context) { switchWeapon = context.started; }

    void Update() {
        UpdatePlayerState();
        UpdatePlayerAnimation();
    }

    void FixedUpdate()
    {
        UpdatePlayerMovement();
    }

    private void UpdatePlayerState() {
        switch(type) {
            case PlayerType.Swordmaster:
                // If player input is > 0, player is attempting to move
                if (CheckAttackTriggered())
                    SetState(PlayerState.Attacking);
                else if (CheckRollTriggered())
                    SetState(PlayerState.Rolling);
                else if (!animations.GetIsAttacking() && !animations.GetIsRolling()) {
                    if (moveVector.x != 0 || moveVector.y != 0) {
                        if (!runHeld && !sprintHeld)
                            SetState(PlayerState.Walking);
                        else if (runHeld && !sprintHeld)
                            SetState(PlayerState.Running);
                        else if (sprintHeld)
                            SetState(PlayerState.Sprinting);
                    }
                    else if (moveVector.x == 0 && moveVector.y == 0) {
                        if (state == PlayerState.Running || state == PlayerState.Sprinting) {
                            CancelRoutine(oneRoutine);
                            oneRoutine = StartCoroutine(RunBuffer(runBufferMaxTime));
                        }
                        SetState(PlayerState.Idle);
                    }
                }
                break;

            case PlayerType.Gunslinger:
                // Gunslinger attack is independent from movement
                if (combat.GetAttackPressed() && !isFiring)
                    isFiring = true;
                else if (!combat.GetAttackPressed() && isFiring)
                    isFiring = false;

                // If player input is > 0, player is attempting to move
                if (moveVector.x != 0 || moveVector.y != 0)
                    SetState(PlayerState.Walking);
                else if (moveVector.x == 0 && moveVector.y == 0)
                    SetState(PlayerState.Idle);

                break;
        }
    }

    private void UpdatePlayerAnimation() {
        switch (state) {
            case PlayerState.Attacking:
                if (CheckAttackTriggered())
                    animations.AttackAnim(false);
                break;
            case PlayerState.Walking:
                animations.WalkAnim();
                break;
            case PlayerState.Running:
                animations.RunAnim();
                CancelRoutine(oneRoutine);
                break;
            case PlayerState.Sprinting:
                animations.SprintAnim();
                CancelRoutine(oneRoutine);
                break;
            case PlayerState.Idle:
                animations.IdleAnim();
                break;
            case PlayerState.Rolling:
                if (CheckRollTriggered())
                    animations.RollAnim();
                break;
            case PlayerState.Stunned:
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
            // Swordmaster cannot move while attacking
            if (type == PlayerType.Swordmaster && state == PlayerState.Attacking)
                return;
            else
                move.Walk(moveVector);
                break;
            case PlayerState.Running:
                move.Run(moveVector);
                break;
            case PlayerState.Sprinting:
                move.Sprint(moveVector);
                break;
            case PlayerState.Rolling:
                move.Roll(moveVector);
                break;
            default:
                break;
        }
    }

    private bool CheckAttackTriggered() {
        if (animations.GetAttackReady() && mainAttackPressed) { return true; }
        return false;
    }

    private bool CheckRollTriggered() { 
        if (animations.GetAttackReady() && rollPressed) {
            Debug.Log("True");
            return true; }
        return false;
    }

    IEnumerator RunBuffer(float maxTimer) {
        yield return new WaitForSeconds(maxTimer);
        CancelRunSprint();
    }

    private void CancelRunSprint() {
        runHeld = false;
        sprintHeld = false;
    }

    private void CancelRoutine(Coroutine routine) {
        if (routine != null)
            StopCoroutine(oneRoutine);
    }

    // SETTERS ///////////////////////////////////////////////////////////////////

    private void SetState(PlayerState newState) {
        if (state == newState) 
            return;

        state = newState;
        Debug.Log("New State: " + state);
    }

    // Called at beginning of attack animation to disallow holding the button (Swordmaster)
    private void SetMainAttackPressed(int flag) {
        if (flag == 0) mainAttackPressed = false;
        else if (flag == 1) mainAttackPressed = true;
    }
    private void SetSecondaryAttackPressed(int flag) {
        if (flag == 0) secondaryAttackPressed = false;
        else if (flag == 1) secondaryAttackPressed = true;
    }
}
