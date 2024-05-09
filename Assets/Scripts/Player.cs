using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class Player : Interactable 
{
    PlayerManager playerManager;
    CharacterStats myStats;

    void Start()
    {
        playerManager = PlayerManager.instance;
        myStats = GetComponent<CharacterStats>();
    }

    public override void Interact() //interaksi dengan self, maka kurangin myStats (enemyStats).
    {
        base.Interact();
        // Atack the enemy
        CharacterCombat playerCombat = playerManager.enemy.GetComponent<CharacterCombat>();
        if (playerCombat != null)
        {
            playerCombat.EnemyAttack(myStats);
            // Debug.Log("HELLO FROM Enemy");
        }
    }
}
