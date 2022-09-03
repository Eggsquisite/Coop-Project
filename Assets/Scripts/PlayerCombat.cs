using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    InputAction _attackAction;

    [Header("Attack Properties")]
    private bool attackPressed;
    private bool switchWeapon;

    static bool IsDown(InputAction action) => action.phase == InputActionPhase.Performed;
    static bool IsUp(InputAction action) => action.phase == InputActionPhase.Canceled;

    public void OnMainAttack(InputAction.CallbackContext context) {
        if (context.performed)
            attackPressed = true;
        else if (context.canceled)
            attackPressed = false;
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context) {
        switchWeapon = context.started;
    }

    // SETTERS //////////////////////////////////////////////////////////////

    // Called at beginning of attack animation to disallow holding the button (Swordmaster)
    private void SetAttackPressed(int flag) {
        if (flag == 0) attackPressed = false;
        else if (flag == 1) attackPressed = true;
    }

    // GETTERS ///////////////////////////////////////////////////////////////
    public bool GetAttackPressed() { return attackPressed; }
    public bool GetSwitchWeapon() { return switchWeapon; }
}