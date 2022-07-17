using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AnimatorController;

public class Archer : UnitCore
{
    float _specialCharge;
    float _chargeRate;

    public GameObject _arrowPrefab;
    

    private void Awake()
    {
        _type = UNIT_TYPE.ARCHER;
        _HP = 75;
        _Speed = 5f;
        _AttackDamage = 5f;
        _AttackSpeed = 0.5f;
        _Range = 15f;
        _Armour = 5f;
        _ArmourPiercing = 0f;

        attack_event += ArrowParticle;
        attack_event += AttackAnim;

    }
    
    private void ArrowParticle(object sender, EventArgs e)
    {
        GameObject arrow = Instantiate(_arrowPrefab, transform.position, transform.rotation, transform);
        arrow.GetComponent<ProjectileController>()._target = _target;
    }
    private void AttackAnim(object sender, EventArgs e)
    {
        current_state = UNIT_STATE.ATTACK_SPELL;
        SoundManager.PlaySound(SoundManager.Sound.TropSpell, transform.localPosition);
    }
   

}
