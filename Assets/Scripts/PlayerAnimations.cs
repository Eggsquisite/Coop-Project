using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("Components")]
    private Animator anim;
    private RuntimeAnimatorController ac;

    [Header("Animation States")]
    private string animState, currentState;

    [Header("Attack Animation Variables")]
    [SerializeField] private float attackTimerBuffer;
    private int attackFlow = 1;
    private bool isAttacking, attackResetWait, attackReady = true;
    private float attackTimer, attackLength;
    private Coroutine oneRoutine;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ac = anim.runtimeAnimatorController;
    }

    // Animation Helper Functions ////////////////////////////////////////
    private void PlayAnimation(string newAnim) {
        AnimHelper.ChangeAnimationState(anim, ref currentState, newAnim);
    }
    private void ReplayAnimation(string newAnim) {
        AnimHelper.ReplayAnimation(anim, ref currentState, newAnim);
    }
    public float GetAnimationLength(string newAnim) {
        return AnimHelper.GetAnimClipLength(ac, newAnim);
    }

    public void IdleAnim() { PlayAnimation(PlayerAnims.IDLE); }

    public void WalkAnim() { PlayAnimation(PlayerAnims.WALK); }

    public void AttackAnim(bool specialAttackflag) {
        if (!attackReady)
            return;
        
        if (specialAttackflag)
            attackFlow = 5;
        //attackFlow = specialAttackflag ? 5: attackFlow + 1;

        switch (attackFlow) {
            case 1:
                attackLength = GetAnimationLength(PlayerAnims.ATTACK_1);
                //Debug.Log("Anim 1");
                PlayAnimation(PlayerAnims.ATTACK_1);

                // if (oneRoutine != null)
                //     StopCoroutine(ResetAttack(attackLength));
                // oneRoutine = StartCoroutine(ResetAttack(attackLength));
                break;
            case 2:
                //Debug.Log("Anim 2");
                attackLength = GetAnimationLength(PlayerAnims.ATTACK_2);
                PlayAnimation(PlayerAnims.ATTACK_2);
                
                // if (oneRoutine != null)
                //     StopCoroutine(ResetAttack(attackLength));
                // oneRoutine = StartCoroutine(ResetAttack(attackLength));
                break;
            case 3:
                //Debug.Log("Anim 3");
                attackLength = GetAnimationLength(PlayerAnims.ATTACK_3);
                PlayAnimation(PlayerAnims.ATTACK_3);

                // if (oneRoutine != null)
                //     StopCoroutine(ResetAttack(attackLength));
                // oneRoutine = StartCoroutine(ResetAttack(attackLength));
                break;
            case 5:
                attackLength = GetAnimationLength(PlayerAnims.ATTACK_4);
                PlayAnimation(PlayerAnims.ATTACK_4);

                // if (oneRoutine != null)
                //     StopCoroutine(ResetAttack(attackLength));
                // oneRoutine = StartCoroutine(ResetAttack(attackLength));
                break;
            default:
                break;
        }
    }

    // ANIMATION EVENTS ////////////////////////////////////////////
    private void BeginAttack(int flow) {
        attackFlow = flow;
        isAttacking = true;
        attackReady = false;
    }
    private void EndAttack() {
        attackFlow = 1;
        isAttacking = false;
        attackReady = true;
    }
    private void SetAttackReady(int flag) { 
        if (flag == 0) {
            attackReady = false;
            Debug.Log("Setting false");
        }
        else if (flag == 1) attackReady = true;
    }


    public bool GetIsAttacking() { return isAttacking; }
    public bool GetAttackReady() { return attackReady; }
}
