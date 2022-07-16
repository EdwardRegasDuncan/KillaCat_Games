using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : UnitCore
{
    float _specialCharge;
    float _chargeRate;

    private void Awake()
    {
        _HP = 100;
        _Speed = 7f;
        _AttackDamage = 20f;
        _AttackSpeed = 1.0f;
        _Range = 2f;
        _Armour = 30f;
        _ArmourPiercing = 10f;
    }

}
