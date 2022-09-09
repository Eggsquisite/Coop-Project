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

    [Header("Hit Properties")]
    private bool isInvincible;

    static bool IsDown(InputAction action) => action.phase == InputActionPhase.Performed;
    static bool IsUp(InputAction action) => action.phase == InputActionPhase.Canceled;

    // SETTERS //////////////////////////////////////////////////////////////
    public void SetInvincible(bool flag) { 
        isInvincible = flag;
    }

    // GETTERS ///////////////////////////////////////////////////////////////
    public bool GetAttackPressed() { return attackPressed; }
    public bool GetSwitchWeapon() { return switchWeapon; }
}