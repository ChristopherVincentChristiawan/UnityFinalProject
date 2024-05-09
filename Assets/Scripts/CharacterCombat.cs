using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    public float attackSpeed = 1f;
    private float attackCooldown = 0f;
    const float combatCooldown = 1; //cooldown animasi
    float lastAttackTime;

    public bool InCombat { get; private set; }
    public event System.Action OnAttack;
    public event System.Action OnEnemyAttack;

    CharacterStats myStats;
    CharacterStats opponentStats;
    PlayerController playerController;
    EnemyAnimator enemyAnimator;
    CharacterAnimator characterAnimator;

    void Start()
    {
        myStats = GetComponent<CharacterStats>();
        enemyAnimator = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAnimator>();
        characterAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterAnimator>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        // Debug.Log("Enemy Hook Status: " + enemyAnimator.EnemyHeavyAttack);
        Debug.Log("Player PreserveStats Status: " + playerController.PreserveStats);
        attackCooldown -= Time.deltaTime;
        if (Time.time - lastAttackTime > combatCooldown)
        {
            InCombat = false;
        }
    }

    public void Attack (CharacterStats targetStats) //ngurangin stats.
    {
        if (attackCooldown <= 0f)
        {
            opponentStats = targetStats;
            if (OnAttack != null) //gaboleh null
                OnAttack();
            attackCooldown = 1f / attackSpeed;
            InCombat = true;
            lastAttackTime = Time.time;
        } 
    }

    public void EnemyAttack (CharacterStats targetStats) //ngurangin stats.
    {
        if (attackCooldown <= 0f)
        {
            opponentStats = targetStats;
            if (OnEnemyAttack != null) //gaboleh null
                OnEnemyAttack();
            attackCooldown = 1f / attackSpeed;
            InCombat = true;
            lastAttackTime = Time.time;
        } 
    }

    public void AttackHitAnimationEvent() //dari player
    {
        Debug.Log("Enemy Block Status: " + playerController.Block);
        if (enemyAnimator.guard == false | playerController.HeavyAttack == true) //waktu enemy ga guard atau player pakai heavy maka damage inflicted.
        {
            opponentStats.TakeDamage(myStats.damage.GetValue()); //menyakiti enemy
            //cara cari tahu opponentsnya yg mana? ini method recycleable
            //initiate animasi stunned
            ////panggil animasi enemy stunned karena mereka yg receive damage
            enemyAnimator.hit = true; //set animasi player stunned karena tersakiti.

            if (opponentStats.currentHealth <= 0)
            {
                InCombat = false;
                opponentStats.Die();
            }
        } 
    }

    public void EnemyAttackHitAnimationEvent() //dari enemy ke player. Sampai animasi mencapai batas tertentu maka dia execute function ini, lewat CharacterAnimationReceiver.
    {
        Debug.Log("Player Block Status: " + playerController.Block);
        if (playerController.PreserveStats == false | enemyAnimator.EnemyHeavyAttack == true) //membuat bisa terkena damage
        {
            opponentStats.TakeDamage(myStats.damage.GetValue());
            //cara cari tahu opponentsnya yg mana? ini method recycleable
            //initiate animasi stunned
            //panggil animasi player stunned karena mereka yg receive damage
            characterAnimator.hit = true;

            if (opponentStats.currentHealth <= 0)
            {
                InCombat = false;
                opponentStats.Die();
            }
        } 
    }
}
