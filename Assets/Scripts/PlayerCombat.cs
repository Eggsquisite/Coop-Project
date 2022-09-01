using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{

    [Header("Attack Properties")]
    private bool isAttacking;
    private bool switchWeapon;

    public void OnMainAttack(InputAction.CallbackContext context) {
        isAttacking = context.action.triggered;
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context) {
        switchWeapon = context.action.triggered;
    }

    public bool GetIsAttack() { return isAttacking; }
    public bool GetSwitchWeapon() { return switchWeapon; }
}