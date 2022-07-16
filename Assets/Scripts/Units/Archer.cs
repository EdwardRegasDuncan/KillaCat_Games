using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : UnitCore
{
    float _specialCharge;
    float _chargeRate;

    public GameObject _arrowPrefab;

    private void Awake()
    {
        _HP = 75;
        _Speed = 5f;
        _AttackDamage = 5f;
        _AttackSpeed = 0.5f;
        _Range = 15f;
        _Armour = 5f;
        _ArmourPiercing = 0f;

        attack_event += ArrowParticle;

    }
    
    private void ArrowParticle(object sender, EventArgs e)
    {
        GameObject arrow = Instantiate(_arrowPrefab, transform.position, Quaternion.identity);
        arrow.GetComponent<ArrowController>()._target = _target;
    }
    
}
