using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : UnitCore
{
    float _specialCharge;
    float _chargeRate;

    private void Awake()
    {
        _HP = 75;
        _Speed = 5f;
        _AttackDamage = 5f;
        _AttackSpeed = 0.5f;
        _Range = 15f;
        _Armour = 5f;
        _ArmourPiercing = 0f;
    }
}
