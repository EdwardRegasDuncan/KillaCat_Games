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
        _AttackDamage = 50f;
        _AttackSpeed = 2.0f;
        _Range = 5f;
        _Armour = 0f;
        _ArmourPiercing = 50f;
    }
    
}
