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
        _AttackDamage = 20f;
        _AttackSpeed = 1f;
        _Range = 7f;
        _Armour = 10f;
        _ArmourPiercing = 0f;
    }
}
