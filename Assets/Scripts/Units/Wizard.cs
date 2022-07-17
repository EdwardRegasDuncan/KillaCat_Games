using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : UnitCore
{
    float _specialCharge;
    float _chargeRate;
    public GameObject _magicBlast;
    [SerializeField] Transform _spawnPoint;

    private void Awake()
    {
        _type = UNIT_TYPE.WIZARD;
        _HP = 50;
        _Speed = 15f;
        _AttackDamage = 0f;
        _AttackSpeed = 2.0f;
        _Range = 100f;
        _Armour = 0f;
        _ArmourPiercing = 50f;

        attack_event += MagicParticle;
        attack_event += AttackAnim;

    }
    
    private void MagicParticle(object sender, EventArgs e)
    {
        GameObject arrow = Instantiate(_magicBlast, _spawnPoint.position, transform.rotation, transform);
        arrow.GetComponent<ProjectileController>()._target = _target;
    }
    
    private void AttackAnim(object sender, EventArgs e)
    {
        current_state = AnimatorController.UNIT_STATE.ATTACK_SPELL;
        SoundManager.PlaySound(SoundManager.Sound.TropSpell, transform.localPosition);
    }
}
