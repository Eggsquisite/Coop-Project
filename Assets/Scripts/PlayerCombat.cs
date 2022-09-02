using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    InputAction _attackAction;

    [Header("Attack Properties")]
    private bool isAttacking;
    private bool switchWeapon;

    static bool IsDown(InputAction action) => action.phase == InputActionPhase.Performed;
    static bool IsUp(InputAction action) => action.phase == InputActionPhase.Canceled;

    void Update() {
        // if (IsDown(_attackAction)) 
        //     isAttacking = true;
        // else if (IsUp(_attackAction))
        //     isAttacking = false;
    }

    public void OnMainAttack(InputAction.CallbackContext context) {
        if (context.performed)
            isAttacking = true;
        else if (context.canceled)
            isAttacking = false;
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context) {
        switchWeapon = context.started;
    }

    public bool GetIsAttack() { return isAttacking; }
    public bool GetSwitchWeapon() { return switchWeapon; }
}