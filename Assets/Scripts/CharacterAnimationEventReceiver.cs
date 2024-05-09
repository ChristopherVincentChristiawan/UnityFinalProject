using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEventReceiver : MonoBehaviour
{
    public CharacterCombat combat;
    public void AttackHitEvent() //Dipanggil dari karakter, pas dia setting moment of impact.
    {
        combat.AttackHitAnimationEvent();
    }

    public void EnemyAttackHitEvent() //Dipanggil dari karakter, pas dia setting moment of impact.
    {
        combat.EnemyAttackHitAnimationEvent();
    }
}
