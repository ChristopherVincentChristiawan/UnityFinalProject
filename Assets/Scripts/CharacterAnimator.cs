using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
    public bool hit = false;
    protected Animator animator;
    protected CharacterCombat combat;
    PlayerController playerController;
    EnemyController enemyController;
    CharacterStats characterStats; 

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        combat = GetComponent<CharacterCombat>();
        characterStats = GetComponent<CharacterStats>();
        enemyController = FindObjectOfType<EnemyController>();
    }

    protected virtual void Update()
    {
        animator.SetBool("inCombat", combat.InCombat);
        playerController = FindObjectOfType<PlayerController>();
        // Debug.Log(playerController.CrossPunch);

        if (playerController.CrossPunch == true)
        { 
            combat.OnAttack += OnAttack;
            // playerController.CrossPunch = false;
        }
        else if (playerController.Punch == true)
        {
            combat.OnAttack += PunchAttack;
            // playerController.Punch = false;
        }
        else if (playerController.Block == true)
        {
            combat.OnAttack += BlockAttack;
            // playerController.Punch = false;
        }
        else if (hit == true)
        {
            Debug.Log("Character is HIT");
            animator.SetTrigger("hit");
            hit = false;
            Invoke("resetHit", 0.5f);
            if (characterStats.isDead == true)
            {
                // Debug.Log("THE PLAYER DIES AND SHOULD EXECUTE ANIMATION");
                animator.SetTrigger("die");
                characterStats.isDead = false; //mutus loop
            }
        }
    }

    protected virtual void resetHit()
    {
        animator.ResetTrigger("hit");
    }

    protected virtual void OnAttack()
    {
        animator.SetTrigger("attack"); //cara setrigger biar mati??
        combat.OnAttack -= OnAttack;
        Invoke("resetPunch", 0.5f);
    }

    protected virtual void PunchAttack()
    {
        animator.SetTrigger("punch");
        combat.OnAttack -= PunchAttack;
        Invoke("resetAttack", 0.5f);
    }

    protected virtual void BlockAttack()
    {
        animator.SetTrigger("block");
        combat.OnAttack -= BlockAttack;
        Invoke("resetBlock", 0.5f);
    }

    void resetAttack()
    {
        animator.ResetTrigger("attack");
        // Debug.Log("reset attack done");
    }

    void resetPunch()
    {
        animator.ResetTrigger("punch");
        // Debug.Log("reset punch done");
    }

    void resetBlock()
    {
        animator.ResetTrigger("block");
        // Debug.Log("reset punch done");
    }

}
