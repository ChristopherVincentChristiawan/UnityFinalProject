using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
    public TextMeshProUGUI text;
    protected Animator animator;
    protected CharacterCombat combat;
    EnemyController enemyController;
    CharacterStats characterStats; 
    public bool hit = false;
    public bool guard = false;
    public bool EnemyHeavyAttack = false;

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
        if (enemyController.attack == true)
        {
            if (enemyController.someRandomNumber == 1)
            {
                text.text = "Punch";
                combat.OnEnemyAttack += AttackOne;
                enemyController.someRandomNumber = 0;
                //bisa ga kondisinya bergantung hanya sama enemyHeavyAttack?
            } 
            else if (enemyController.someRandomNumber == 2)
            {
                text.text = "Hook";
                combat.OnEnemyAttack += AttackTwo;
                enemyController.someRandomNumber = 0;
                EnemyHeavyAttack = true; //ini harus balik ke false, otherwise nyala trs, mau punch pun bakal jebol block.
                Invoke("resetEnemyHeavyAttack", 2f); //2 karena 0.5 terlalu cepat jadi player ga kena damage (status enemyheavy attack udah jadi false duluan karena paralel).
            }
            else if (enemyController.someRandomNumber == 3)
            {
                text.text = "Block";
                combat.OnEnemyAttack += block;
                enemyController.someRandomNumber = 0;
                guard = true;
                Invoke("resetEnemyGuard", 2f);
            }
        }

        if (hit == true)
        {
            animator.SetTrigger("hit");
            hit = false;
            Invoke("resetHit", 0.5f);
            if (characterStats.isDead == true)
            {
                Debug.Log("ENEMY DIES, EXECUTE ANIMATION");
                animator.SetTrigger("die");
                characterStats.isDead = false; //mutus loop

            }
        }
    }

    protected virtual void resetHit()
    {
        animator.ResetTrigger("hit");
    }

    protected virtual void AttackOne()
    {
        animator.SetTrigger("attackOne"); //cara setrigger biar mati??
        combat.OnEnemyAttack -= AttackOne;
        Invoke("resetAttackOne", 0.5f);
    }

    protected virtual void AttackTwo()
    {
        animator.SetTrigger("attackTwo");
        combat.OnEnemyAttack -= AttackTwo;
        Invoke("resetAttackTwo", 0.5f);
    }

    protected virtual void block()
    {
        animator.SetTrigger("block");
        combat.OnEnemyAttack -= block;
        Invoke("resetBlock", 0.5f);
        Debug.Log("block executed");
    }

    void resetAttackOne()
    {
        animator.ResetTrigger("attackOne");
        // Debug.Log("reset attackOne done");
    }

    void resetAttackTwo()
    {
        animator.ResetTrigger("attackTwo");
        // Debug.Log("reset attackTwo done");
    }

    void resetBlock()
    {
        animator.ResetTrigger("block");
        Debug.Log("reset block done");
    }

    void resetEnemyHeavyAttack()
    {
        EnemyHeavyAttack = false;
    }

    void resetEnemyGuard()
    {
        guard = false;
    }
}
