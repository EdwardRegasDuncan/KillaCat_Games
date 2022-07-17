using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : UnitCore
{
    float _specialCharge;
    float _chargeRate;
    
    private void Awake()
    {
        _type = UNIT_TYPE.KNIGHT;
        _HP = 100;
        _Speed = 7f;
        _AttackDamage = 20f;
        _AttackSpeed = 1.0f;
        _Range = 10f;
        _Armour = 30f;
        _ArmourPiercing = 10f;

        attack_event += AttackAnim;

    }

    private void AttackAnim(object sender, EventArgs e)
    {
        current_state = AnimatorController.UNIT_STATE.ATTACK_MELEE;
        SoundManager.PlaySound(SoundManager.Sound.TropAttack, transform.localPosition);
    }    

}
