using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : UnitCore
{
    float _specialCharge;
    float _chargeRate;

    private void Awake()
    {
        _HP = 50;
        _Speed = 3f;
        _AttackDamage = 30f;
        _AttackSpeed = 2.5f;
        _Range = 5f;
        _Armour = 0f;
        _ArmourPiercing = 20f;
    }
}
